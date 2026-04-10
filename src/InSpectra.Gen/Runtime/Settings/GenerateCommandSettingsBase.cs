using System.ComponentModel;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Runtime.Settings;

public abstract class GenerateCommandSettingsBase : CommonCommandSettings
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

    [Description("Enrich the generated OpenCLI document with XML documentation when the source CLI exposes it.")]
    [CommandOption("--with-xmldoc")]
    public bool WithXmlDoc { get; init; }

    [Description("Override the arguments used to invoke the source CLI's XML documentation export command.")]
    [CommandOption("--xmldoc-arg <ARG>")]
    public string[] XmlDocArguments { get; init; } = [];

    [Description("Write the generated OpenCLI JSON to this file instead of stdout.")]
    [CommandOption("--out <FILE>")]
    public string? OutputFile { get; init; }

    [Description("Write crawl.json when the selected acquisition mode produces crawl data.")]
    [CommandOption("--crawl-out <PATH>")]
    public string? CrawlOutputPath { get; init; }
}
