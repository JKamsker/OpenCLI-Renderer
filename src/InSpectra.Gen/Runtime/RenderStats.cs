namespace InSpectra.Gen.Runtime;

public sealed record RenderStats(
    int CommandCount,
    int OptionCount,
    int ArgumentCount,
    int FileCount);
