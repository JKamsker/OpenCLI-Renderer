namespace InSpectra.Gen.Acquisition.App.Summaries;


internal sealed record IndexDeltaSpectreConsoleCliCommandSummary(
    string Command,
    string InputDeltaPath,
    string OutputDeltaPath,
    string QueueOutputPath,
    int ScannedChangeCount,
    int MatchedPackageCount,
    int QueueCount);
