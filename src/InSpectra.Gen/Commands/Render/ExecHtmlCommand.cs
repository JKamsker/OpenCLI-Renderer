using InSpectra.Gen.Runtime;
using InSpectra.Gen.Services;
using Spectre.Console.Cli;

namespace InSpectra.Gen.Commands.Render;

public sealed class ExecHtmlCommand(HtmlRenderService renderService) : AsyncCommand<ExecHtmlSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, ExecHtmlSettings settings, CancellationToken cancellationToken)
    {
        var options = RenderRequestFactory.CreateHtmlOptions(settings, null, null, settings.OutputDirectory, settings.TimeoutSeconds, hasTimeoutSupport: true);
        var features = RenderRequestFactory.CreateHtmlFeatureFlags(settings);
        var workingDirectory = RenderRequestFactory.ResolveWorkingDirectory(settings.WorkingDirectory);
        var request = new ExecRenderRequest(
            settings.Source,
            settings.SourceArguments,
            settings.OpenCliArguments.Length > 0 ? settings.OpenCliArguments : ["cli", "opencli"],
            settings.IncludeXmlDoc || settings.XmlDocArguments.Length > 0,
            settings.XmlDocArguments.Length > 0 ? settings.XmlDocArguments : ["cli", "xmldoc"],
            workingDirectory,
            RenderRequestFactory.ResolveTimeoutSeconds(settings.TimeoutSeconds),
            options);

        return CommandOutputHandler.ExecuteAsync(
            options.OutputMode,
            options.Verbose,
            () => renderService.RenderFromExecAsync(request, features, cancellationToken));
    }
}

public sealed class ExecHtmlSettings : HtmlCommandSettingsBase
{
    [CommandArgument(0, "<SOURCE>")]
    public string Source { get; init; } = string.Empty;

    [CommandOption("--source-arg <ARG>")]
    public string[] SourceArguments { get; init; } = [];

    [CommandOption("--opencli-arg <ARG>")]
    public string[] OpenCliArguments { get; init; } = [];

    [CommandOption("--with-xmldoc")]
    public bool IncludeXmlDoc { get; init; }

    [CommandOption("--xmldoc-arg <ARG>")]
    public string[] XmlDocArguments { get; init; } = [];

    [CommandOption("--cwd <PATH>")]
    public string? WorkingDirectory { get; init; }

    [CommandOption("--timeout <SECONDS>")]
    public int? TimeoutSeconds { get; init; }
}
