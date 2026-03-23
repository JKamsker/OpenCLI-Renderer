using System.Net;
using System.Text.Json;
using OpenCli.Renderer.Models;

namespace OpenCli.Renderer.Services;

public sealed class HtmlContentFormatter
{
    public string Encode(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);

    public string EncodeOrFallback(string? value, string fallback)
    {
        return Encode(string.IsNullOrWhiteSpace(value) ? fallback : value);
    }

    public string RenderParagraphBlock(string? value, string fallback = "")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.IsNullOrWhiteSpace(fallback) ? string.Empty : $"<p class=\"empty\">{Encode(fallback)}</p>";
        }

        return string.Join(string.Empty, value
            .Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(paragraph => $"<p>{Encode(paragraph).Replace("\r", string.Empty, StringComparison.Ordinal).Replace("\n", "<br />", StringComparison.Ordinal)}</p>"));
    }

    public string CreateDefinition(string label, string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : $"<div><dt>{Encode(label)}</dt><dd>{Encode(value)}</dd></div>";
    }

    public string CreateLinkDefinition(string label, string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : $"<div><dt>{Encode(label)}</dt><dd><a href=\"{Encode(value)}\">{Encode(value)}</a></dd></div>";
    }

    public string FormatMetadataValue(OpenCliMetadata metadata)
    {
        if (metadata.Value is null)
        {
            return "<code>null</code>";
        }

        return metadata.Value.GetValueKind() switch
        {
            JsonValueKind.String => $"<code>{Encode(metadata.Value.GetValue<string>())}</code>",
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => $"<code>{Encode(metadata.Value.ToJsonString())}</code>",
            _ => $"<pre><code>{Encode(metadata.Value.ToJsonString(new JsonSerializerOptions { WriteIndented = true }))}</code></pre>",
        };
    }
}
