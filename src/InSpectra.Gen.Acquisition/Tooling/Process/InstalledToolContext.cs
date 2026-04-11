namespace InSpectra.Gen.Acquisition.Tooling.Process;

internal sealed record InstalledToolContext(
    IReadOnlyDictionary<string, string> Environment,
    string InstallDirectory,
    string CommandPath,
    string? PreferredEntryPointPath = null);
