using System.Text.Json;
using ConnectX.Domain.Services.DND.Catalogs.Spells;
using ConnectX.Web.Components.Pages.DND.Catalogs.Skills.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Xunit;

namespace ConnectX.Spells.Tests;

public class SpellUiTests
{
    [Fact]
    public async Task Every_catalog_record_renders_with_dedicated_components_and_excludes_provenance()
    {
        await using var provider = new ServiceCollection().AddLogging().AddSingleton<IJSRuntime, NoJs>().BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            foreach (var filename in Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "spell-catalog"), "*.json").Where(x => !Path.GetFileName(x).StartsWith('_')))
            {
                var source = SpellFlowTests.ReadSpell(filename);
                var spell = new SpellDto { Name = source.Name, Components = source.Components };
                var output = await renderer.RenderComponentAsync<SpellDetailModal>(ParameterView.FromDictionary(new Dictionary<string, object?> { [nameof(SpellDetailModal.Spell)] = spell }));
                var html = output.ToHtmlString();
                Assert.Contains("spell-modal-title", html);
                Assert.DoesNotContain("Source files", html);
                Assert.DoesNotContain("Handbook casting details", html);
                Assert.DoesNotContain("crawl-output-cleaned", html);
                Assert.DoesNotContain("System.Text.Json", html);
                Assert.DoesNotContain("<script", html, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("<dt>levelNumber</dt>", html);
            }
        });
    }

    [Fact]
    public async Task Optional_roles_are_hidden_strings_are_encoded_and_unsafe_urls_are_omitted()
    {
        await using var provider = new ServiceCollection().AddLogging().AddSingleton<IJSRuntime, NoJs>().BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var spell = new SpellDto { Name = "Encoded spell", Components =
            [
                Component("description", "[\"<script>alert(1)</script>\"]"),
                Component("spellTags", "[]"), Component("castingTimeBlurb", "\"\""),
                Component("damageEffect", "{\"text\":\"\",\"damageTypes\":[]}"),
                Component("url", "\"javascript:alert(1)\""),
                Component("imageUrl", "\"data:text/html,unsafe\""),
                Component("unknownFutureRole", "{\"value\":\"must not leak as raw JSON\"}")
            ] };
            var output = await renderer.RenderComponentAsync<SpellDetailModal>(ParameterView.FromDictionary(new Dictionary<string, object?> { [nameof(SpellDetailModal.Spell)] = spell }));
            var html = output.ToHtmlString();
            Assert.Contains("&lt;script&gt;", html);
            Assert.DoesNotContain("<script>", html);
            Assert.DoesNotContain("Spell tags", html);
            Assert.DoesNotContain("Casting note", html);
            Assert.DoesNotContain("Damage / effect", html);
            Assert.DoesNotContain("javascript:", html);
            Assert.DoesNotContain("<img", html);
            Assert.DoesNotContain("must not leak", html);
            Assert.DoesNotContain("<aside", html);
            Assert.DoesNotContain("<footer", html);
            Assert.DoesNotContain("<dl", html);
            Assert.Contains("aria-label=\"Spell information\"", html);
        });
    }

    [Fact]
    public async Task Summary_card_and_material_details_have_explicit_layouts()
    {
        await using var provider = new ServiceCollection().AddLogging().BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var card = await renderer.RenderComponentAsync<SpellCard>(ParameterView.FromDictionary(new Dictionary<string, object?> { [nameof(SpellCard.Entry)] = new SpellPageEntry(Guid.NewGuid(), new("Fireball", "https://example.test/evocation.png", "3rd", "Evocation")) }));
            var html = card.ToHtmlString();
            Assert.Contains("Fireball", html);
            Assert.Contains("Evocation", html);
            Assert.Contains("3rd", html);
            Assert.Contains("https://example.test/evocation.png", html);
            Assert.Contains("alt=\"\"", html);
            Assert.DoesNotContain("spell-level-badge", html);
            var component = Component("components", "{\"text\":\"V, M\",\"material\":{\"required\":true,\"description\":\"a gem\",\"consumed\":false,\"costGp\":0,\"costIsMinimum\":true}}");
            var material = await renderer.RenderComponentAsync<SpellCastingComponentsComponent>(ParameterView.FromDictionary(new Dictionary<string, object?> { [nameof(SpellComponentView.Component)] = component }));
            html = material.ToHtmlString();
            Assert.Contains("Material: a gem", html);
            Assert.Contains("Consumed: No", html);
            Assert.Contains("Cost: 0 GP minimum", html);
        });
    }

    private static SpellComponentDto Component(string type, string json) => new() { Type = type, Label = type, Value = JsonDocument.Parse(json).RootElement.Clone(), Html = "<script>source markup must never run</script>" };
    private sealed class NoJs : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => throw new InvalidOperationException("Static rendering must not invoke JS");
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new InvalidOperationException("Static rendering must not invoke JS");
    }
}
