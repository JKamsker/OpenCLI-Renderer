using System.ComponentModel;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Runtime;

/// <summary>
/// Shared acquisition settings for render commands that can synthesize OpenCLI from source code or help output.
/// </summary>
public abstract class AcquisitionCommandSettingsBase : CommonCommandSettings
{
    [Description("OpenCLI acquisition mode: native, auto, help, clifx, static, or hook.")]
    [CommandOption("--opencli-mode <MODE>")]
    public string? OpenCliMode { get; init; }

    [Description("Override the root command name used for generated OpenCLI documents.")]
    [CommandOption("--command <NAME>")]
    public string? CommandName { get; init; }

    [Description("Hint or override the detected CLI framework for non-native analysis.")]
    [CommandOption("--cli-framework <NAME>")]
    public string? CliFramework { get; init; }

    [Description("Write the exact OpenCLI JSON used for rendering to this path.")]
    [CommandOption("--opencli-out <PATH>")]
    public string? OpenCliOutputPath { get; init; }

    [Description("Write crawl.json when the selected acquisition mode produces crawl data.")]
    [CommandOption("--crawl-out <PATH>")]
    public string? CrawlOutputPath { get; init; }
}

public abstract class AcquisitionHtmlCommandSettingsBase : HtmlCommandSettingsBase
{
    [Description("OpenCLI acquisition mode: native, auto, help, clifx, static, or hook.")]
    [CommandOption("--opencli-mode <MODE>")]
    public string? OpenCliMode { get; init; }

    [Description("Override the root command name used for generated OpenCLI documents.")]
    [CommandOption("--command <NAME>")]
    public string? CommandName { get; init; }

    [Description("Hint or override the detected CLI framework for non-native analysis.")]
    [CommandOption("--cli-framework <NAME>")]
    public string? CliFramework { get; init; }

    [Description("Write the exact OpenCLI JSON used for rendering to this path.")]
    [CommandOption("--opencli-out <PATH>")]
    public string? OpenCliOutputPath { get; init; }

    [Description("Write crawl.json when the selected acquisition mode produces crawl data.")]
    [CommandOption("--crawl-out <PATH>")]
    public string? CrawlOutputPath { get; init; }
}

public abstract class AcquisitionMarkdownCommandSettingsBase : MarkdownCommandSettingsBase
{
    [Description("OpenCLI acquisition mode: native, auto, help, clifx, static, or hook.")]
    [CommandOption("--opencli-mode <MODE>")]
    public string? OpenCliMode { get; init; }

    [Description("Override the root command name used for generated OpenCLI documents.")]
    [CommandOption("--command <NAME>")]
    public string? CommandName { get; init; }

    [Description("Hint or override the detected CLI framework for non-native analysis.")]
    [CommandOption("--cli-framework <NAME>")]
    public string? CliFramework { get; init; }

    [Description("Write the exact OpenCLI JSON used for rendering to this path.")]
    [CommandOption("--opencli-out <PATH>")]
    public string? OpenCliOutputPath { get; init; }

    [Description("Write crawl.json when the selected acquisition mode produces crawl data.")]
    [CommandOption("--crawl-out <PATH>")]
    public string? CrawlOutputPath { get; init; }
}
