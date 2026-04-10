using System.Text.Json.Serialization;

namespace InSpectra.Gen.Models.OpenCli;

public sealed class OpenCliConventions
{
    [JsonPropertyName("groupOptions")]
    public bool? GroupOptions { get; init; }

    [JsonPropertyName("optionSeparator")]
    public string? OptionSeparator { get; init; }
}
