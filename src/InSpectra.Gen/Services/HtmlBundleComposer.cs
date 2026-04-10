using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using InSpectra.Gen.Models;
using InSpectra.Gen.Runtime.Json;
using InSpectra.Gen.Runtime.Rendering;

namespace InSpectra.Gen.Services;

internal static class HtmlBundleComposer
{
    public static string BuildInlineBootstrap(
        AcquiredRenderDocument prepared,
        bool includeHidden,
        bool includeMetadata,
        HtmlFeatureFlags features,
        string? label,
        string? title,
        string? commandPrefix,
        HtmlThemeOptions? themeOptions,
        int compressLevel)
    {
        var json = BuildRawBootstrapJson(prepared, includeHidden, includeMetadata, features, label, title, commandPrefix, themeOptions);
        return compressLevel >= 1
            ? GzipBase64(json)
            : json.Replace("</script", "<\\/script", StringComparison.OrdinalIgnoreCase);
    }

    public static string BuildSelfExtractingHtml(
        AcquiredRenderDocument prepared,
        RenderExecutionOptions options,
        HtmlFeatureFlags features,
        string? label,
        string? title,
        string? commandPrefix,
        HtmlThemeOptions? themeOptions,
        string bundleRoot,
        string outputDirectory)
    {
        var referencedAssets = CollectReferencedAssets(bundleRoot);
        var assetsDir = Path.Combine(outputDirectory, "assets");
        var cssFiles = Directory.GetFiles(assetsDir, "*.css")
            .Where(file => referencedAssets.Contains($"assets/{Path.GetFileName(file)}"))
            .ToList();
        var jsFiles = Directory.GetFiles(assetsDir, "*.js")
            .Where(file => referencedAssets.Contains($"assets/{Path.GetFileName(file)}"))
            .ToList();

        var css = string.Join("\n", cssFiles.Select(File.ReadAllText));
        var entryFile = jsFiles.FirstOrDefault(static file => !Path.GetFileName(file).StartsWith("loadSource", StringComparison.Ordinal));
        var js = entryFile is not null
            ? BundleModulesAsIife(File.ReadAllText(entryFile), assetsDir)
            : string.Join("\n", jsFiles.Select(File.ReadAllText));
        var bootstrap = BuildRawBootstrapJson(prepared, options.IncludeHidden, options.IncludeMetadata, features, label, title, commandPrefix, themeOptions);
        var pack = JsonSerializer.Serialize(new { c = css, j = js, b = bootstrap }, JsonOutput.CompactSerializerOptions);
        var compressedBlob = GzipBase64(pack);

        const string head =
            """<!doctype html><html lang="en"><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1.0"><title>InSpectraUI</title><link rel="preconnect" href="https://fonts.googleapis.com"><link rel="preconnect" href="https://fonts.gstatic.com" crossorigin><link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500;700&display=swap" rel="stylesheet"></head><body><div id="root"></div>""";
        const string themeScript =
            """<script>(function(){var s=localStorage.getItem("inspectra-theme");if(s==="dark"||s==="light")document.documentElement.dataset.theme=s;else if(matchMedia("(prefers-color-scheme:dark)").matches)document.documentElement.dataset.theme="dark";var c=localStorage.getItem("inspectra-color-theme");if(c)document.documentElement.dataset.colorTheme=c})()</script>""";
        const string decompressor =
            """<script>var _u=Uint8Array.from(atob(document.getElementById("_z").textContent),function(c){return c.charCodeAt(0)});new Response(new Blob([_u]).stream().pipeThrough(new DecompressionStream("gzip"))).text().then(function(t){var p=JSON.parse(t);var d=document;var s=d.createElement("style");s.textContent=p.c;d.head.appendChild(s);var b=d.createElement("script");b.id="inspectra-bootstrap";b.type="application/json";b.textContent=p.b;d.body.appendChild(b);var j=d.createElement("script");j.textContent=p.j;d.body.appendChild(j)})</script>""";

        return head + themeScript
            + """<script id="_z" type="text/plain">""" + compressedBlob + "</script>"
            + decompressor + "</body></html>";
    }

    public static string InlineAssets(string html, string outputDirectory)
    {
        html = Regex.Replace(html, @"<link\s[^>]*href=""\./(assets/[^""]+\.css)""[^>]*/?>", match =>
        {
            var cssPath = Path.Combine(outputDirectory, match.Groups[1].Value);
            return !File.Exists(cssPath)
                ? match.Value
                : $"<style>{File.ReadAllText(cssPath)}</style>";
        });

        html = Regex.Replace(html, @"<link\s[^>]*rel=""modulepreload""[^>]*/?>[\r\n]*", string.Empty);
        html = Regex.Replace(
            html,
            @"<script\s+id=""inspectra-bootstrap""\s+type=""application/json"">[^<]+</script>",
            match =>
            {
                var tagContent = Regex.Match(match.Value, @">([^<]+)<").Groups[1].Value;
                return $"<script>window.__inspectraBootstrap=JSON.parse('{EscapeForJsStringLiteral(tagContent)}')</script>";
            });

        string? inlineScriptBlock = null;
        html = Regex.Replace(html, @"<script\s[^>]*src=""\./(assets/[^""]+\.js)""[^>]*></script>[\r\n]*", match =>
        {
            var entryPath = Path.Combine(outputDirectory, match.Groups[1].Value);
            if (!File.Exists(entryPath))
            {
                return match.Value;
            }

            var entryCode = File.ReadAllText(entryPath);
            var entryDirectory = Path.GetDirectoryName(entryPath)!;
            inlineScriptBlock = $"<script>{BundleModulesAsIife(entryCode, entryDirectory)}</script>";
            return string.Empty;
        });

        return inlineScriptBlock is null
            ? html
            : html.Replace("</body>", $"{inlineScriptBlock}\n</body>", StringComparison.Ordinal);
    }

    public static string MinifyHtml(string html)
    {
        html = Regex.Replace(html, @">\s+<", "> <");
        html = Regex.Replace(html, @"^\s+", string.Empty, RegexOptions.Multiline);
        html = Regex.Replace(html, @"\n{2,}", "\n");
        return html.Trim();
    }

    public static HashSet<string> CollectReferencedAssets(string bundleRoot)
    {
        var referenced = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "static.html" };
        var staticHtmlPath = Path.Combine(bundleRoot, "static.html");
        if (!File.Exists(staticHtmlPath))
        {
            return referenced;
        }

        var html = File.ReadAllText(staticHtmlPath);
        foreach (Match match in Regex.Matches(html, @"(?:src|href)=""\./([^""]+)"""))
        {
            referenced.Add(match.Groups[1].Value);
        }

        return referenced;
    }

    private static string BuildRawBootstrapJson(
        AcquiredRenderDocument prepared,
        bool includeHidden,
        bool includeMetadata,
        HtmlFeatureFlags features,
        string? label,
        string? title,
        string? commandPrefix,
        HtmlThemeOptions? themeOptions)
    {
        var payload = new InlineBootstrap
        {
            Mode = "inline",
            OpenCli = prepared.RawDocument,
            XmlDoc = prepared.XmlDocument,
            Options = new ViewerOptionsPayload
            {
                IncludeHidden = includeHidden,
                IncludeMetadata = includeMetadata,
                Label = label,
                Title = title,
                CommandPrefix = commandPrefix,
                Theme = themeOptions?.Theme,
                ColorTheme = themeOptions?.ColorTheme,
                CustomAccent = themeOptions?.CustomAccent,
                CustomAccentDark = themeOptions?.CustomAccentDark,
            },
            Features = new FeatureFlagsPayload
            {
                ShowHome = features.ShowHome,
                Composer = features.Composer,
                DarkTheme = features.DarkTheme,
                LightTheme = features.LightTheme,
                UrlLoading = features.UrlLoading,
                NugetBrowser = features.NugetBrowser,
                PackageUpload = features.PackageUpload,
                ColorThemePicker = features.ColorThemePicker,
            },
        };

        return JsonSerializer.Serialize(payload, JsonOutput.CompactSerializerOptions);
    }

    private static string EscapeForJsStringLiteral(string value)
        => value
            .Replace("\\", "\\\\")
            .Replace("'", "\\'")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("</script", "<\\/script", StringComparison.OrdinalIgnoreCase);

    private static string BundleModulesAsIife(string entryCode, string entryDirectory)
    {
        var importPattern = new Regex(@"import\{([^}]+)\}from""\./([\w.=-]+\.js)"";?");
        var importMatch = importPattern.Match(entryCode);
        if (!importMatch.Success)
        {
            return $"(function(){{{entryCode}}})();";
        }

        var chunkPath = Path.Combine(entryDirectory, importMatch.Groups[2].Value);
        if (!File.Exists(chunkPath))
        {
            return $"(function(){{{entryCode}}})();";
        }

        var chunkCode = File.ReadAllText(chunkPath);
        var exportPattern = new Regex(@"export\{([^}]+)\};?\s*$");
        var exportMatch = exportPattern.Match(chunkCode);
        if (!exportMatch.Success)
        {
            return $"(function(){{{chunkCode}\n{entryCode}}})();";
        }

        var exportMap = CreateExportMap(exportMatch.Groups[1].Value);
        var importBindings = ParseImportBindings(importMatch.Groups[1].Value);
        var cleanedChunk = exportPattern.Replace(chunkCode, string.Empty);
        var cleanedEntry = importPattern.Replace(entryCode, string.Empty, 1);
        var exportedMembers = string.Join(",", importBindings.Select(binding =>
            exportMap.TryGetValue(binding.ExportedName, out var chunkLocal)
                ? $"{binding.ExportedName}:{chunkLocal}"
                : string.Empty));
        var aliases = string.Join(string.Empty, importBindings.Select(binding => $"var {binding.LocalAlias}=__M.{binding.ExportedName};"));

        return $"(function(){{var __M=(function(){{{cleanedChunk}return{{{exportedMembers}}}}})();{aliases}{cleanedEntry}}})();";
    }

    private static Dictionary<string, string> CreateExportMap(string exportList)
    {
        var exportMap = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in exportList.Split(','))
        {
            var parts = pair.Trim().Split(" as ", 2, StringSplitOptions.TrimEntries);
            exportMap[parts[^1]] = parts[0];
        }

        return exportMap;
    }

    private static List<(string ExportedName, string LocalAlias)> ParseImportBindings(string bindingList)
    {
        var bindings = new List<(string ExportedName, string LocalAlias)>();
        foreach (var pair in bindingList.Split(','))
        {
            var parts = pair.Trim().Split(" as ", 2, StringSplitOptions.TrimEntries);
            bindings.Add(parts.Length == 2 ? (parts[0], parts[1]) : (parts[0], parts[0]));
        }

        return bindings;
    }

    private static string GzipBase64(string text)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.SmallestSize, leaveOpen: true))
        {
            gzip.Write(bytes);
        }

        return Convert.ToBase64String(output.ToArray());
    }

    private sealed class InlineBootstrap
    {
        public required string Mode { get; init; }

        public required OpenCliDocument OpenCli { get; init; }

        public string? XmlDoc { get; init; }

        public required ViewerOptionsPayload Options { get; init; }

        public required FeatureFlagsPayload Features { get; init; }
    }

    private sealed class ViewerOptionsPayload
    {
        public required bool IncludeHidden { get; init; }

        public required bool IncludeMetadata { get; init; }

        public string? Label { get; init; }

        public string? Title { get; init; }

        public string? CommandPrefix { get; init; }

        public string? Theme { get; init; }

        public string? ColorTheme { get; init; }

        public string? CustomAccent { get; init; }

        public string? CustomAccentDark { get; init; }
    }

    private sealed class FeatureFlagsPayload
    {
        public required bool ShowHome { get; init; }

        public required bool Composer { get; init; }

        public required bool DarkTheme { get; init; }

        public required bool LightTheme { get; init; }

        public required bool UrlLoading { get; init; }

        public required bool NugetBrowser { get; init; }

        public required bool PackageUpload { get; init; }

        public required bool ColorThemePicker { get; init; }
    }
}
