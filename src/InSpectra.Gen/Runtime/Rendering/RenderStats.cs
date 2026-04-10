namespace InSpectra.Gen.Runtime.Rendering;

public sealed record RenderStats(
    int CommandCount,
    int OptionCount,
    int ArgumentCount,
    int FileCount);
