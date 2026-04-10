using System.ComponentModel;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Runtime;

/// <summary>
/// Shared flags supported by the self-documentation command.
/// </summary>
public abstract class SelfDocCommandSettingsBase : CommandSettings
{
    [Description("Allow existing self-documentation output to be replaced.")]
    [CommandOption("--overwrite")]
    public bool Overwrite { get; init; }

    [Description("Include hidden commands and options from InSpectra's own CLI surface.")]
    [CommandOption("--include-hidden")]
    public bool IncludeHidden { get; init; }

    [Description("Include metadata sections in the generated Markdown tree and HTML bundle.")]
    [CommandOption("--include-metadata")]
    public bool IncludeMetadata { get; init; }
}

/// <summary>
/// Shared HTML feature flags supported by the self-documentation command.
/// </summary>
public abstract class SelfDocHtmlCommandSettingsBase : SelfDocCommandSettingsBase
{
    [Description("Directory where the self-documentation bundle should be written.")]
    [CommandOption("--out-dir <DIR>")]
    public string? OutputDirectory { get; init; }

    [Description("Show the viewer home screen button in the generated self-documentation app.")]
    [CommandOption("--show-home")]
    public bool ShowHome { get; init; }

    [Description("Hide the interactive command composer from the generated self-documentation app.")]
    [CommandOption("--no-composer")]
    public bool NoComposer { get; init; }

    [Description("Disable dark mode in the generated self-documentation app.")]
    [CommandOption("--no-dark")]
    public bool NoDark { get; init; }

    [Description("Disable light mode in the generated self-documentation app.")]
    [CommandOption("--no-light")]
    public bool NoLight { get; init; }

    [Description("Allow the viewer to load alternate OpenCLI inputs from URL query parameters.")]
    [CommandOption("--enable-url")]
    public bool EnableUrl { get; init; }

    [Description("Enable the NuGet package browser on the self-documentation viewer home screen.")]
    [CommandOption("--enable-nuget-browser")]
    public bool EnableNugetBrowser { get; init; }

    [Description("Enable local package upload on the self-documentation viewer home screen.")]
    [CommandOption("--enable-package-upload")]
    public bool EnablePackageUpload { get; init; }

    [Description("Custom label shown in the viewer header (e.g. a version string).")]
    [CommandOption("--label <TEXT>")]
    public string? Label { get; init; }

    [Description("Override the CLI title shown in the viewer header and overview.")]
    [CommandOption("--title <TEXT>")]
    public string? Title { get; init; }

    [Description("Override the CLI command prefix used in generated examples and the composer.")]
    [CommandOption("--command-prefix <TEXT>")]
    public string? CommandPrefix { get; init; }

    [Description("Emit a single self-contained HTML file with all assets inlined. Works from file:// without a web server.")]
    [CommandOption("--single-file")]
    public bool SingleFile { get; init; }

    [Description("Compression level: 0 = none, 1 = compress embedded JSON, 2 = self-extracting bundle (default 2).")]
    [CommandOption("--compression-level <LEVEL>")]
    public int? CompressionLevel { get; init; }

    [Description("Set the initial theme mode (light or dark).")]
    [CommandOption("--theme <MODE>")]
    public string? Theme { get; init; }

    [Description("Set the color theme (cyan, indigo, emerald, amber, rose, blue).")]
    [CommandOption("--color-theme <NAME>")]
    public string? ColorTheme { get; init; }

    [Description("Custom accent color for light mode (hex, e.g. \"#7c3aed\").")]
    [CommandOption("--accent <COLOR>")]
    public string? Accent { get; init; }

    [Description("Custom accent color for dark mode (hex). Falls back to --accent if omitted.")]
    [CommandOption("--accent-dark <COLOR>")]
    public string? AccentDark { get; init; }

    [Description("Hide the color theme picker from the viewer toolbar.")]
    [CommandOption("--no-theme-picker")]
    public bool NoThemePicker { get; init; }
}
