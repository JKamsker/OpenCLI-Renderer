using OpenCli.Renderer.Runtime;
using OpenCli.Renderer.Services;
using OpenCli.Renderer.Tests.TestSupport;

namespace OpenCli.Renderer.Tests;

public class MarkdownRenderServiceTests
{
    private readonly MarkdownRenderService _service = new(
        new OpenCliDocumentLoader(new OpenCliSchemaProvider()),
        new OpenCliXmlEnricher(),
        new OpenCliNormalizer(),
        new MarkdownRenderer(),
        new ExecutableResolver(),
        new ProcessRunner());

    [Fact]
    public async Task Dry_run_does_not_write_single_output_file()
    {
        using var temp = new TempDirectory();
        var outputFile = System.IO.Path.Combine(temp.Path, "docs.md");
        var request = new FileMarkdownRenderRequest(
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

        var result = await _service.RenderFromFileAsync(request, CancellationToken.None);

        Assert.False(File.Exists(outputFile));
        Assert.True(result.IsDryRun);
        Assert.Single(result.Files);
    }

    [Fact]
    public async Task Tree_render_refuses_non_empty_directory_without_overwrite()
    {
        using var temp = new TempDirectory();
        File.WriteAllText(System.IO.Path.Combine(temp.Path, "keep.txt"), "existing");

        var request = new FileMarkdownRenderRequest(
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
            _service.RenderFromFileAsync(request, CancellationToken.None));

        Assert.Contains("not empty", exception.Message);
    }
}
