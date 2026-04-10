using InSpectra.Gen.Runtime;

namespace InSpectra.Gen.Services;

public sealed class OpenCliGenerationService(
    OpenCliAcquisitionService acquisitionService,
    OpenCliDocumentLoader documentLoader)
{
    public Task<GenerateExecutionResult> GenerateFromExecAsync(ExecAcquisitionRequest request, string? outputFile, CancellationToken cancellationToken)
        => GenerateAsync(() => acquisitionService.AcquireFromExecAsync(request, cancellationToken), outputFile, cancellationToken);

    public Task<GenerateExecutionResult> GenerateFromDotnetAsync(DotnetAcquisitionRequest request, string? outputFile, CancellationToken cancellationToken)
        => GenerateAsync(() => acquisitionService.AcquireFromDotnetAsync(request, cancellationToken), outputFile, cancellationToken);

    public Task<GenerateExecutionResult> GenerateFromPackageAsync(PackageAcquisitionRequest request, string? outputFile, CancellationToken cancellationToken)
        => GenerateAsync(() => acquisitionService.AcquireFromPackageAsync(request, cancellationToken), outputFile, cancellationToken);

    private async Task<GenerateExecutionResult> GenerateAsync(
        Func<Task<OpenCliAcquisitionResult>> action,
        string? outputFile,
        CancellationToken cancellationToken)
    {
        var acquisition = await action();
        _ = documentLoader.LoadFromJson(acquisition.OpenCliJson, acquisition.Source.OpenCliOrigin);

        string? resolvedOutputFile = null;
        if (!string.IsNullOrWhiteSpace(outputFile))
        {
            resolvedOutputFile = Path.GetFullPath(outputFile);
            var directory = Path.GetDirectoryName(resolvedOutputFile);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(resolvedOutputFile, acquisition.OpenCliJson, cancellationToken);
        }

        return new GenerateExecutionResult(
            acquisition.Source,
            acquisition.Metadata,
            acquisition.Warnings,
            acquisition.OpenCliJson,
            resolvedOutputFile);
    }
}
