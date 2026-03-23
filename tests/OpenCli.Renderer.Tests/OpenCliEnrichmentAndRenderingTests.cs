using OpenCli.Renderer.Services;
using OpenCli.Renderer.Tests.TestSupport;

namespace OpenCli.Renderer.Tests;

public class OpenCliEnrichmentAndRenderingTests
{
    private readonly OpenCliDocumentLoader _loader = new(new OpenCliSchemaProvider());
    private readonly OpenCliXmlEnricher _enricher = new();
    private readonly OpenCliNormalizer _normalizer = new();
    private readonly MarkdownRenderer _renderer = new();

    [Fact]
    public async Task Xml_enrichment_restores_missing_command_descriptions()
    {
        var document = await _loader.LoadFromFileAsync(FixturePaths.OpenCliJson, CancellationToken.None);
        var authLogin = document.Commands
            .Single(command => command.Name == "auth")
            .Commands
            .Single(command => command.Name == "login");

        authLogin.Description = null;

        var enrichment = await _enricher.EnrichFromFileAsync(document, FixturePaths.XmlDoc, CancellationToken.None);

        Assert.Equal("Store encrypted auth material for a profile.", authLogin.Description);
        Assert.True(enrichment.MatchedCommandCount > 0);
    }

    [Fact]
    public async Task Single_markdown_omits_metadata_by_default_and_can_include_it()
    {
        var document = await _loader.LoadFromFileAsync(FixturePaths.OpenCliJson, CancellationToken.None);
        var normalized = _normalizer.Normalize(document, includeHidden: false);

        var markdownWithoutMetadata = _renderer.RenderSingle(normalized, includeMetadata: false);
        var markdownWithMetadata = _renderer.RenderSingle(normalized, includeMetadata: true);

        Assert.Contains("# jdr", markdownWithoutMetadata);
        Assert.Contains("## Commands", markdownWithoutMetadata);
        Assert.Contains("`auth login`", markdownWithoutMetadata);
        Assert.DoesNotContain("Metadata Appendix", markdownWithoutMetadata);
        Assert.Contains("Metadata Appendix", markdownWithMetadata);
        Assert.Contains("ClrType", markdownWithMetadata);
    }

    [Fact]
    public async Task Tree_markdown_creates_expected_command_pages()
    {
        var document = await _loader.LoadFromFileAsync(FixturePaths.OpenCliJson, CancellationToken.None);
        var normalized = _normalizer.Normalize(document, includeHidden: false);

        var files = _renderer.RenderTree(normalized, includeMetadata: false);

        Assert.Contains(files, file => file.RelativePath == "index.md");
        Assert.Contains(files, file => file.RelativePath == "auth/index.md");
        Assert.Contains(files, file => file.RelativePath == "auth/login.md");
        Assert.Contains("Store encrypted auth material for a profile.", files.Single(file => file.RelativePath == "auth/login.md").Content);
    }
}
