namespace InSpectra.Gen.Runtime.Acquisition;

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
