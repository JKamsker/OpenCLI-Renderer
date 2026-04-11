using System.Text.Json.Serialization;

namespace InSpectra.Gen.OpenCli.Model;

public sealed class OpenCliExitCode
{
    [JsonPropertyName("code")]
    public int Code { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
