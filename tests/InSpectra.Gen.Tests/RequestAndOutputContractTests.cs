using System.Text.Json.Nodes;
using InSpectra.Gen.Commands.Generate;
using InSpectra.Gen.Commands.Render;
using InSpectra.Gen.Runtime;

namespace InSpectra.Gen.Tests;

public class RequestAndOutputContractTests
{
    [Fact]
    public void Json_single_output_requires_out_file()
    {
        var settings = new TestMarkdownSettings
        {
            Json = true,
        };

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateMarkdownOptions(settings, "single", null, null, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("requires `--out`", exception.Message);
    }

    [Fact]
    public void Hybrid_layout_requires_out_dir()
    {
        var settings = new TestMarkdownSettings();

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateMarkdownOptions(settings, "hybrid", null, null, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("`--layout hybrid` requires `--out-dir`", exception.Message);
    }

    [Fact]
    public void Hybrid_layout_rejects_out_file()
    {
        var settings = new TestMarkdownSettings();

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateMarkdownOptions(settings, "hybrid", "docs.md", null, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("`--out` is only valid with `--layout single`", exception.Message);
    }

    [Fact]
    public void Split_depth_requires_hybrid_layout()
    {
        var settings = new TestMarkdownSettings();

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateMarkdownOptions(settings, "single", "docs.md", null, timeoutSeconds: null, hasTimeoutSupport: false, splitDepth: 2));

        Assert.Contains("`--split-depth` is only valid with `--layout hybrid`", exception.Message);
    }

    [Fact]
    public void Split_depth_must_be_positive()
    {
        var settings = new TestMarkdownSettings();

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateMarkdownOptions(settings, "hybrid", null, "out", timeoutSeconds: null, hasTimeoutSupport: false, splitDepth: 0));

        Assert.Contains("`--split-depth` must be at least 1", exception.Message);
    }

    [Fact]
    public void Hybrid_layout_with_valid_split_depth_creates_markdown_render_options()
    {
        var settings = new TestMarkdownSettings();

        var options = RenderRequestFactory.CreateMarkdownOptions(settings, "hybrid", null, "out", timeoutSeconds: null, hasTimeoutSupport: false, splitDepth: 2);
        var markdownOptions = RenderRequestFactory.CreateMarkdownRenderOptions(settings, options.Layout, 2);

        Assert.Equal(RenderLayout.Hybrid, options.Layout);
        Assert.NotNull(markdownOptions);
        Assert.Equal(2, markdownOptions!.HybridSplitDepth);
    }

    [Fact]
    public void Hybrid_layout_default_split_depth_is_one()
    {
        var settings = new TestMarkdownSettings();

        var options = RenderRequestFactory.CreateMarkdownOptions(settings, "hybrid", null, "out", timeoutSeconds: null, hasTimeoutSupport: false);
        var markdownOptions = RenderRequestFactory.CreateMarkdownRenderOptions(settings, options.Layout, splitDepth: null);

        Assert.NotNull(markdownOptions);
        Assert.Equal(1, markdownOptions!.HybridSplitDepth);
    }

    [Fact]
    public void Non_hybrid_layout_produces_no_markdown_render_options()
    {
        var settings = new TestMarkdownSettings();

        Assert.Null(RenderRequestFactory.CreateMarkdownRenderOptions(settings, RenderLayout.Single, splitDepth: null));
        Assert.Null(RenderRequestFactory.CreateMarkdownRenderOptions(settings, RenderLayout.Tree, splitDepth: null));
    }

    [Fact]
    public void Html_output_requires_out_dir()
    {
        var settings = new TestHtmlSettings();

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateHtmlOptions(settings, null, null, null, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("requires `--out-dir`", exception.Message);
    }

    [Fact]
    public void Html_output_rejects_out_file()
    {
        var settings = new TestHtmlSettings();

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateHtmlOptions(settings, null, "docs.html", null, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("`--out` is not supported", exception.Message);
    }

    [Fact]
    public void Html_output_rejects_layout()
    {
        var settings = new TestHtmlSettings
        {
            OutputDirectory = "docs",
        };

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateHtmlOptions(settings, "tree", null, settings.OutputDirectory, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("`--layout` is not supported", exception.Message);
    }

    [Fact]
    public void Html_command_settings_do_not_expose_markdown_output_flags()
    {
        var fileProperties = typeof(FileHtmlSettings).GetProperties().Select(property => property.Name).ToArray();

        Assert.DoesNotContain(nameof(MarkdownCommandSettingsBase.Layout), fileProperties);
        Assert.DoesNotContain(nameof(MarkdownCommandSettingsBase.OutputFile), fileProperties);
        Assert.Contains(nameof(HtmlCommandSettingsBase.OutputDirectory), fileProperties);
    }

    [Theory]
    [InlineData("native", OpenCliMode.Native)]
    [InlineData("auto", OpenCliMode.Auto)]
    [InlineData("help", OpenCliMode.Help)]
    [InlineData("clifx", OpenCliMode.CliFx)]
    [InlineData("static", OpenCliMode.Static)]
    [InlineData("hook", OpenCliMode.Hook)]
    public void OpenCli_mode_parsing_supports_all_public_values(string value, OpenCliMode expected)
    {
        var mode = RenderRequestFactory.ResolveOpenCliMode(value, OpenCliMode.Native);
        Assert.Equal(expected, mode);
    }

    [Fact]
    public void Generate_settings_expose_xml_enrichment_flags()
    {
        var execProperties = typeof(ExecGenerateSettings).GetProperties().Select(property => property.Name).ToArray();
        var dotnetProperties = typeof(DotnetGenerateSettings).GetProperties().Select(property => property.Name).ToArray();
        var packageProperties = typeof(PackageGenerateSettings).GetProperties().Select(property => property.Name).ToArray();

        Assert.Contains(nameof(GenerateCommandSettingsBase.WithXmlDoc), execProperties);
        Assert.Contains(nameof(GenerateCommandSettingsBase.XmlDocArguments), execProperties);
        Assert.Contains(nameof(GenerateCommandSettingsBase.WithXmlDoc), dotnetProperties);
        Assert.Contains(nameof(GenerateCommandSettingsBase.XmlDocArguments), dotnetProperties);
        Assert.Contains(nameof(GenerateCommandSettingsBase.WithXmlDoc), packageProperties);
        Assert.Contains(nameof(GenerateCommandSettingsBase.XmlDocArguments), packageProperties);
    }

    [Fact]
    public void File_render_settings_do_not_expose_acquisition_flags()
    {
        var markdownProperties = typeof(FileRenderSettings).GetProperties().Select(property => property.Name).ToArray();
        var htmlProperties = typeof(FileHtmlSettings).GetProperties().Select(property => property.Name).ToArray();

        Assert.DoesNotContain(nameof(GenerateCommandSettingsBase.OpenCliMode), markdownProperties);
        Assert.DoesNotContain(nameof(GenerateCommandSettingsBase.CommandName), markdownProperties);
        Assert.DoesNotContain(nameof(GenerateCommandSettingsBase.CliFramework), markdownProperties);
        Assert.DoesNotContain(nameof(GenerateCommandSettingsBase.OpenCliMode), htmlProperties);
        Assert.DoesNotContain(nameof(GenerateCommandSettingsBase.CommandName), htmlProperties);
        Assert.DoesNotContain(nameof(GenerateCommandSettingsBase.CliFramework), htmlProperties);
    }

    [Fact]
    public async Task Json_output_writer_emits_versioned_success_envelope()
    {
        var original = Console.Out;
        await using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            var result = new RenderExecutionResult
            {
                Format = DocumentFormat.Html,
                Layout = RenderLayout.App,
                Source = new RenderSourceInfo("file", "sample.json", null, null),
                Acquisition = new OpenCliAcquisitionMetadata(
                    "help",
                    "tool",
                    "CliFx",
                    [new OpenCliAcquisitionAttempt("native", null, "failed", "native unavailable"), new OpenCliAcquisitionAttempt("help", null, "success")],
                    "sample-opencli.json",
                    "sample-crawl.json"),
                Stats = new RenderStats(1, 2, 3, 1),
                Warnings = [],
                IsDryRun = false,
                Files = [new RenderedFile("index.html", "C:\\temp\\index.html", null)],
                Summary = null,
            };

            var exitCode = await CommandOutputHandler.ExecuteAsync(
                ResolvedOutputMode.Json,
                verbose: false,
                () => Task.FromResult(result));

            var json = JsonNode.Parse(writer.ToString());

            Assert.Equal(0, exitCode);
            Assert.NotNull(json);
            Assert.True(json!["ok"]!.GetValue<bool>());
            Assert.Equal("html", json["data"]!["format"]!.GetValue<string>());
            Assert.Equal("app", json["data"]!["layout"]!.GetValue<string>());
            Assert.Equal("help", json["data"]!["acquisition"]!["selectedMode"]!.GetValue<string>());
            Assert.Equal("sample-crawl.json", json["data"]!["acquisition"]!["artifacts"]!["crawl"]!.GetValue<string>());
            var files = json["data"]!["output"]!["files"]!.AsArray();
            var file = Assert.Single(files);
            Assert.Equal("index.html", file!["relativePath"]!.GetValue<string>());
            Assert.Equal(1, json["meta"]!["schemaVersion"]!.GetValue<int>());
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    [Fact]
    public async Task Generate_json_output_writer_emits_acquisition_metadata()
    {
        var original = Console.Out;
        await using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            var result = new GenerateExecutionResult(
                new RenderSourceInfo("dotnet", "src/InSpectra.Gen", "C:\\temp\\inspectra.exe", null),
                new OpenCliAcquisitionMetadata(
                    "native",
                    "inspectra",
                    "Spectre.Console.Cli",
                    [new OpenCliAcquisitionAttempt("native", "Spectre.Console.Cli", "success")],
                    "generated-opencli.json",
                    null),
                [],
                "{}",
                "generated-opencli.json");

            var exitCode = await GenerateOutputHandler.ExecuteAsync(
                ResolvedOutputMode.Json,
                verbose: false,
                () => Task.FromResult(result));

            var json = JsonNode.Parse(writer.ToString());

            Assert.Equal(0, exitCode);
            Assert.NotNull(json);
            Assert.True(json!["ok"]!.GetValue<bool>());
            Assert.Equal("dotnet", json["data"]!["source"]!["kind"]!.GetValue<string>());
            Assert.Equal("native", json["data"]!["acquisition"]!["selectedMode"]!.GetValue<string>());
            Assert.Equal("generated-opencli.json", json["data"]!["acquisition"]!["artifacts"]!["openCli"]!.GetValue<string>());
            var attempts = json["data"]!["acquisition"]!["attempts"]!.AsArray();
            var attempt = Assert.Single(attempts);
            Assert.Equal("success", attempt!["outcome"]!.GetValue<string>());
            Assert.Equal(1, json["meta"]!["schemaVersion"]!.GetValue<int>());
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    private sealed class TestMarkdownSettings : MarkdownCommandSettingsBase
    {
    }

    private sealed class TestHtmlSettings : HtmlCommandSettingsBase
    {
    }
}
