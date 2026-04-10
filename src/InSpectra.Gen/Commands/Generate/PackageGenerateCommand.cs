using System.ComponentModel;
using InSpectra.Gen.Runtime;
using InSpectra.Gen.Runtime.Acquisition;
using InSpectra.Gen.Runtime.Output;
using InSpectra.Gen.Runtime.Settings;
using InSpectra.Gen.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Commands.Generate;

public sealed class PackageGenerateCommand(IOpenCliGenerationService generationService) : AsyncCommand<PackageGenerateSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, PackageGenerateSettings settings, CancellationToken cancellationToken)
    {
        var outputMode = RenderRequestFactory.ResolveOutputMode(settings);
        var request = new PackageAcquisitionRequest(
            settings.PackageId,
            settings.Version,
            RenderRequestFactory.ResolveOpenCliMode(settings.OpenCliMode, OpenCliMode.Auto),
            settings.CommandName,
            settings.CliFramework,
            settings.OpenCliArguments.Length > 0 ? settings.OpenCliArguments : ["cli", "opencli"],
            settings.WithXmlDoc,
            settings.XmlDocArguments.Length > 0 ? settings.XmlDocArguments : ["cli", "xmldoc"],
            RenderRequestFactory.ResolveTimeoutSeconds(settings.TimeoutSeconds, defaultSeconds: 120),
            new OpenCliArtifactOptions(null, settings.CrawlOutputPath));

        return GenerateOutputHandler.ExecuteAsync(
            outputMode,
            settings.Verbose,
            () => generationService.GenerateFromPackageAsync(request, settings.OutputFile, cancellationToken));
    }
}

public sealed class PackageGenerateSettings : GenerateCommandSettingsBase
{
    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Version))
        {
            return ValidationResult.Error("`--version` is required.");
        }

        return ValidationResult.Success();
    }

    [Description("NuGet package id for the .NET tool package to analyze.")]
    [CommandArgument(0, "<PACKAGE_ID>")]
    public string PackageId { get; init; } = string.Empty;

    [Description("Package version to install and analyze.")]
    [CommandOption("--version <VERSION>")]
    public string Version { get; init; } = string.Empty;

    [Description("Override the arguments used to invoke the installed tool's OpenCLI export command.")]
    [CommandOption("--opencli-arg <ARG>")]
    public string[] OpenCliArguments { get; init; } = [];

    [Description("Timeout in seconds for package install and command execution.")]
    [CommandOption("--timeout <SECONDS>")]
    public int? TimeoutSeconds { get; init; }
}
