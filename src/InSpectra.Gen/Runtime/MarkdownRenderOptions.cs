namespace InSpectra.Gen.Runtime;

public sealed record MarkdownRenderOptions(
    int HybridSplitDepth,
    string? Title = null,
    string? CommandPrefix = null);
