namespace InSpectra.Gen.Runtime;

public sealed record FileRenderRequest(
    string OpenCliJsonPath,
    string? XmlDocPath,
    RenderExecutionOptions Options,
    MarkdownRenderOptions? MarkdownOptions = null);
