using InSpectra.Gen.Runtime;
using InSpectra.Gen.Runtime.Acquisition;

namespace InSpectra.Gen.Services;

public interface IOpenCliGenerationService
{
    Task<GenerateExecutionResult> GenerateFromExecAsync(
        ExecAcquisitionRequest request,
        string? outputFile,
        CancellationToken cancellationToken);

    Task<GenerateExecutionResult> GenerateFromDotnetAsync(
        DotnetAcquisitionRequest request,
        string? outputFile,
        CancellationToken cancellationToken);

    Task<GenerateExecutionResult> GenerateFromPackageAsync(
        PackageAcquisitionRequest request,
        string? outputFile,
        CancellationToken cancellationToken);
}
