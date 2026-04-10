namespace InSpectra.Gen.Runtime;

public sealed record DotnetAcquisitionRequest(
    string ProjectPath,
    string? Configuration,
    string? Framework,
    string? LaunchProfile,
    bool NoBuild,
    bool NoRestore,
    OpenCliMode Mode,
    string? CommandName,
    string? CliFramework,
    IReadOnlyList<string> OpenCliArguments,
    bool IncludeXmlDoc,
    IReadOnlyList<string> XmlDocArguments,
    string WorkingDirectory,
    int TimeoutSeconds,
    OpenCliArtifactOptions Artifacts);
