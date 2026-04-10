using System.ComponentModel;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Runtime;

/// <summary>
/// Shared CLI flags for the file and exec render commands.
/// </summary>
public abstract class CommonCommandSettings : CommandSettings
{
    [Description("Emit the stable machine-readable JSON envelope instead of human output.")]
    [CommandOption("--json")]
    public bool Json { get; init; }

    [Description("Override the output mode. Supported values are human and json.")]
    [CommandOption("--output <MODE>")]
    public string? Output { get; init; }

    [Description("Suppress non-essential console output.")]
    [CommandOption("-q|--quiet")]
    public bool Quiet { get; init; }

    [Description("Increase diagnostic detail in the rendered summary output.")]
    [CommandOption("--verbose")]
    public bool Verbose { get; init; }

    [Description("Disable ANSI color sequences in human-readable console output.")]
    [CommandOption("--no-color")]
    public bool NoColor { get; init; }

    [Description("Preview the resolved render plan without writing files.")]
    [CommandOption("--dry-run")]
    public bool DryRun { get; init; }

    [Description("Allow existing output files or directories to be replaced.")]
    [CommandOption("--overwrite")]
    public bool Overwrite { get; init; }

    [Description("Include commands and options marked hidden by the source CLI.")]
    [CommandOption("--include-hidden")]
    public bool IncludeHidden { get; init; }

    [Description("Include metadata sections in the rendered Markdown or HTML output.")]
    [CommandOption("--include-metadata")]
    public bool IncludeMetadata { get; init; }
}
