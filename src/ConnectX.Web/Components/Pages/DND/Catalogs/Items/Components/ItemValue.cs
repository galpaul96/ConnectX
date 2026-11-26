using System.Text.Json;

namespace ConnectX.Web.Components.Pages.DND.Catalogs.Items.Components;

public static class ItemValue
{
    public static IReadOnlyList<JsonElement> Array(JsonElement value) => value.ValueKind == JsonValueKind.Array ? value.EnumerateArray().ToList() : [];
    public static bool HasContent(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.Null or JsonValueKind.Undefined => false,
        JsonValueKind.String => !string.IsNullOrWhiteSpace(value.GetString()),
        JsonValueKind.Array => value.EnumerateArray().Any(HasContent),
        JsonValueKind.Object => value.EnumerateObject().Any(x => HasContent(x.Value)),
        _ => true
    };
    public static string Label(string value)
    {
        var label = System.Text.RegularExpressions.Regex.Replace(value, "([a-z])([A-Z])", "$1 $2");
        return label.Length > 0 ? char.ToUpperInvariant(label[0]) + label[1..] : value;
    }
    public static int? TryGetInt32Safe(this JsonElement value) => value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var number) ? number : null;
    public static JsonElement Property(JsonElement value, string property) =>
        value.ValueKind == JsonValueKind.Object && value.TryGetProperty(property, out var result) ? result : default;
    public static string? Text(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.String => value.GetString(),
        JsonValueKind.Number => value.GetRawText(),
        _ => null
    };
    public static bool? Flag(JsonElement value) => value.ValueKind switch
    {
        JsonValueKind.True => true, JsonValueKind.False => false, _ => null
    };
    public static IReadOnlyList<string> Strings(JsonElement value) => value.ValueKind == JsonValueKind.Array
        ? value.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.String)
            .Select(x => x.GetString()!).Where(x => !string.IsNullOrWhiteSpace(x)).ToList() : [];
    public static string? SafeUrl(string? value) => Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp) ? uri.AbsoluteUri : null;
}
