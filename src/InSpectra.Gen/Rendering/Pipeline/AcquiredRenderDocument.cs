using InSpectra.Gen.OpenCli.Metadata;
using InSpectra.Gen.OpenCli.Model;
using InSpectra.Gen.Rendering.Contracts;
using InSpectra.Gen.Rendering.Pipeline.Model;

namespace InSpectra.Gen.Rendering.Pipeline;

public sealed class AcquiredRenderDocument
{
    public required OpenCliDocument RawDocument { get; init; }

    public required OpenCliDocument RenderDocument { get; init; }

    public string? XmlDocument { get; init; }

    public required RenderSourceInfo Source { get; init; }

    public OpenCliAcquisitionMetadata? Acquisition { get; init; }

    public required IReadOnlyList<string> Warnings { get; init; }
}
