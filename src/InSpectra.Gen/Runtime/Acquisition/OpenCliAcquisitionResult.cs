using InSpectra.Gen.Runtime.Rendering;

namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record OpenCliAcquisitionResult(
    string OpenCliJson,
    string? XmlDocument,
    string? CrawlJson,
    RenderSourceInfo Source,
    OpenCliAcquisitionMetadata Metadata,
    IReadOnlyList<string> Warnings);
