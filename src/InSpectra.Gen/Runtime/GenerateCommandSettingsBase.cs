using System.ComponentModel;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Runtime;

public abstract class GenerateCommandSettingsBase : CommandSettings
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

    [Description("Increase diagnostic detail in machine-readable failures.")]
    [CommandOption("--verbose")]
    public bool Verbose { get; init; }

    [Description("Disable ANSI color sequences in human-readable console output.")]
    [CommandOption("--no-color")]
    public bool NoColor { get; init; }

    [Description("OpenCLI acquisition mode: native, auto, help, clifx, static, or hook.")]
    [CommandOption("--opencli-mode <MODE>")]
    public string? OpenCliMode { get; init; }

    [Description("Override the root command name used for generated OpenCLI documents.")]
    [CommandOption("--command <NAME>")]
    public string? CommandName { get; init; }

    [Description("Hint or override the detected CLI framework for non-native analysis.")]
    [CommandOption("--cli-framework <NAME>")]
    public string? CliFramework { get; init; }

    [Description("Write the generated OpenCLI JSON to this file instead of stdout.")]
    [CommandOption("--out <FILE>")]
    public string? OutputFile { get; init; }

    [Description("Write crawl.json when the selected acquisition mode produces crawl data.")]
    [CommandOption("--crawl-out <PATH>")]
    public string? CrawlOutputPath { get; init; }
}
