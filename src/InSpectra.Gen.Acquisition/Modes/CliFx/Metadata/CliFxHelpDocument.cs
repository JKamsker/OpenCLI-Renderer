namespace InSpectra.Gen.Acquisition.Modes.CliFx.Metadata;

internal sealed record CliFxHelpDocument(
    string? Title,
    string? Version,
    string? ApplicationDescription,
    string? CommandDescription,
    IReadOnlyList<string> UsageLines,
    IReadOnlyList<CliFxHelpItem> Parameters,
    IReadOnlyList<CliFxHelpItem> Options,
    IReadOnlyList<CliFxHelpItem> Commands);
