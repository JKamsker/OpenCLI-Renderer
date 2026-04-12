using InSpectra.Gen.OpenCli.Metadata;
using InSpectra.Gen.Rendering.Contracts;
using InSpectra.Gen.UseCases.Generate.Requests;

namespace InSpectra.Gen.UseCases.Generate;

public sealed record GenerateExecutionResult(
    RenderSourceInfo Source,
    OpenCliAcquisitionMetadata Acquisition,
    IReadOnlyList<string> Warnings,
    string OpenCliJson,
    string? OutputFile);
