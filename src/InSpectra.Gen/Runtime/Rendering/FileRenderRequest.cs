namespace InSpectra.Gen.Runtime.Rendering;

public sealed record FileRenderRequest(
    string OpenCliJsonPath,
    string? XmlDocPath,
    RenderExecutionOptions Options,
    MarkdownRenderOptions? MarkdownOptions = null);
