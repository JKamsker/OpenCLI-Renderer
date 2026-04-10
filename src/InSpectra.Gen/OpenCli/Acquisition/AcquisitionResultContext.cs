using InSpectra.Gen.Runtime.Acquisition;

namespace InSpectra.Gen.OpenCli.Acquisition;

internal sealed record AcquisitionResultContext(
    string Kind,
    string SourceLabel,
    string? ReportedExecutablePath,
    string? CommandName,
    string? CliFramework,
    OpenCliArtifactOptions Artifacts);
