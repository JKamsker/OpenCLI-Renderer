using System.Text.Json.Serialization;

namespace InSpectra.Gen.Runtime;

public sealed class JsonMeta
{
    [JsonPropertyName("schemaVersion")]
    public int SchemaVersion { get; init; } = 1;

    [JsonPropertyName("warnings")]
    public IReadOnlyList<string> Warnings { get; init; } = [];
}
