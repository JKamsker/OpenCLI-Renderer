namespace InSpectra.Gen.Acquisition.App.Summaries;


internal sealed record IndexBuildCommandSummary(
    string Command,
    string OutputPath,
    int PackageCount,
    string SortOrder);
