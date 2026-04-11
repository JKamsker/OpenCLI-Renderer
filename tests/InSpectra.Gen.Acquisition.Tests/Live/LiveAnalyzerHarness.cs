namespace InSpectra.Gen.Acquisition.Tests.Live;

using InSpectra.Gen.Acquisition.Modes.Hook;
using InSpectra.Gen.Acquisition.Tooling;
using InSpectra.Gen.Acquisition.Tooling.Paths;
using InSpectra.Gen.Acquisition.Tooling.Process;
using InSpectra.Gen.Acquisition.Tooling.Results;

using System.Text.Json;
using System.Text.Json.Nodes;

/// <summary>
/// Adapts <see cref="HookInstalledToolAnalysisSupport"/> back to the end-to-end shape
/// that the legacy discovery-repo <c>HookService.RunAsync</c> entry point exposed: install
/// a tool from NuGet, run hook-based analysis, and land <c>result.json</c> and
/// <c>opencli.json</c> artifacts under <paramref name="outputRoot"/>.
/// Preserves the classification/disposition assertions the ported live tests rely on.
/// </summary>
internal static class LiveAnalyzerHarness
{
    private const string AnalysisMode = "hook";
    private const string BatchId = "live-hook";
    private const string Source = "live-hook-test";

    private static readonly JsonSerializerOptions ResultJsonOptions = new()
    {
        WriteIndented = true,
    };

    /// <summary>
    /// Installs the requested NuGet tool into a sandbox under <paramref name="outputRoot"/>,
    /// drives hook-based analysis, serializes the resulting <c>result.json</c> next to the
    /// <c>opencli.json</c> artifact, and returns 0 on success.
    /// </summary>
    public static async Task<int> RunHookAsync(
        string packageId,
        string version,
        string commandName,
        string cliFramework,
        string outputRoot,
        int installTimeoutSeconds,
        int commandTimeoutSeconds,
        CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(outputRoot);
        var tempRoot = Path.Combine(outputRoot, "workspace");
        Directory.CreateDirectory(tempRoot);

        var result = NonSpectreResultSupport.CreateInitialResult(
            packageId,
            version,
            commandName,
            batchId: BatchId,
            attempt: 1,
            source: Source,
            cliFramework: cliFramework,
            analysisMode: AnalysisMode,
            analyzedAt: DateTimeOffset.UtcNow);

        // Mirror AcquisitionAnalysisDispatcher: fetch nugetTitle/nugetDescription from the
        // catalog leaf so HookInstalledToolAnalysisSupport.ApplyNuGetMetadata can overlay
        // the NuGet package title/description onto the OpenCLI document — which matches
        // the behaviour the frozen snapshot fixtures were captured under.
        await TryAttachNuGetMetadataAsync(result, packageId, version, cancellationToken);

        try
        {
            var runtime = new CommandRuntime();
            var analyzer = new HookInstalledToolAnalysisSupport(runtime);
            await analyzer.AnalyzeAsync(
                result,
                packageId,
                version,
                commandName,
                outputDirectory: outputRoot,
                tempRoot: tempRoot,
                installTimeoutSeconds: installTimeoutSeconds,
                commandTimeoutSeconds: commandTimeoutSeconds,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            NonSpectreResultSupport.ApplyUnexpectedRetryableFailure(result, ex.ToString());
        }
        finally
        {
            NonSpectreResultSupport.FinalizeFailureSignature(result);
            WriteResult(outputRoot, result);
        }

        var disposition = result["disposition"]?.GetValue<string>();
        return string.Equals(disposition, "success", StringComparison.Ordinal) ? 0 : 1;
    }

    private static async Task TryAttachNuGetMetadataAsync(
        JsonObject result,
        string packageId,
        string version,
        CancellationToken cancellationToken)
    {
        try
        {
            using var scope = ApplicationLifetime.CreateNuGetApiClientScope();
            var (_, catalogLeaf) = await PackageVersionResolver.ResolveAsync(
                scope.Client,
                packageId,
                version,
                cancellationToken);
            result["nugetTitle"] = catalogLeaf.Title;
            result["nugetDescription"] = catalogLeaf.Description;
        }
        catch
        {
            // Best-effort: if metadata fetching fails, continue with the in-tool title/description.
        }
    }

    private static void WriteResult(string outputRoot, JsonObject result)
    {
        var resultPath = Path.Combine(outputRoot, "result.json");
        File.WriteAllText(resultPath, result.ToJsonString(ResultJsonOptions));
    }
}
