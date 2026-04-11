namespace InSpectra.Gen.Acquisition.Modes.Help.Crawling;

using InSpectra.Gen.Acquisition.Modes.Help.Documents;


using System.Text.Json.Nodes;

internal sealed record CrawlResult(
    IReadOnlyDictionary<string, Document> Documents,
    IReadOnlyDictionary<string, JsonObject> Captures,
    IReadOnlyDictionary<string, CaptureSummary> CaptureSummaries,
    string? GuardrailFailureMessage = null);
