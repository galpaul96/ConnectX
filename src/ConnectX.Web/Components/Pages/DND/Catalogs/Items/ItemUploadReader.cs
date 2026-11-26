using System.Text.Json;
using ConnectX.Domain.Services.DND.Catalogs.Items;

namespace ConnectX.Web.Components.Pages.DND.Catalogs.Items;

public static class ItemUploadReader
{
    public const int MaximumBytes = 1024 * 1024;

    public static async Task<ItemWriteDto> ReadAsync(Stream stream, string filename, long size, CancellationToken cancellationToken = default)
    {
        if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Choose a JSON file containing one item.");
        if (size is <= 0 or > MaximumBytes) throw new ArgumentException("Choose a nonempty file no larger than 1 MB.");
        using var buffer = new MemoryStream();
        var bytes = new byte[8192];
        int read;
        while ((read = await stream.ReadAsync(bytes, cancellationToken)) > 0)
        {
            if (buffer.Length + read > MaximumBytes) throw new ArgumentException("The file exceeds 1 MB.");
            buffer.Write(bytes, 0, read);
        }
        buffer.Position = 0;
        using var document = await JsonDocument.ParseAsync(buffer, new JsonDocumentOptions { MaxDepth = 32 }, cancellationToken);
        var root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object) throw new ArgumentException("Upload one item object, rather than a list or summary.");
        if (!root.TryGetProperty("components", out var components) || components.ValueKind != JsonValueKind.Array)
            throw new ArgumentException("The item must include a components array.");
        var mapped = components.EnumerateArray().Select((component, position) =>
        {
            if (component.ValueKind != JsonValueKind.Object || !component.TryGetProperty("value", out var value))
                throw new ArgumentException("Each component must be an object with a value.");
            return new ItemComponentDto { Type = Required(component, "type"), Label = Required(component, "label"),
                Value = value.Clone(), Html = Required(component, "html", allowEmpty: true), Position = position };
        }).ToList();
        var input = new ItemWriteDto { SourceId = Required(root, "id"), Slug = Required(root, "slug"), Name = Required(root, "name"),
            StorageFile = Required(root, "storageFile"), Components = mapped };
        ItemWriteValidation.Validate(input);
        if (input.StorageFile.IndexOfAny(['/', '\\']) >= 0 || !input.StorageFile.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ||
            input.Slug.IndexOfAny(['/', '\\']) >= 0)
            throw new ArgumentException("Slug and storageFile must not contain paths; storageFile must end in .json.");
        if (!mapped.Any(x => x.Type == "description" && x.Value.EnumerateArray().Any(v => !string.IsNullOrWhiteSpace(v.GetString()))))
            throw new ArgumentException("Include a nonempty description component.");
        if (!mapped.Any(x => x.Type == "recordKind" && x.Value.GetString() == "item") ||
            !mapped.Any(x => x.Type == "itemType" && !string.IsNullOrWhiteSpace(x.Value.GetString())))
            throw new ArgumentException("Include recordKind 'item' and a nonempty itemType component.");
        return input;
    }

    private static string Required(JsonElement element, string name, bool allowEmpty = false)
    {
        if (!element.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.String ||
            (!allowEmpty && string.IsNullOrWhiteSpace(value.GetString())))
            throw new ArgumentException($"'{name}' must be {(allowEmpty ? "a string" : "a nonempty string")}.");
        return value.GetString()!;
    }
}
