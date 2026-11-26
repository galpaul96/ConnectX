using System.Text.Json;
using ConnectX.Domain.Services.DND.Catalogs.Spells;
using Microsoft.AspNetCore.Components;

namespace ConnectX.Web.Components.Pages.DND.Catalogs.Skills.Components;

public abstract class SpellComponentView : ComponentBase
{
    [Parameter] public SpellComponentDto? Component { get; set; }
    protected JsonElement Value => Component?.Value ?? default;
    protected string? Text(string? property = null) => SpellValue.Text(property is null ? Value : SpellValue.Property(Value, property));
    protected bool? Flag(string property) => SpellValue.Flag(SpellValue.Property(Value, property));
    protected IReadOnlyList<string> Strings => SpellValue.Strings(Value);
}

// Scalar readers only. Objects are rendered by their dedicated role component,
// never by dumping JSON or injecting source HTML into the page.
public static class SpellValue
{
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
