using InSpectra.Gen.Runtime;
using InSpectra.Gen.Services;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Commands.Render;

public sealed class FileHtmlCommand(HtmlRenderService renderService) : AsyncCommand<FileHtmlSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, FileHtmlSettings settings, CancellationToken cancellationToken)
    {
        var options = RenderRequestFactory.CreateHtmlOptions(settings, null, null, settings.OutputDirectory, timeoutSeconds: null, hasTimeoutSupport: false);
        var features = RenderRequestFactory.CreateHtmlFeatureFlags(settings);
        var request = new FileRenderRequest(
            settings.OpenCliJsonPath,
            settings.XmlDocPath,
            options);

        return CommandOutputHandler.ExecuteAsync(
            options.OutputMode,
            options.Verbose,
            () => renderService.RenderFromFileAsync(request, features, cancellationToken));
    }
}

public sealed class FileHtmlSettings : HtmlCommandSettingsBase
{
    [CommandArgument(0, "<OPENCLI_JSON>")]
    public string OpenCliJsonPath { get; init; } = string.Empty;

    [CommandOption("--xmldoc <PATH>")]
    public string? XmlDocPath { get; init; }
}
