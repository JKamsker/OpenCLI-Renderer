namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record DotnetBuildSettings(
    string ProjectPath,
    string? Configuration,
    string? Framework,
    string? LaunchProfile,
    bool NoBuild,
    bool NoRestore);
