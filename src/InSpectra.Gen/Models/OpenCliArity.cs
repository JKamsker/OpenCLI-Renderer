using System.Text.Json.Serialization;

namespace InSpectra.Gen.Models;

public sealed class OpenCliArity
{
    [JsonPropertyName("minimum")]
    public int? Minimum { get; init; }

    [JsonPropertyName("maximum")]
    public int? Maximum { get; init; }
}
