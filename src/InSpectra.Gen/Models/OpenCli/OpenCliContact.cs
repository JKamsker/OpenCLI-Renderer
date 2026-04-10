using System.Text.Json.Serialization;

namespace InSpectra.Gen.Models.OpenCli;

public sealed class OpenCliContact
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }
}
