using System.Text.Json;
using ConnectX.Services.DND.Catalogs;
using Microsoft.AspNetCore.Components.Forms;

namespace ConnectX.Web.Components.Pages.DND.Catalogs.Items;

public enum ItemUploadStatus { Pending, Uploading, Created, Failed }

public sealed class ItemUploadItem(IBrowserFile file)
{
    public IBrowserFile File { get; } = file;
    public ItemUploadStatus Status { get; internal set; }
    public string? Error { get; internal set; }
    public Guid? ItemId { get; internal set; }
}

public sealed class ItemUploadBatch
{
    public const int MaximumFiles = 500;
    public IReadOnlyList<ItemUploadItem> Items { get; private set; } = [];
    public bool IsRunning { get; private set; }
    public bool HasPending => Items.Any(x => x.Status == ItemUploadStatus.Pending);

    public void Select(IReadOnlyList<IBrowserFile> files)
    {
        if (IsRunning) return;
        Items = [];
        if (files.Count > MaximumFiles) throw new ArgumentException($"Choose at most {MaximumFiles} files at a time.");
        Items = files.Select(file =>
        {
            var item = new ItemUploadItem(file);
            if (!file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) item.Error = "Choose a .json file.";
            else if (file.Size is <= 0 or > ItemUploadReader.MaximumBytes) item.Error = "File must be nonempty and no larger than 1 MB.";
            if (item.Error is not null) item.Status = ItemUploadStatus.Failed;
            return item;
        }).ToList();
    }

    public async Task RunAsync(Func<IBrowserFile, CancellationToken, Task<Guid>> add,
        Func<Task> progress, Action<Exception, string> logError, CancellationToken cancellationToken = default)
    {
        if (IsRunning) return;
        IsRunning = true;
        try
        {
            foreach (var item in Items.Where(x => x.Status == ItemUploadStatus.Pending))
            {
                cancellationToken.ThrowIfCancellationRequested();
                item.Status = ItemUploadStatus.Uploading;
                await progress();
                try
                {
                    item.ItemId = await add(item.File, cancellationToken);
                    item.Status = ItemUploadStatus.Created;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { item.Status = ItemUploadStatus.Pending; throw; }
                catch (Exception ex)
                {
                    item.Status = ItemUploadStatus.Failed;
                    item.Error = ex switch
                    {
                        JsonException => "Invalid item JSON. Check its format and nesting.",
                        ArgumentException or ItemAlreadyExistsException or UnauthorizedAccessException => ex.Message,
                        IOException => "Could not read the file. Select it again to retry.",
                        _ => "Could not create the item. Select the file again to retry."
                    };
                    if (ex is not (JsonException or ArgumentException or ItemAlreadyExistsException or UnauthorizedAccessException or IOException))
                        logError(ex, item.File.Name);
                    if (ex is UnauthorizedAccessException)
                    {
                        foreach (var remaining in Items.Where(x => x.Status == ItemUploadStatus.Pending))
                        {
                            remaining.Status = ItemUploadStatus.Failed;
                            remaining.Error = "Not processed: DungeonMaster authorization is required.";
                        }
                        break;
                    }
                }
                await progress();
            }
        }
        finally { IsRunning = false; }
    }
}
