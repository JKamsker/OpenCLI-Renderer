using System.Text.Json.Serialization;

namespace InSpectra.Gen.Models.OpenCli;

public sealed class OpenCliArity
{
    [JsonPropertyName("minimum")]
    public int? Minimum { get; init; }

    [JsonPropertyName("maximum")]
    public int? Maximum { get; init; }
}
