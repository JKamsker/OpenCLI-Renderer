namespace InSpectra.Gen.Acquisition.Modes.CliFx.Metadata;

internal sealed record CliFxHelpItem(
    string Key,
    bool IsRequired,
    string? Description);
