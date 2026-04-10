namespace InSpectra.Gen.Runtime.Rendering;

public sealed record RenderedFile(
    string RelativePath,
    string FullPath,
    string? Content);
