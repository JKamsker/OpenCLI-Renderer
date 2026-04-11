namespace InSpectra.Gen.Acquisition.Modes.Static.Metadata;

internal sealed record StaticValueDefinition(
    int Index,
    string? Name,
    bool IsRequired,
    bool IsSequence,
    string? ClrType,
    string? Description,
    string? DefaultValue,
    IReadOnlyList<string> AcceptedValues);
