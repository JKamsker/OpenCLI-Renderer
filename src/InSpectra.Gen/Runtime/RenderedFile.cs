namespace InSpectra.Gen.Runtime;

public sealed record RenderedFile(
    string RelativePath,
    string FullPath,
    string? Content);
