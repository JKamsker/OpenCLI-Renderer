using System.ComponentModel;
using InSpectra.Gen.Runtime;
using InSpectra.Gen.Services;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Commands.Render;

public sealed class PackageHtmlCommand(HtmlRenderService renderService) : AsyncCommand<PackageHtmlSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, PackageHtmlSettings settings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.Version))
        {
            throw new CliUsageException("`--version` is required.");
        }

        var options = RenderRequestFactory.CreateHtmlOptions(settings, null, null, settings.OutputDirectory, settings.TimeoutSeconds, hasTimeoutSupport: true);
        var features = RenderRequestFactory.CreateHtmlFeatureFlags(settings);
        var themeOptions = RenderRequestFactory.CreateHtmlThemeOptions(settings);
        var request = new PackageRenderRequest(
            settings.PackageId,
            settings.Version,
            RenderRequestFactory.ResolveOpenCliMode(settings.OpenCliMode, OpenCliMode.Auto),
            settings.CommandName,
            settings.CliFramework,
            settings.OpenCliArguments.Length > 0 ? settings.OpenCliArguments : ["cli", "opencli"],
            settings.IncludeXmlDoc || settings.XmlDocArguments.Length > 0,
            settings.XmlDocArguments.Length > 0 ? settings.XmlDocArguments : ["cli", "xmldoc"],
            RenderRequestFactory.ResolveTimeoutSeconds(settings.TimeoutSeconds, defaultSeconds: 120),
            RenderRequestFactory.CreateArtifactOptions(settings.OpenCliOutputPath, settings.CrawlOutputPath),
            options);

        return CommandOutputHandler.ExecuteAsync(
            options.OutputMode,
            options.Verbose,
            () => renderService.RenderFromPackageAsync(request, features, cancellationToken, settings.Label, settings.Title, settings.CommandPrefix, themeOptions));
    }
}

public sealed class PackageHtmlSettings : AcquisitionHtmlCommandSettingsBase
{
    [Description("NuGet package id for the .NET tool package to analyze.")]
    [CommandArgument(0, "<PACKAGE_ID>")]
    public string PackageId { get; init; } = string.Empty;

    [Description("Package version to install and analyze.")]
    [CommandOption("--version <VERSION>")]
    public string Version { get; init; } = string.Empty;

    [Description("Also invoke the installed tool's cli xmldoc command for XML enrichment.")]
    [CommandOption("--with-xmldoc")]
    public bool IncludeXmlDoc { get; init; }

    [Description("Override the arguments used to invoke the installed tool's OpenCLI export command.")]
    [CommandOption("--opencli-arg <ARG>")]
    public string[] OpenCliArguments { get; init; } = [];

    [Description("Override the arguments used to invoke the installed tool's XML documentation export command.")]
    [CommandOption("--xmldoc-arg <ARG>")]
    public string[] XmlDocArguments { get; init; } = [];

    [Description("Timeout in seconds for package install and command execution.")]
    [CommandOption("--timeout <SECONDS>")]
    public int? TimeoutSeconds { get; init; }
}
