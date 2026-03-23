using OpenCli.Renderer.Runtime;
using OpenCli.Renderer.Services;
using Spectre.Console.Cli;

namespace OpenCli.Renderer.Commands.Render;

public sealed class FileHtmlCommand(DocumentRenderService renderService, HtmlRenderer renderer) : AsyncCommand<FileRenderSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, FileRenderSettings settings, CancellationToken cancellationToken)
    {
        var options = RenderRequestFactory.CreateOptions(settings, settings.Layout, settings.OutputFile, settings.OutputDirectory, timeoutSeconds: null, hasTimeoutSupport: false);
        var request = new FileRenderRequest(
            settings.OpenCliJsonPath,
            settings.XmlDocPath,
            options);

        return CommandOutputHandler.ExecuteAsync(
            options.OutputMode,
            options.Verbose,
            () => renderService.RenderFromFileAsync(request, renderer, cancellationToken));
    }
}
