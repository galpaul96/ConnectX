using System.Text.Json;

namespace ConnectX.Domain.Services.DND.Catalogs.Items;

public static class ItemWriteValidation
{
    public static void Validate(ItemWriteDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        Required(input.SourceId, 128, nameof(input.SourceId));
        Required(input.Slug, 256, nameof(input.Slug));
        Required(input.Name, 256, nameof(input.Name));
        Required(input.StorageFile, 512, nameof(input.StorageFile));
        ArgumentNullException.ThrowIfNull(input.Components);
        if (input.Components.Count is < 1 or > 128)
            throw new ArgumentException("An item must have between 1 and 128 components.", nameof(input));
        var types = new HashSet<string>(StringComparer.Ordinal);
        foreach (var component in input.Components)
        {
            ArgumentNullException.ThrowIfNull(component);
            Required(component.Type, 128, nameof(component.Type));
            Required(component.Label, 256, nameof(component.Label));
            if (!types.Add(component.Type))
                throw new ArgumentException($"Duplicate item component type '{component.Type}'.", nameof(input));
            if (component.Value.ValueKind == JsonValueKind.Undefined)
                throw new ArgumentException("Component values must be valid JSON, including explicit null.", nameof(input));
            ArgumentNullException.ThrowIfNull(component.Html);
            if (component.Html.Length > 262144 || component.Value.GetRawText().Length > 262144)
                throw new ArgumentException("A component is too large.", nameof(input));
            var expected = component.Type switch
            {
                "cost" or "weight" or "sourceMetadata" or "completeness" or "poison" or "toolDetails" or "damage" or "currencyValue" or "armorClass" or "strengthRequirement" or "stealth" or "purchaseQuantity" => JsonValueKind.Object,
                "description" or "sourceFiles" or "usage" or "aliases" or "itemTags" or "tables" or "vehicleParts" or "mastery" or "properties" or "capacity" or "vehicleStats" or "notices" or "contents" => JsonValueKind.Array,
                "recordKind" or "itemType" or "source" or "url" or "imageUrl" => JsonValueKind.String,
                _ when component.Type.EndsWith("Variant", StringComparison.Ordinal) => JsonValueKind.Array,
                _ => component.Value.ValueKind
            };
            if (component.Value.ValueKind != expected)
                throw new ArgumentException($"Component '{component.Type}' has an invalid value type.", nameof(input));
            if (component.Type is "description" or "itemTags" or "sourceFiles" or "usage" or "aliases" or "notices" &&
                component.Value.EnumerateArray().Any(x => x.ValueKind != JsonValueKind.String))
                throw new ArgumentException($"Component '{component.Type}' must contain text entries.", nameof(input));
        }
    }

    private static void Required(string value, int maxLength, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
        if (value.Length > maxLength) throw new ArgumentException($"Maximum length is {maxLength}.", name);
    }
}
