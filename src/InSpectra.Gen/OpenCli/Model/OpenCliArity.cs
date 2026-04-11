using System.Text.Json.Serialization;

namespace InSpectra.Gen.OpenCli.Model;

public sealed class OpenCliArity
{
    [JsonPropertyName("minimum")]
    public int? Minimum { get; init; }

    [JsonPropertyName("maximum")]
    public int? Maximum { get; init; }
}
