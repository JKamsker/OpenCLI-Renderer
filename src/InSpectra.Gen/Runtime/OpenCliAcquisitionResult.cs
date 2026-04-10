namespace InSpectra.Gen.Runtime;

public sealed record OpenCliAcquisitionResult(
    string OpenCliJson,
    string? XmlDocument,
    string? CrawlJson,
    RenderSourceInfo Source,
    OpenCliAcquisitionMetadata Metadata,
    IReadOnlyList<string> Warnings);
