namespace InSpectra.Gen.Runtime;

public sealed record OpenCliAcquisitionMetadata(
    string SelectedMode,
    string? CommandName,
    string? CliFramework,
    IReadOnlyList<OpenCliAcquisitionAttempt> Attempts,
    string? OpenCliOutputPath,
    string? CrawlOutputPath);
