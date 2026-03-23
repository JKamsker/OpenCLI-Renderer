using OpenCli.Renderer.Runtime;
using OpenCli.Renderer.Services;
using OpenCli.Renderer.Tests.TestSupport;

namespace OpenCli.Renderer.Tests;

public class DocumentRenderServiceTests
{
    private readonly DocumentRenderService _service = new(
        new OpenCliDocumentLoader(new OpenCliSchemaProvider()),
        new OpenCliXmlEnricher(),
        new OpenCliNormalizer(),
        new ExecutableResolver(),
        new ProcessRunner());
    private readonly MarkdownRenderer _markdownRenderer = RendererFactory.CreateMarkdownRenderer();
    private readonly HtmlRenderer _htmlRenderer = RendererFactory.CreateHtmlRenderer();

    [Fact]
    public async Task Dry_run_does_not_write_single_output_file()
    {
        using var temp = new TempDirectory();
        var outputFile = System.IO.Path.Combine(temp.Path, "docs.md");
        var request = new FileRenderRequest(
            FixturePaths.OpenCliJson,
            null,
            new RenderExecutionOptions(
                MarkdownLayout.Single,
                ResolvedOutputMode.Human,
                DryRun: true,
                Quiet: false,
                Verbose: false,
                NoColor: false,
                IncludeHidden: false,
                IncludeMetadata: false,
                Overwrite: false,
                OutputFile: outputFile,
                OutputDirectory: null));

        var result = await _service.RenderFromFileAsync(request, _markdownRenderer, CancellationToken.None);

        Assert.False(File.Exists(outputFile));
        Assert.True(result.IsDryRun);
        Assert.Single(result.Files);
        Assert.Equal(DocumentFormat.Markdown, result.Format);
    }

    [Fact]
    public async Task Tree_render_refuses_non_empty_directory_without_overwrite()
    {
        using var temp = new TempDirectory();
        File.WriteAllText(System.IO.Path.Combine(temp.Path, "keep.txt"), "existing");

        var request = new FileRenderRequest(
            FixturePaths.OpenCliJson,
            null,
            new RenderExecutionOptions(
                MarkdownLayout.Tree,
                ResolvedOutputMode.Human,
                DryRun: false,
                Quiet: false,
                Verbose: false,
                NoColor: false,
                IncludeHidden: false,
                IncludeMetadata: false,
                Overwrite: false,
                OutputFile: null,
                OutputDirectory: temp.Path));

        var exception = await Assert.ThrowsAsync<CliUsageException>(() =>
            _service.RenderFromFileAsync(request, _markdownRenderer, CancellationToken.None));

        Assert.Contains("not empty", exception.Message);
    }

    [Fact]
    public async Task Dry_run_tracks_html_output_format()
    {
        using var temp = new TempDirectory();
        var outputFile = System.IO.Path.Combine(temp.Path, "docs.html");
        var request = new FileRenderRequest(
            FixturePaths.OpenCliJson,
            null,
            new RenderExecutionOptions(
                MarkdownLayout.Single,
                ResolvedOutputMode.Human,
                DryRun: true,
                Quiet: false,
                Verbose: false,
                NoColor: false,
                IncludeHidden: false,
                IncludeMetadata: false,
                Overwrite: false,
                OutputFile: outputFile,
                OutputDirectory: null));

        var result = await _service.RenderFromFileAsync(request, _htmlRenderer, CancellationToken.None);

        Assert.Equal(DocumentFormat.Html, result.Format);
        Assert.Single(result.Files);
        Assert.Equal(outputFile, result.Files[0].FullPath);
    }
}
