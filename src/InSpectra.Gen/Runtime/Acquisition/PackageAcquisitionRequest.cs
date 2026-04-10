namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record PackageAcquisitionRequest(
    string PackageId,
    string Version,
    AcquisitionOptions Options);
