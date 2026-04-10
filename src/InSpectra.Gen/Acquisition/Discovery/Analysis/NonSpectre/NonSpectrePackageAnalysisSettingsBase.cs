namespace InSpectra.Gen.Acquisition.Analysis.NonSpectre;

using InSpectra.Gen.Acquisition.Analysis.Settings;

using Spectre.Console.Cli;

internal abstract class NonSpectrePackageAnalysisSettingsBase : PackageAnalysisSettingsBase
{
    [CommandOption("--command|--tool-command <NAME>")]
    public string? Command { get; set; }
}
