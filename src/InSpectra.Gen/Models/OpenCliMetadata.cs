using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace InSpectra.Gen.Models;

public sealed class OpenCliMetadata
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("value")]
    public JsonNode? Value { get; init; }
}
