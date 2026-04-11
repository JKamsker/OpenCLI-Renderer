namespace InSpectra.Gen.Acquisition.Modes.CliFx.Metadata;

internal sealed record CliFxCommandDefinition(
    string? Name,
    string? Description,
    IReadOnlyList<CliFxParameterDefinition> Parameters,
    IReadOnlyList<CliFxOptionDefinition> Options);
