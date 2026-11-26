using System.Text;
using ConnectX.Web.Components.Pages.DND.Catalogs.Items;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;

namespace ConnectX.Items.Tests;

public class ItemUploadBatchTests
{
    [Fact]
    public async Task Batch_creates_valid_files_continues_after_invalid_and_duplicate_files_and_never_repeats_successes()
    {
        await using var db = await TestDatabase.CreateAsync();
        var folder = Path.Combine(AppContext.BaseDirectory, "items");
        var first = new UploadFile("longsword.json", File.ReadAllText(Path.Combine(folder, "longsword.json")));
        var second = new UploadFile("backpack.json", File.ReadAllText(Path.Combine(folder, "backpack.json")));
        var batch = new ItemUploadBatch();
        batch.Select([first, new UploadFile("invalid.json", "{"), first, new UploadFile("wrong.txt", "ignored"), second]);
        var calls = 0;
        async Task<Guid> Add(IBrowserFile file, CancellationToken token)
        {
            calls++;
            await using var stream = file.OpenReadStream(ItemUploadReader.MaximumBytes, token);
            var input = await ItemUploadReader.ReadAsync(stream, file.Name, file.Size, token);
            return await db.Service.AddAsync(input, token);
        }
        await batch.RunAsync(Add, () => Task.CompletedTask, (_, _) => Assert.Fail("Unexpected upload failure"), TestContext.Current.CancellationToken);
        Assert.Equal(4, calls);
        Assert.Equal(2, batch.Items.Count(x => x.Status == ItemUploadStatus.Created));
        Assert.Equal(3, batch.Items.Count(x => x.Status == ItemUploadStatus.Failed));
        Assert.All(batch.Items.Where(x => x.Status == ItemUploadStatus.Failed), x => Assert.False(string.IsNullOrWhiteSpace(x.Error)));
        Assert.False(batch.HasPending);
        Assert.False(batch.IsRunning);
        await batch.RunAsync(Add, () => Task.CompletedTask, (_, _) => { }, TestContext.Current.CancellationToken);
        Assert.Equal(4, calls);
        Assert.Equal(2, (await db.Service.GetPageAsync(cancellationToken: TestContext.Current.CancellationToken)).TotalCount);
    }

    [Fact]
    public async Task Batch_stops_when_authorization_is_lost_and_rejects_excessive_selection()
    {
        var batch = new ItemUploadBatch();
        var file = new UploadFile("item.json", "{}");
        Assert.Throws<ArgumentException>(() => batch.Select(Enumerable.Repeat<IBrowserFile>(file, ItemUploadBatch.MaximumFiles + 1).ToList()));
        Assert.Empty(batch.Items);
        batch.Select([file, file]);
        var calls = 0;
        await batch.RunAsync((_, _) => { calls++; throw new UnauthorizedAccessException("Role removed"); },
            () => Task.CompletedTask, (_, _) => { }, TestContext.Current.CancellationToken);
        Assert.Equal(1, calls);
        Assert.All(batch.Items, x => Assert.Equal(ItemUploadStatus.Failed, x.Status));
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
