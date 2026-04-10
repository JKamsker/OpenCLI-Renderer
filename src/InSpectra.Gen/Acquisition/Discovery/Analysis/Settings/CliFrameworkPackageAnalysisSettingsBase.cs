namespace InSpectra.Gen.Acquisition.Analysis.Settings;

using InSpectra.Gen.Acquisition.Analysis.NonSpectre;

using Spectre.Console.Cli;

internal abstract class CliFrameworkPackageAnalysisSettingsBase : NonSpectrePackageAnalysisSettingsBase
{
    [CommandOption("--cli-framework|--framework <NAME>")]
    public string? CliFramework { get; set; }
}
