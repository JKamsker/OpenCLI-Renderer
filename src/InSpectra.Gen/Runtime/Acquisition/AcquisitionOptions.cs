namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record AcquisitionOptions(
    OpenCliMode Mode,
    string? CommandName,
    string? CliFramework,
    IReadOnlyList<string> OpenCliArguments,
    bool IncludeXmlDoc,
    IReadOnlyList<string> XmlDocArguments,
    int TimeoutSeconds,
    OpenCliArtifactOptions Artifacts);
