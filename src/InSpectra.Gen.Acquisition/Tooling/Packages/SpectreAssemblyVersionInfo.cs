namespace InSpectra.Gen.Acquisition.Tooling.Packages;

internal sealed record SpectreAssemblyVersionInfo(
    string Path,
    string? AssemblyVersion,
    string? FileVersion,
    string? InformationalVersion);
