using OpenCli.Renderer.Models;
using OpenCli.Renderer.Runtime;

namespace OpenCli.Renderer.Services;

public sealed class DocumentRenderService(
    OpenCliDocumentLoader documentLoader,
    OpenCliXmlEnricher xmlEnricher,
    OpenCliNormalizer normalizer,
    ExecutableResolver executableResolver,
    ProcessRunner processRunner)
{
    public async Task<RenderExecutionResult> RenderFromFileAsync(
        FileRenderRequest request,
        IDocumentRenderer renderer,
        CancellationToken cancellationToken)
    {
        var document = await documentLoader.LoadFromFileAsync(request.OpenCliJsonPath, cancellationToken);
        var warnings = new List<string>();
        if (!string.IsNullOrWhiteSpace(request.XmlDocPath))
        {
            var enrichment = await xmlEnricher.EnrichFromFileAsync(document, request.XmlDocPath, cancellationToken);
            warnings.AddRange(enrichment.Warnings);
        }

        return await RenderAsync(
            document,
            new RenderSourceInfo("file", Path.GetFullPath(request.OpenCliJsonPath), request.XmlDocPath is null ? null : Path.GetFullPath(request.XmlDocPath), null),
            request.Options,
            warnings,
            renderer);
    }

    public async Task<RenderExecutionResult> RenderFromExecAsync(
        ExecRenderRequest request,
        IDocumentRenderer renderer,
        CancellationToken cancellationToken)
    {
        var executablePath = executableResolver.Resolve(request.Source, request.WorkingDirectory);
        var openCliArguments = request.SourceArguments.Concat(request.OpenCliArguments).ToArray();
        var openCliResult = await processRunner.RunAsync(
            executablePath,
            request.WorkingDirectory,
            openCliArguments,
            request.TimeoutSeconds,
            cancellationToken);

        var document = documentLoader.LoadFromJson(openCliResult.StandardOutput, executablePath);
        var warnings = new List<string>();
        string? xmlOrigin = null;

        if (request.IncludeXmlDoc)
        {
            var xmlArguments = request.SourceArguments.Concat(request.XmlDocArguments).ToArray();
            var xmlResult = await processRunner.RunAsync(
                executablePath,
                request.WorkingDirectory,
                xmlArguments,
                request.TimeoutSeconds,
                cancellationToken);

            var enrichment = xmlEnricher.EnrichFromXml(document, xmlResult.StandardOutput, executablePath);
            warnings.AddRange(enrichment.Warnings);
            xmlOrigin = executablePath;
        }

        return await RenderAsync(
            document,
            new RenderSourceInfo("exec", executablePath, xmlOrigin, executablePath),
            request.Options,
            warnings,
            renderer);
    }

    private Task<RenderExecutionResult> RenderAsync(
        OpenCliDocument document,
        RenderSourceInfo source,
        RenderExecutionOptions options,
        IReadOnlyList<string> warnings,
        IDocumentRenderer renderer)
    {
        var normalized = normalizer.Normalize(document, options.IncludeHidden);

        return options.Layout switch
        {
            MarkdownLayout.Single => Task.FromResult(HandleSingleLayout(renderer, renderer.RenderSingle(normalized, options.IncludeMetadata), source, normalized, options, warnings)),
            MarkdownLayout.Tree => Task.FromResult(HandleTreeLayout(renderer, renderer.RenderTree(normalized, options.IncludeMetadata), source, normalized, options, warnings)),
            _ => throw new ArgumentOutOfRangeException(nameof(options.Layout)),
        };
    }

    private static RenderExecutionResult HandleSingleLayout(
        IDocumentRenderer renderer,
        string content,
        RenderSourceInfo source,
        NormalizedCliDocument document,
        RenderExecutionOptions options,
        IReadOnlyList<string> warnings)
    {
        if (options.DryRun)
        {
            IReadOnlyList<RenderedFile> plannedFiles = options.OutputFile is null
                ? Array.Empty<RenderedFile>()
                : [new RenderedFile(Path.GetFileName(options.OutputFile), options.OutputFile, string.Empty)];

            return CreateResult(
                renderer,
                source,
                document,
                warnings,
                options,
                plannedFiles,
                options.OutputFile is null ? $"Dry run: render `{source.OpenCliOrigin}` as single {FormatDisplayName(renderer)} to stdout." : $"Dry run: render `{source.OpenCliOrigin}` as single {FormatDisplayName(renderer)} to `{options.OutputFile}`.");
        }

        if (options.OutputFile is null)
        {
            return CreateResult(
                renderer,
                source,
                document,
                warnings,
                options,
                [],
                null,
                content);
        }

        EnsureFileWritable(options.OutputFile, options.Overwrite);
        var directory = Path.GetDirectoryName(options.OutputFile);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(options.OutputFile, content);

        var written = new RenderedFile(Path.GetFileName(options.OutputFile), options.OutputFile, content);
        var summary = options.Quiet
            ? null
            : $"Wrote {FormatDisplayName(renderer)} to `{options.OutputFile}`.";

        return CreateResult(renderer, source, document, warnings, options, [written], summary);
    }

    private static RenderExecutionResult HandleTreeLayout(
        IDocumentRenderer renderer,
        IReadOnlyList<RelativeRenderedFile> files,
        RenderSourceInfo source,
        NormalizedCliDocument document,
        RenderExecutionOptions options,
        IReadOnlyList<string> warnings)
    {
        var outputDirectory = options.OutputDirectory
            ?? throw new CliUsageException("`--layout tree` requires `--out-dir`.");

        if (options.DryRun)
        {
            var planned = files
                .Select(file => new RenderedFile(file.RelativePath, Path.Combine(outputDirectory, file.RelativePath), string.Empty))
                .ToList();

            return CreateResult(
                renderer,
                source,
                document,
                warnings,
                options,
                planned,
                $"Dry run: render `{source.OpenCliOrigin}` as a {FormatDisplayName(renderer)} tree in `{outputDirectory}` ({planned.Count} files planned).");
        }

        PrepareDirectory(outputDirectory, options.Overwrite);

        var writtenFiles = new List<RenderedFile>();
        foreach (var file in files)
        {
            var fullPath = Path.Combine(outputDirectory, file.RelativePath);
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(fullPath, file.Content);
            writtenFiles.Add(new RenderedFile(file.RelativePath, fullPath, file.Content));
        }

        var summary = options.Quiet
            ? null
            : $"Wrote {writtenFiles.Count} {FormatDisplayName(renderer)} files to `{outputDirectory}`.";

        return CreateResult(renderer, source, document, warnings, options, writtenFiles, summary);
    }

    private static RenderExecutionResult CreateResult(
        IDocumentRenderer renderer,
        RenderSourceInfo source,
        NormalizedCliDocument document,
        IReadOnlyList<string> warnings,
        RenderExecutionOptions options,
        IReadOnlyList<RenderedFile> files,
        string? summary,
        string? stdoutDocument = null)
    {
        return new RenderExecutionResult
        {
            Format = renderer.Format,
            Layout = options.Layout,
            Source = source,
            Warnings = warnings,
            IsDryRun = options.DryRun,
            StdoutDocument = stdoutDocument,
            Files = files,
            Summary = summary,
            Stats = new RenderStats(
                CountCommands(document.Commands),
                CountOptions(document),
                CountArguments(document),
                files.Count),
        };
    }

    private static string FormatDisplayName(IDocumentRenderer renderer)
    {
        return renderer.Format == DocumentFormat.Html ? "HTML" : "Markdown";
    }

    private static int CountCommands(IEnumerable<NormalizedCommand> commands)
    {
        return commands.Sum(command => 1 + CountCommands(command.Commands));
    }

    private static int CountOptions(NormalizedCliDocument document)
    {
        return document.RootOptions.Count + CountOptions(document.Commands);
    }

    private static int CountOptions(IEnumerable<NormalizedCommand> commands)
    {
        return commands.Sum(command => command.DeclaredOptions.Count + CountOptions(command.Commands));
    }

    private static int CountArguments(NormalizedCliDocument document)
    {
        return document.RootArguments.Count + CountArguments(document.Commands) + CountOptionArguments(document.RootOptions);
    }

    private static int CountArguments(IEnumerable<NormalizedCommand> commands)
    {
        return commands.Sum(command =>
            command.Arguments.Count +
            CountOptionArguments(command.DeclaredOptions) +
            CountArguments(command.Commands));
    }

    private static int CountOptionArguments(IEnumerable<OpenCliOption> options)
    {
        return options.Sum(option => option.Arguments.Count);
    }

    private static void EnsureFileWritable(string outputFile, bool overwrite)
    {
        if (File.Exists(outputFile) && !overwrite)
        {
            throw new CliUsageException($"Output file `{outputFile}` already exists. Use `--overwrite` to replace it.");
        }
    }

    private static void PrepareDirectory(string outputDirectory, bool overwrite)
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
            return;
        }

        if (!Directory.EnumerateFileSystemEntries(outputDirectory).Any())
        {
            return;
        }

        if (!overwrite)
        {
            throw new CliUsageException($"Output directory `{outputDirectory}` is not empty. Use `--overwrite` to replace it.");
        }

        Directory.Delete(outputDirectory, recursive: true);
        Directory.CreateDirectory(outputDirectory);
    }
}
