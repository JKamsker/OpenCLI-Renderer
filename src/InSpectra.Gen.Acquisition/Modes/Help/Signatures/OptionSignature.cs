namespace InSpectra.Gen.Acquisition.Modes.Help.Signatures;


internal sealed record OptionSignature(
    string? PrimaryName,
    IReadOnlyList<string> Aliases,
    string? ArgumentName,
    bool ArgumentRequired);
