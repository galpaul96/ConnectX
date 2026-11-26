using System.Text.Json;
using ConnectX.Services.DND.Catalogs;
using Microsoft.AspNetCore.Components.Forms;

namespace ConnectX.Web.Components.Pages.DND.Catalogs.Skills;

public enum SpellUploadStatus { Pending, Uploading, Created, Failed }

public sealed class SpellUploadItem(IBrowserFile file)
{
    public IBrowserFile File { get; } = file;
    public SpellUploadStatus Status { get; internal set; }
    public string? Error { get; internal set; }
    public Guid? SpellId { get; internal set; }
}

public sealed class SpellUploadBatch
{
    public const int MaximumFiles = 500;
    public IReadOnlyList<SpellUploadItem> Items { get; private set; } = [];
    public bool IsRunning { get; private set; }
    public bool HasPending => Items.Any(x => x.Status == SpellUploadStatus.Pending);

    public void Select(IReadOnlyList<IBrowserFile> files)
    {
        if (IsRunning) return;
        Items = [];
        if (files.Count > MaximumFiles) throw new ArgumentException($"Choose at most {MaximumFiles} files at a time.");
        Items = files.Select(file =>
        {
            var item = new SpellUploadItem(file);
            if (!file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) item.Error = "Choose a .json file.";
            else if (file.Size is <= 0 or > SpellUploadReader.MaximumBytes) item.Error = "File must be nonempty and no larger than 1 MB.";
            if (item.Error is not null) item.Status = SpellUploadStatus.Failed;
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
            foreach (var item in Items.Where(x => x.Status == SpellUploadStatus.Pending))
            {
                cancellationToken.ThrowIfCancellationRequested();
                item.Status = SpellUploadStatus.Uploading;
                await progress();
                try
                {
                    item.SpellId = await add(item.File, cancellationToken);
                    item.Status = SpellUploadStatus.Created;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { item.Status = SpellUploadStatus.Pending; throw; }
                catch (Exception ex)
                {
                    item.Status = SpellUploadStatus.Failed;
                    item.Error = ex switch
                    {
                        JsonException => "Invalid spell JSON. Check its format and nesting.",
                        ArgumentException or SpellAlreadyExistsException or UnauthorizedAccessException => ex.Message,
                        IOException => "Could not read the file. Select it again to retry.",
                        _ => "Could not create the spell. Select the file again to retry."
                    };
                    if (ex is not (JsonException or ArgumentException or SpellAlreadyExistsException or UnauthorizedAccessException or IOException))
                        logError(ex, item.File.Name);
                    if (ex is UnauthorizedAccessException)
                    {
                        foreach (var remaining in Items.Where(x => x.Status == SpellUploadStatus.Pending))
                        {
                            remaining.Status = SpellUploadStatus.Failed;
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
