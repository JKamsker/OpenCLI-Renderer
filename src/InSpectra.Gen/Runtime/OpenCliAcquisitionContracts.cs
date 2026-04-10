namespace InSpectra.Gen.Runtime;

public enum OpenCliMode
{
    Native,
    Auto,
    Help,
    CliFx,
    Static,
    Hook,
}

public sealed record OpenCliArtifactOptions(
    string? OpenCliOutputPath,
    string? CrawlOutputPath);

public sealed record OpenCliAcquisitionAttempt(
    string Mode,
    string? Framework,
    string Outcome,
    string? Detail = null);

public sealed record OpenCliAcquisitionMetadata(
    string SelectedMode,
    string? CommandName,
    string? CliFramework,
    IReadOnlyList<OpenCliAcquisitionAttempt> Attempts,
    string? OpenCliOutputPath,
    string? CrawlOutputPath);

public sealed record OpenCliAcquisitionResult(
    string OpenCliJson,
    string? XmlDocument,
    string? CrawlJson,
    RenderSourceInfo Source,
    OpenCliAcquisitionMetadata Metadata,
    IReadOnlyList<string> Warnings);

public sealed record ExecAcquisitionRequest(
    string Source,
    IReadOnlyList<string> SourceArguments,
    OpenCliMode Mode,
    string? CommandName,
    string? CliFramework,
    IReadOnlyList<string> OpenCliArguments,
    bool IncludeXmlDoc,
    IReadOnlyList<string> XmlDocArguments,
    string WorkingDirectory,
    int TimeoutSeconds,
    OpenCliArtifactOptions Artifacts);

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

public sealed record PackageAcquisitionRequest(
    string PackageId,
    string Version,
    OpenCliMode Mode,
    string? CommandName,
    string? CliFramework,
    IReadOnlyList<string> OpenCliArguments,
    bool IncludeXmlDoc,
    IReadOnlyList<string> XmlDocArguments,
    int TimeoutSeconds,
    OpenCliArtifactOptions Artifacts);
