using System.Text;
using ConnectX.Web.Components.Pages.DND.Catalogs.Skills;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;

namespace ConnectX.Spells.Tests;

public class SpellUploadBatchTests
{
    [Fact]
    public async Task Batch_creates_valid_files_continues_after_invalid_and_duplicate_files_and_never_repeats_successes()
    {
        await using var db = await TestDatabase.CreateAsync();
        var folder = Path.Combine(AppContext.BaseDirectory, "spell-catalog");
        var first = new UploadFile("fireball.json", File.ReadAllText(Path.Combine(folder, "fireball.json")));
        var second = new UploadFile("revivify.json", File.ReadAllText(Path.Combine(folder, "revivify.json")));
        var batch = new SpellUploadBatch();
        batch.Select([first, new UploadFile("invalid.json", "{"), first, new UploadFile("wrong.txt", "ignored"), second]);
        var calls = 0;
        async Task<Guid> Add(IBrowserFile file, CancellationToken token)
        {
            calls++;
            await using var stream = file.OpenReadStream(SpellUploadReader.MaximumBytes, token);
            var input = await SpellUploadReader.ReadAsync(stream, file.Name, file.Size, token);
            return await db.Service.AddAsync(input, token);
        }
        await batch.RunAsync(Add, () => Task.CompletedTask, (_, _) => Assert.Fail("Unexpected upload failure"), TestContext.Current.CancellationToken);
        Assert.Equal(4, calls);
        Assert.Equal(2, batch.Items.Count(x => x.Status == SpellUploadStatus.Created));
        Assert.Equal(3, batch.Items.Count(x => x.Status == SpellUploadStatus.Failed));
        Assert.All(batch.Items.Where(x => x.Status == SpellUploadStatus.Failed), x => Assert.False(string.IsNullOrWhiteSpace(x.Error)));
        Assert.False(batch.HasPending);
        Assert.False(batch.IsRunning);
        await batch.RunAsync(Add, () => Task.CompletedTask, (_, _) => { }, TestContext.Current.CancellationToken);
        Assert.Equal(4, calls);
        Assert.Equal(2, (await db.Service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
    }

    [Fact]
    public async Task Batch_stops_when_authorization_is_lost_and_rejects_excessive_selection()
    {
        var batch = new SpellUploadBatch();
        var file = new UploadFile("spell.json", "{}");
        Assert.Throws<ArgumentException>(() => batch.Select(Enumerable.Repeat<IBrowserFile>(file, SpellUploadBatch.MaximumFiles + 1).ToList()));
        Assert.Empty(batch.Items);
        batch.Select([file, file]);
        var calls = 0;
        await batch.RunAsync((_, _) => { calls++; throw new UnauthorizedAccessException("Role removed"); },
            () => Task.CompletedTask, (_, _) => { }, TestContext.Current.CancellationToken);
        Assert.Equal(1, calls);
        Assert.All(batch.Items, x => Assert.Equal(SpellUploadStatus.Failed, x.Status));
        Assert.False(batch.IsRunning);
    }

    private sealed class UploadFile(string name, string content) : IBrowserFile
    {
        public string Name => name;
        public DateTimeOffset LastModified => DateTimeOffset.UtcNow;
        public long Size => Encoding.UTF8.GetByteCount(content);
        public string ContentType => "application/json";
        public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default) => new MemoryStream(Encoding.UTF8.GetBytes(content));
    }
}
