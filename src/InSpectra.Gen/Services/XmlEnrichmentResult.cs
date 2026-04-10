namespace InSpectra.Gen.Services;

public sealed class XmlEnrichmentResult
{
    public int MatchedCommandCount { get; set; }

    public int EnrichedDescriptionCount { get; set; }

    public List<string> Warnings { get; } = [];
}
