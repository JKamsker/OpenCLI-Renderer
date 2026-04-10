namespace InSpectra.Gen.Runtime.Rendering;

public sealed record MarkdownRenderOptions(
    int HybridSplitDepth,
    string? Title = null,
    string? CommandPrefix = null);
