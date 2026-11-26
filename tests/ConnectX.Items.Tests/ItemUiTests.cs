using System.Text.Json;
using ConnectX.Domain.Services.DND.Catalogs.Items;
using ConnectX.Web.Components.Pages.DND.Catalogs.Items.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Xunit;

namespace ConnectX.Items.Tests;

public class ItemUiTests
{
    [Fact]
    public async Task Entire_corpus_renders_without_html_injection_or_internal_provenance()
    {
        await using var provider = new ServiceCollection().AddLogging().AddSingleton<IJSRuntime, NoJs>().BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            foreach (var filename in Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "items"), "*.json"))
            {
                var source = ItemFlowTests.ReadItem(filename);
                var output = await renderer.RenderComponentAsync<ItemDetailModal>(ParameterView.FromDictionary(new Dictionary<string, object?>
                {
                    [nameof(ItemDetailModal.Item)] = new ItemDto { Name = source.Name, Components = source.Components }
                }));
                var html = output.ToHtmlString();
                if (Environment.GetEnvironmentVariable("CONNECTX_ITEM_PREVIEW") is { Length: > 0 } preview &&
                    Path.GetFileName(filename) is "longsword.json" or "sailing ship.json" or "sailingship.json" or "arcanefocus.json" or "backpack.json")
                {
                    Directory.CreateDirectory(preview);
                    await File.WriteAllTextAsync(Path.Combine(preview, Path.GetFileNameWithoutExtension(filename) + ".html"),
                        "<!doctype html><html data-bs-theme=\"dark\"><head><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><link rel=\"stylesheet\" href=\"bootstrap.css\"><link rel=\"stylesheet\" href=\"app.css\"><link rel=\"stylesheet\" href=\"scoped.css\"></head><body>" + html +
                        "<script type=\"module\">import {open} from './itemDialog.js';open(document.querySelector('dialog'),{invokeMethodAsync(){}});</script></body></html>");
                }
                Assert.Contains("item-modal-title", html);
                Assert.DoesNotContain("crawl-output-cleaned", html);
                Assert.DoesNotContain("variantGroup", html);
                Assert.DoesNotContain("System.Text.Json", html);
                Assert.DoesNotContain("<script", html, StringComparison.OrdinalIgnoreCase);
                if (source.Components.Any(x => x.Type == "tables")) Assert.Contains("<table>", html);
            }
        });
    }

    [Fact]
    public async Task Details_encode_source_text_hide_unsafe_links_and_render_item_stats_and_variants()
    {
        await using var provider = new ServiceCollection().AddLogging().AddSingleton<IJSRuntime, NoJs>().BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, provider.GetRequiredService<ILoggerFactory>());
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var source = ItemFlowTests.ReadItem(Path.Combine(AppContext.BaseDirectory, "items", "longsword.json"));
            var components = source.Components.Where(x => x.Type is not ("url" or "description")).ToList();
            components.Add(new() { Type = "description", Label = "Description", Value = JsonSerializer.SerializeToElement(new[] { "<script>alert(1)</script>" }), Html = "<script>alert(1)</script>" });
            components.Add(new() { Type = "url", Label = "URL", Value = JsonSerializer.SerializeToElement("javascript:alert(1)") });
            var output = await renderer.RenderComponentAsync<ItemDetailModal>(ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                [nameof(ItemDetailModal.Item)] = new ItemDto { Name = source.Name, Components = components }
            }));
            var html = output.ToHtmlString();
            Assert.Contains("&lt;script&gt;", html);
            Assert.DoesNotContain("<script>", html);
            Assert.DoesNotContain("javascript:", html);
            Assert.Contains("At a glance", html);
            Assert.Contains("1d8", html);
            Assert.Contains("Source variants", html);
            Assert.Contains("<details", html);
            Assert.Contains("<table>", html);
            var card = await renderer.RenderComponentAsync<ItemCard>(ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                [nameof(ItemCard.Entry)] = new ItemPageEntry(Guid.NewGuid(), new("Longsword", null, "Martial Melee Weapon", "15 GP"))
            }));
            Assert.Contains("Martial Melee Weapon", card.ToHtmlString());
            Assert.Contains("15 GP", card.ToHtmlString());
        });
    }

    private sealed class NoJs : IJSRuntime
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => ValueTask.FromResult(default(TValue)!);
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => ValueTask.FromResult(default(TValue)!);
    }
}
