namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record ExecAcquisitionRequest(
    string Source,
    IReadOnlyList<string> SourceArguments,
    string WorkingDirectory,
    AcquisitionOptions Options);
