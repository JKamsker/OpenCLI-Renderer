namespace InSpectra.Gen.Runtime.Acquisition;

public sealed record OpenCliAcquisitionAttempt(
    string Mode,
    string? Framework,
    string Outcome,
    string? Detail = null);
