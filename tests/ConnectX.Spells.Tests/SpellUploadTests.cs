using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ConnectX.Domain.Services.DND.Catalogs.Spells;
using ConnectX.Services.DND.Catalogs;
using ConnectX.Web.Components.Pages.DND.Catalogs.Skills;
using ConnectX.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace ConnectX.Spells.Tests;

public class SpellUploadTests
{
    [Fact]
    public async Task Every_template_maps_and_can_be_added_through_the_upload_contract()
    {
        await using var db = await TestDatabase.CreateAsync();
        var files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "spell-catalog"), "*.json").Where(x => !Path.GetFileName(x).StartsWith('_')).ToList();
        Assert.Equal(470, files.Count);
        foreach (var file in files)
        {
            await using var stream = File.OpenRead(file);
            var input = await SpellUploadReader.ReadAsync(stream, Path.GetFileName(file), stream.Length, TestContext.Current.CancellationToken);
            var id = await db.Service.AddAsync(input, TestContext.Current.CancellationToken);
            var actual = await db.Service.GetByIdAsync(id, TestContext.Current.CancellationToken);
            Assert.Equal(input.Name, actual!.Name);
            Assert.Equal(input.Components.Count, actual.Components.Count);
        }
    }

    [Fact]
    public async Task Rejects_wrong_files_missing_fields_bad_shapes_duplicate_roles_and_oversized_streams()
    {
        var sample = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "spell-catalog", "fireball.json"));
        async Task Read(string content, string name = "spell.json", long? size = null)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await SpellUploadReader.ReadAsync(stream, name, size ?? stream.Length, TestContext.Current.CancellationToken);
        }
        await Assert.ThrowsAsync<ArgumentException>(() => Read(sample, "spell.txt"));
        await Assert.ThrowsAsync<ArgumentException>(() => Read("[]"));
        await Assert.ThrowsAsync<ArgumentException>(() => Read("{}"));
        await Assert.ThrowsAnyAsync<JsonException>(() => Read("{"));
        await Assert.ThrowsAsync<ArgumentException>(() => Read(sample, size: SpellUploadReader.MaximumBytes + 1));
        await Assert.ThrowsAsync<ArgumentException>(() => Read(new string(' ', SpellUploadReader.MaximumBytes + 1), size: 1));
        var input = SpellFlowTests.ReadSpell(Path.Combine(AppContext.BaseDirectory, "spell-catalog", "fireball.json"));
        Assert.Throws<ArgumentException>(() => SpellWriteValidation.Validate(input with { Components = [] }));
        Assert.Throws<ArgumentException>(() => SpellWriteValidation.Validate(input with { Components = [input.Components[0], input.Components[0]] }));
        Assert.Throws<ArgumentException>(() => SpellWriteValidation.Validate(input with { Components = [new() { Type = "level", Label = "Level", Value = JsonSerializer.SerializeToElement("wrong shape") }] }));
    }

    [Fact]
    public async Task Duplicate_add_is_rejected_and_next_valid_upload_still_succeeds()
    {
        await using var db = await TestDatabase.CreateAsync();
        var input = SpellFlowTests.ReadSpell(Path.Combine(AppContext.BaseDirectory, "spell-catalog", "fireball.json"));
        await db.Service.AddAsync(input, TestContext.Current.CancellationToken);
        await Assert.ThrowsAsync<SpellAlreadyExistsException>(() => db.Service.AddAsync(input, TestContext.Current.CancellationToken));
        await db.Service.AddAsync(input with { SourceId = "another", Slug = "another" }, TestContext.Current.CancellationToken);
        Assert.Equal(2, (await db.Service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
    }

    [Fact]
    public async Task Upload_requires_authenticated_role_and_current_database_membership()
    {
        await using var spells = await TestDatabase.CreateAsync();
        var collection = new ServiceCollection();
        collection.AddLogging();
        var identityDatabase = "upload-identity-" + Guid.NewGuid();
        collection.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(identityDatabase));
        collection.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        await using var services = collection.BuildServiceProvider();
        await using var scope = services.CreateAsyncScope();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var user = new ApplicationUser { UserName = "dungeonmaster" };
        Assert.True((await users.CreateAsync(user)).Succeeded);
        Assert.True((await roles.CreateAsync(new("DungeonMaster"))).Succeeded);
        var principal = new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, user.Id), new(ClaimTypes.Role, "DungeonMaster")], "test"));
        var auth = new TestAuthentication(principal);
        var upload = new SpellUploadService(auth, services.GetRequiredService<IServiceScopeFactory>(), spells.Service);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => upload.EnsureAuthorizedAsync());
        Assert.True((await users.AddToRoleAsync(user, "DungeonMaster")).Succeeded);
        await upload.EnsureAuthorizedAsync();
        using var file = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "spell-catalog", "fireball.json"));
        var id = await upload.AddAsync(file, "fireball.json", file.Length, TestContext.Current.CancellationToken);
        Assert.NotNull(await spells.Service.GetByIdAsync(id, TestContext.Current.CancellationToken));
        Assert.True((await users.RemoveFromRoleAsync(user, "DungeonMaster")).Succeeded);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => upload.EnsureAuthorizedAsync());
        foreach (var denied in new[] { new ClaimsPrincipal(new ClaimsIdentity()), new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, user.Id)], "test")) })
        {
            var blocked = new SpellUploadService(new TestAuthentication(denied), services.GetRequiredService<IServiceScopeFactory>(), spells.Service);
            using var empty = new MemoryStream();
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => blocked.AddAsync(empty, "file.json", 1, TestContext.Current.CancellationToken));
        }
    }
    [Fact]
    public async Task Create_button_is_visible_only_to_authenticated_dungeonmasters()
    {
        await using var db = await TestDatabase.CreateAsync();
        foreach (var kind in new[] { "anonymous", "member", "DungeonMaster" })
        {
            var identity = kind == "anonymous" ? new ClaimsIdentity() : new ClaimsIdentity(
                kind == "DungeonMaster" ? new[] { new Claim(ClaimTypes.Role, "DungeonMaster") } : [], "test");
            var collection = new ServiceCollection().AddLogging();
            collection.AddAuthorizationCore();
            collection.AddCascadingAuthenticationState();
            collection.AddSingleton<AuthenticationStateProvider>(new TestAuthentication(new(identity)));
            collection.AddSingleton<ISpellService>(db.Service);
            collection.AddSingleton<SpellUploadService>();
            await using var provider = collection.BuildServiceProvider();
            await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
            var html = await renderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await renderer.RenderComponentAsync<ConnectX.Web.Components.Pages.DND.Catalogs.Skills.Components.SpellCreateButton>(ParameterView.Empty);
                return output.ToHtmlString();
            });
            Assert.Equal(kind == "DungeonMaster", html.Contains("spell-create", StringComparison.Ordinal));
        }
    }
    private sealed class TestAuthentication(ClaimsPrincipal principal) : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(new AuthenticationState(principal));
    }
}
