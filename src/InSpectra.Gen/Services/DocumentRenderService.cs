using InSpectra.Gen.Models;
using InSpectra.Gen.Runtime;

namespace InSpectra.Gen.Services;

public sealed class AcquiredRenderDocument
{
    public required OpenCliDocument RawDocument { get; init; }

    public required OpenCliDocument RenderDocument { get; init; }

    public string? XmlDocument { get; init; }

    public required RenderSourceInfo Source { get; init; }

    public OpenCliAcquisitionMetadata? Acquisition { get; init; }

    public required IReadOnlyList<string> Warnings { get; init; }
}

public sealed class DocumentRenderService(
    OpenCliDocumentLoader documentLoader,
    OpenCliDocumentCloner documentCloner,
    OpenCliXmlEnricher xmlEnricher,
    OpenCliAcquisitionService acquisitionService)
{
    public async Task<AcquiredRenderDocument> LoadFromFileAsync(
        FileRenderRequest request,
        CancellationToken cancellationToken)
    {
        var rawDocument = await documentLoader.LoadFromFileAsync(request.OpenCliJsonPath, cancellationToken);
        var xmlDocument = await LoadXmlDocumentAsync(request.XmlDocPath, cancellationToken);

        return CreatePreparedDocument(
            rawDocument,
            xmlDocument,
            new RenderSourceInfo(
                "file",
                Path.GetFullPath(request.OpenCliJsonPath),
                request.XmlDocPath is null ? null : Path.GetFullPath(request.XmlDocPath),
                null));
    }

    public async Task<AcquiredRenderDocument> LoadFromExecAsync(
        ExecRenderRequest request,
        CancellationToken cancellationToken)
    {
        var acquisition = await acquisitionService.AcquireFromExecAsync(
            new ExecAcquisitionRequest(
                request.Source,
                request.SourceArguments,
                request.Mode,
                request.CommandName,
                request.CliFramework,
                request.OpenCliArguments,
                request.IncludeXmlDoc,
                request.XmlDocArguments,
                request.WorkingDirectory,
                request.TimeoutSeconds,
                request.Artifacts),
            cancellationToken);
        var rawDocument = documentLoader.LoadFromJson(acquisition.OpenCliJson, acquisition.Source.OpenCliOrigin);

        return CreatePreparedDocument(
            rawDocument,
            acquisition.XmlDocument,
            acquisition.Source,
            acquisition.Metadata,
            acquisition.Warnings);
    }

    public async Task<AcquiredRenderDocument> LoadFromDotnetAsync(
        DotnetRenderRequest request,
        CancellationToken cancellationToken)
    {
        var acquisition = await acquisitionService.AcquireFromDotnetAsync(
            new DotnetAcquisitionRequest(
                request.ProjectPath,
                request.Configuration,
                request.Framework,
                request.LaunchProfile,
                request.NoBuild,
                request.NoRestore,
                request.Mode,
                request.CommandName,
                request.CliFramework,
                request.OpenCliArguments,
                request.IncludeXmlDoc,
                request.XmlDocArguments,
                request.WorkingDirectory,
                request.TimeoutSeconds,
                request.Artifacts),
            cancellationToken);
        var rawDocument = documentLoader.LoadFromJson(acquisition.OpenCliJson, acquisition.Source.OpenCliOrigin);

        return CreatePreparedDocument(
            rawDocument,
            acquisition.XmlDocument,
            acquisition.Source,
            acquisition.Metadata,
            acquisition.Warnings);
    }

    public async Task<AcquiredRenderDocument> LoadFromPackageAsync(
        PackageRenderRequest request,
        CancellationToken cancellationToken)
    {
        var acquisition = await acquisitionService.AcquireFromPackageAsync(
            new PackageAcquisitionRequest(
                request.PackageId,
                request.Version,
                request.Mode,
                request.CommandName,
                request.CliFramework,
                request.OpenCliArguments,
                request.IncludeXmlDoc,
                request.XmlDocArguments,
                request.TimeoutSeconds,
                request.Artifacts),
            cancellationToken);
        var rawDocument = documentLoader.LoadFromJson(acquisition.OpenCliJson, acquisition.Source.OpenCliOrigin);

        return CreatePreparedDocument(
            rawDocument,
            acquisition.XmlDocument,
            acquisition.Source,
            acquisition.Metadata,
            acquisition.Warnings);
    }

    private AcquiredRenderDocument CreatePreparedDocument(
        OpenCliDocument rawDocument,
        string? xmlDocument,
        RenderSourceInfo source,
        OpenCliAcquisitionMetadata? acquisition = null,
        IReadOnlyList<string>? acquisitionWarnings = null)
    {
        var renderDocument = documentCloner.Clone(rawDocument);
        var warnings = acquisitionWarnings?.ToList() ?? [];

        if (!string.IsNullOrWhiteSpace(xmlDocument))
        {
            var enrichment = xmlEnricher.EnrichFromXml(
                renderDocument,
                xmlDocument,
                source.XmlDocOrigin ?? source.OpenCliOrigin);
            warnings.AddRange(enrichment.Warnings);
        }

        return new AcquiredRenderDocument
        {
            RawDocument = rawDocument,
            RenderDocument = renderDocument,
            XmlDocument = xmlDocument,
            Source = source,
            Acquisition = acquisition,
            Warnings = warnings,
        };
    }

    private static async Task<string?> LoadXmlDocumentAsync(string? path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        var resolvedPath = Path.GetFullPath(path);
        if (!File.Exists(resolvedPath))
        {
            throw new CliUsageException($"XML enrichment file `{resolvedPath}` does not exist.");
        }

        return await File.ReadAllTextAsync(resolvedPath, cancellationToken);
    }
}
