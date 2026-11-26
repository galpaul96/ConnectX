using System.Text.Json;
using System.Text.Json.Nodes;
using ConnectX.Domain.Entities.DND.Items;
using ConnectX.Domain.Services.DND.Catalogs.Items;
using ConnectX.Ef;
using ConnectX.Services.DND.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using Xunit;

namespace ConnectX.Items.Tests;

public class ItemFlowTests
{
    [Fact]
    public void Relational_model_has_only_parent_and_component_tables_with_explicit_json_storage()
    {
        using var context = new EfContext(new DbContextOptionsBuilder<EfContext>()
            .UseNpgsql("Host=localhost;Database=model_only;Username=unused").Options);
        var model = context.Model;
        Assert.False(context.Database.HasPendingModelChanges());
        var sql = context.GetService<Microsoft.EntityFrameworkCore.Migrations.IMigrator>()
            .GenerateScript("20261126144902_SpellCatalog", "20261126180000_ItemCatalog");
        Assert.Contains("CREATE TABLE \"Items\"", sql);
        Assert.Contains("CREATE TABLE \"ItemComponents\"", sql);
        Assert.DoesNotContain("DROP TABLE", sql);
        Assert.DoesNotContain("CREATE TABLE \"Spells\"", sql);
        Assert.Equal(new[] { "ItemComponents", "Items", "SpellComponents", "Spells" }, model.GetEntityTypes().Select(x => x.GetTableName()).Order().ToArray());
        Assert.DoesNotContain(model.GetEntityTypes(), x => x.IsOwned());
        var component = model.FindEntityType(typeof(ItemComponent))!;
        Assert.Equal("jsonb", component.FindProperty(nameof(ItemComponent.Value))!.GetColumnType());
        Assert.Equal(typeof(JsonElement), component.FindProperty(nameof(ItemComponent.Value))!.ClrType);
        Assert.Equal("text", component.FindProperty(nameof(ItemComponent.Html))!.GetColumnType());
        Assert.Single(component.GetNavigations());
    }

    [Fact]
    public async Task All_310_records_round_trip_with_ordered_lossless_components_and_compact_list_queries()
    {
        await using var database = await TestDatabase.CreateAsync();
        var service = database.Service;
        var sources = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "items"), "*.json")
            .Where(x => !Path.GetFileName(x).StartsWith('_')).Order().ToList();
        Assert.Equal(310, sources.Count);
        var expected = new List<(Guid Id, ItemWriteDto Input)>();
        foreach (var source in sources)
        {
            var input = ReadItem(source);
            expected.Add((await service.CreateAsync(input, cancellationToken: TestContext.Current.CancellationToken), input));
        }
        database.Context.ChangeTracker.Clear();
        database.Commands.Clear();
        var first = await service.GetPageAsync(1, 24, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(310, first.TotalCount);
        Assert.Equal(13, first.TotalPages);
        Assert.Equal(24, first.Items.Count);
        Assert.Equal(4, typeof(ItemSummaryDto).GetProperties().Length);
        Assert.Empty(database.Context.ChangeTracker.Entries());
        if (database.IsPostgres)
        {
            Assert.Contains(database.Commands, sql => sql.Contains("ItemComponents", StringComparison.Ordinal));
            Assert.DoesNotContain(database.Commands, sql => sql.Contains("\"Html\"", StringComparison.Ordinal));
            Assert.DoesNotContain(database.Commands, sql => sql.Contains("\"StorageFile\"", StringComparison.Ordinal));
            Assert.Contains(database.Commands, sql => sql.Contains("LIMIT", StringComparison.Ordinal));
        }
        foreach (var entry in first.Items)
        {
            var input = expected.Single(x => x.Id == entry.Id).Input;
            Assert.Equal(input.Components.First(x => x.Type == "itemType").Value.GetString(), entry.Item.ItemType);
            var cost = input.Components.FirstOrDefault(x => x.Type == "cost");
            Assert.Equal(cost?.Value.GetProperty("text").GetString(), entry.Item.Cost);
        }
        var last = await service.GetPageAsync(int.MaxValue, 24, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(13, last.PageNumber);
        Assert.Equal(22, last.Items.Count);
        Assert.Empty(first.Items.Select(x => x.Id).Intersect(last.Items.Select(x => x.Id)));
        Assert.Contains((await service.GetPageAsync(search: " LONGSWORD ", cancellationToken: TestContext.Current.CancellationToken)).Items, x => x.Item.Name == "Longsword");
        Assert.Empty((await service.GetPageAsync(search: "no-such-item-7f6e2", cancellationToken: TestContext.Current.CancellationToken)).Items);
        foreach (var (id, input) in expected)
        {
            var detail = await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(detail);
            Assert.Equal(input.Name, detail.Name);
            Assert.Equal(input.SourceId, detail.SourceId);
            Assert.Equal(input.Slug, detail.Slug);
            Assert.Equal(input.StorageFile, detail.StorageFile);
            Assert.Equal(input.Components.Count, detail.Components.Count);
            for (var i = 0; i < input.Components.Count; i++)
            {
                var wanted = input.Components[i];
                var actual = detail.Components[i];
                Assert.Equal(i, actual.Position);
                Assert.Equal(wanted.Type, actual.Type);
                Assert.Equal(wanted.Label, actual.Label);
                Assert.Equal(wanted.Html, actual.Html);
                Assert.True(JsonNode.DeepEquals(JsonNode.Parse(wanted.Value.GetRawText()), JsonNode.Parse(actual.Value.GetRawText())), $"Changed {input.Slug}/{wanted.Type}");
            }
        }
    }

    [Fact]
    public async Task Crud_reconciles_removed_and_reintroduced_roles_and_preserves_all_json_kinds()
    {
        await using var database = await TestDatabase.CreateAsync();
        var service = database.Service;
        var values = new[] { "\"plain string\"", "123.5", "true", "false", "null", "[0,false,null,{\"nested\":[1,2]}]", "{\"material\":{\"cost\":0,\"consumed\":false}}" };
        var input = new ItemWriteDto
        {
            SourceId = "test-source", Slug = "test-item", Name = "Test 🪶", StorageFile = "test-item.json",
            Components = values.Select((v, i) => new ItemComponentDto
            {
                Type = $"extension-{i}", Label = $"Extension {i}", Value = JsonDocument.Parse(v).RootElement.Clone(), Html = new string('x', 7000)
            }).ToList()
        };
        var id = await service.CreateAsync(input, cancellationToken: TestContext.Current.CancellationToken);
        database.Context.ChangeTracker.Clear();
        var originalRows = await database.Context.Set<ItemComponent>().Where(x => x.ItemId == id).ToDictionaryAsync(x => x.Type, x => x.Id, cancellationToken: TestContext.Current.CancellationToken);
        var reduced = input with { Name = "Renamed", Components = input.Components.Take(1).ToList() };
        Assert.True(await service.UpdateAsync(id, reduced, cancellationToken: TestContext.Current.CancellationToken));
        database.Context.ChangeTracker.Clear();
        Assert.Single((await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken))!.Components);
        Assert.True(await service.UpdateAsync(id, input, cancellationToken: TestContext.Current.CancellationToken));
        database.Context.ChangeTracker.Clear();
        var restored = (await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken))!;
        Assert.Equal(values.Length, restored.Components.Count);
        Assert.Equal(input.Name, restored.Name);
        for (var i = 0; i < values.Length; i++)
            Assert.True(JsonNode.DeepEquals(JsonNode.Parse(values[i]), JsonNode.Parse(restored.Components[i].Value.GetRawText())));
        var restoredRows = await database.Context.Set<ItemComponent>().Where(x => x.ItemId == id).ToDictionaryAsync(x => x.Type, x => x.Id, cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(originalRows.OrderBy(x => x.Key), restoredRows.OrderBy(x => x.Key));
        Assert.True(await service.DeleteAsync(id, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Null(await service.GetByIdAsync(id, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(0, (await service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
        Assert.False(await service.DeleteAsync(id, cancellationToken: TestContext.Current.CancellationToken));
        Assert.False(await service.UpdateAsync(id, input, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Null(await service.GetByIdAsync(Guid.NewGuid(), cancellationToken: TestContext.Current.CancellationToken));
        Assert.False(await service.DeleteAsync(Guid.NewGuid(), cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Updating_identity_rejects_conflicts_without_changing_the_item()
    {
        await using var database = await TestDatabase.CreateAsync();
        var source = ReadItem(Path.Combine(AppContext.BaseDirectory, "items", "longsword.json"));
        var first = await database.Service.CreateAsync(source, TestContext.Current.CancellationToken);
        var secondInput = source with { SourceId = "another-item", Slug = "another-item", Name = "Another item" };
        var second = await database.Service.CreateAsync(secondInput, TestContext.Current.CancellationToken);
        await Assert.ThrowsAsync<ItemAlreadyExistsException>(() => database.Service.UpdateAsync(second, source, TestContext.Current.CancellationToken));
        Assert.Equal("Another item", (await database.Service.GetByIdAsync(second, TestContext.Current.CancellationToken))!.Name);
        Assert.True(await database.Service.UpdateAsync(second, secondInput with { Name = "Updated item" }, TestContext.Current.CancellationToken));
        Assert.Equal(source.Name, (await database.Service.GetByIdAsync(first, TestContext.Current.CancellationToken))!.Name);
    }

    [Fact]
    public async Task Rejects_invalid_writes_and_pages_and_observes_cancellation()
    {
        await using var database = await TestDatabase.CreateAsync();
        var service = database.Service;
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetPageAsync(0, cancellationToken: TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetPageAsync(pageSize: 101, cancellationToken: TestContext.Current.CancellationToken));
        using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.GetPageAsync(cancellationToken: cancelled.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.GetByIdAsync(Guid.NewGuid(), cancelled.Token));
        var input = ReadItem(Path.Combine(AppContext.BaseDirectory, "items", "longsword.json"));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(input with { Components = [input.Components[0], input.Components[0]] }, cancellationToken: TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(input with { Components = [new() { Type = "invalid", Label = "Invalid" }] }, cancellationToken: TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(input with { Name = " " }, cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(0, (await service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
    }

    internal static ItemWriteDto ReadItem(string filename)
    {
        using var json = JsonDocument.Parse(File.ReadAllText(filename));
        var root = json.RootElement;
        return new()
        {
            SourceId = root.GetProperty("id").GetString()!, Slug = root.GetProperty("slug").GetString()!,
            Name = root.GetProperty("name").GetString()!, StorageFile = root.GetProperty("storageFile").GetString()!,
            Components = root.GetProperty("components").EnumerateArray().Select((c, i) => new ItemComponentDto
            {
                Type = c.GetProperty("type").GetString()!, Label = c.GetProperty("label").GetString()!,
                Value = c.GetProperty("value").Clone(), Html = c.GetProperty("html").GetString()!, Position = i
            }).ToList()
        };
    }
}

internal sealed class TestDatabase : IAsyncDisposable
{
    public EfContext Context { get; }
    public ItemsService Service { get; }
    public bool IsPostgres { get; }
    public List<string> Commands { get; } = [];
    private TestDatabase()
    {
        var connection = Environment.GetEnvironmentVariable("CONNECTX_ITEM_TEST_POSTGRES");
        var options = new DbContextOptionsBuilder<EfContext>();
        IsPostgres = !string.IsNullOrEmpty(connection);
        if (IsPostgres)
        {
            // Always create a unique disposable database. Never migrate/drop the supplied database.
            var builder = new NpgsqlConnectionStringBuilder(connection) { Database = $"connectx_item_test_{Guid.NewGuid():N}" };
            options.UseNpgsql(builder.ConnectionString).AddInterceptors(new CaptureCommands(Commands));
        }
        else options.UseInMemoryDatabase($"item-test-{Guid.NewGuid():N}");
        Context = new EfContext(options.Options);
        Service = new ItemsService(new Repository(Context));
    }
    public static async Task<TestDatabase> CreateAsync()
    {
        var result = new TestDatabase();
        if (result.IsPostgres) await result.Context.Database.MigrateAsync();
        else await result.Context.Database.EnsureCreatedAsync();
        return result;
    }
    public async ValueTask DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }
    private sealed class CaptureCommands(List<string> commands) : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<System.Data.Common.DbDataReader>> ReaderExecutingAsync(
            System.Data.Common.DbCommand command, CommandEventData eventData,
            InterceptionResult<System.Data.Common.DbDataReader> result, CancellationToken cancellationToken = default)
        {
            commands.Add(command.CommandText);
            return ValueTask.FromResult(result);
        }
    }
}
