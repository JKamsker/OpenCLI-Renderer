namespace InSpectra.Gen.Runtime;

public sealed record GenerateExecutionResult(
    RenderSourceInfo Source,
    OpenCliAcquisitionMetadata Acquisition,
    IReadOnlyList<string> Warnings,
    string OpenCliJson,
    string? OutputFile);
