namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record DotnetAcquisitionRequest(
    DotnetBuildSettings Build,
    string WorkingDirectory,
    AcquisitionOptions Options);
