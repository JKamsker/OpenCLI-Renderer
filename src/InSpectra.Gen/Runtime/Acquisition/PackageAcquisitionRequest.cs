namespace InSpectra.Gen.Runtime.Acquisition;

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
