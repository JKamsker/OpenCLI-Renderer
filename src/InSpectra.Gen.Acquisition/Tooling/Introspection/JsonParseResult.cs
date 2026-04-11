namespace InSpectra.Gen.Acquisition.Tooling.Introspection;

using System.Text.Json.Nodes;

internal sealed record JsonParseResult(
    bool Success,
    JsonNode? Document,
    string? ArtifactText,
    string? Error);
