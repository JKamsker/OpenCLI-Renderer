namespace InSpectra.Gen.Acquisition.Promotion.Services;

using InSpectra.Gen.Acquisition.App.Machine;

using InSpectra.Gen.Acquisition.Infrastructure.Host;

using InSpectra.Gen.Acquisition.Indexing;

using InSpectra.Gen.Acquisition.Promotion.State;

using InSpectra.Gen.Acquisition.Infrastructure.Json;

using InSpectra.Gen.Acquisition.Promotion.Results;

using InSpectra.Gen.Acquisition.Promotion.Artifacts;

using InSpectra.Gen.Acquisition.Promotion.Planning;

using InSpectra.Gen.Acquisition.Infrastructure.Paths;

using InSpectra.Discovery.Tool.Analysis;

using System.Text.Json.Nodes;

internal sealed class PromotionApplyCommandService
{
    public async Task<int> ApplyUntrustedAsync(
        string downloadRoot,
        string? summaryOutputPath,
        bool json,
        CancellationToken cancellationToken)
        => (await ApplyCoreAsync(downloadRoot, summaryOutputPath, json, suppressOutput: false, cancellationToken)).ExitCode;

    internal Task<PromotionApplyRunResult> ApplyUntrustedQuietAsync(
        string downloadRoot,
        string? summaryOutputPath,
        CancellationToken cancellationToken)
        => ApplyCoreAsync(downloadRoot, summaryOutputPath, json: false, suppressOutput: true, cancellationToken);

    private async Task<PromotionApplyRunResult> ApplyCoreAsync(
        string downloadRoot,
        string? summaryOutputPath,
        bool json,
        bool suppressOutput,
        CancellationToken cancellationToken)
    {
        var repositoryRoot = RepositoryPathResolver.ResolveRepositoryRoot();
        var packagesRoot = Path.Combine(repositoryRoot, "index", "packages");
        var stateRoot = Path.Combine(repositoryRoot, "state");
        var now = DateTimeOffset.UtcNow;
        var downloadDirectory = Path.GetFullPath(downloadRoot);
        var plan = await PromotionPlanSupport.LoadMergedPlanAsync(downloadDirectory, cancellationToken);
        var resultLookup = await PromotionResultArtifactLookup.BuildAsync(downloadDirectory, cancellationToken);

        var summary = new JsonObject
        {
            ["schemaVersion"] = 1,
            [ResultKey.BatchId] = plan.BatchId,
            ["targetBranch"] = plan.TargetBranch,
            ["promotedAt"] = now.ToString("O"),
            ["expectedCount"] = plan.Items.Count,
            ["successCount"] = 0,
            ["terminalNegativeCount"] = 0,
            ["retryableFailureCount"] = 0,
            ["terminalFailureCount"] = 0,
            ["missingCount"] = 0,
            ["successItems"] = new JsonArray(),
            ["createdPackages"] = new JsonArray(),
            ["updatedPackages"] = new JsonArray(),
            ["nonSuccessItems"] = new JsonArray(),
        };

        foreach (var item in plan.Items.OfType<JsonObject>())
        {
            var hasResultArtifact = resultLookup.TryResolve(item, out var resultEntry);
            var result = hasResultArtifact
                ? resultEntry!.Result
                : PromotionFailureResultSupport.NewSyntheticFailureResult(
                    item,
                    item[ResultKey.Attempt]?.GetValue<int?>() ?? 1,
                    "missing-result-artifact",
                    "No result artifact was uploaded for this matrix item.",
                    plan.BatchId ?? string.Empty,
                    now);
            PromotionPlanItemMergeSupport.MergeIntoResult(item, result);
            var artifactDirectory = hasResultArtifact ? resultEntry!.ArtifactDirectory : null;
            if (!hasResultArtifact)
            {
                summary["missingCount"] = (summary["missingCount"]?.GetValue<int>() ?? 0) + 1;
            }

            if (string.Equals(result[ResultKey.Disposition]?.GetValue<string>(), AnalysisDisposition.Success, StringComparison.Ordinal))
            {
                var validatedSuccess = PromotionSuccessArtifactValidationSupport.Validate(
                    item,
                    result,
                    artifactDirectory,
                    plan.BatchId ?? string.Empty,
                    now);
                result = validatedSuccess.Result;
                artifactDirectory = validatedSuccess.ArtifactDirectory;
            }

            var packageId = item["packageId"]?.GetValue<string>() ?? throw new InvalidOperationException("Plan item is missing packageId.");
            var version = item["version"]?.GetValue<string>() ?? throw new InvalidOperationException($"Plan item '{packageId}' is missing version.");
            var lowerId = packageId.ToLowerInvariant();
            var lowerVersion = version.ToLowerInvariant();
            var statePath = Path.Combine(stateRoot, "packages", lowerId, $"{lowerVersion}.json");
            var existingState = await JsonNodeFileLoader.TryLoadJsonObjectAsync(statePath, cancellationToken);
            var existingPackageIndexPath = Path.Combine(packagesRoot, lowerId, "index.json");
            var existingPackageIndex = await JsonNodeFileLoader.TryLoadJsonObjectAsync(existingPackageIndexPath, cancellationToken);

            JsonObject? indexedPaths = null;
            if (string.Equals(result[ResultKey.Disposition]?.GetValue<string>(), AnalysisDisposition.Success, StringComparison.Ordinal))
            {
                try
                {
                    indexedPaths = await PromotionArtifactWriter.WriteSuccessArtifactsAsync(
                        repositoryRoot,
                        packagesRoot,
                        result,
                        artifactDirectory,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    result = PromotionFailureResultSupport.NewSyntheticFailureResult(
                        item,
                        result[ResultKey.Attempt]?.GetValue<int?>() ?? item[ResultKey.Attempt]?.GetValue<int?>() ?? 1,
                        "invalid-success-artifact",
                        $"Success artifacts could not be promoted: {ex.Message}",
                        plan.BatchId ?? string.Empty,
                        now);
                    PromotionPlanItemMergeSupport.MergeIntoResult(item, result);
                }
            }

            if (!string.Equals(result[ResultKey.Disposition]?.GetValue<string>(), AnalysisDisposition.Success, StringComparison.Ordinal))
            {
                PromotionIndexCleanupSupport.RemoveIndexedVersionArtifacts(packagesRoot, packageId, version);
            }

            var stateRecord = PromotionStateRecordSupport.UpdateStateRecord(existingState, result, indexedPaths, now);
            RepositoryPathResolver.WriteJsonFile(statePath, stateRecord);

            PromotionSummarySupport.IncrementSummaryCount(summary, stateRecord["currentStatus"]?.GetValue<string>());
            PromotionSummarySupport.RecordSuccessItem(summary, existingPackageIndex, result);
            PromotionSummarySupport.UpdatePackageChangeSummary(summary, existingPackageIndex, result);

            if (!string.Equals(stateRecord["currentStatus"]?.GetValue<string>(), AnalysisDisposition.Success, StringComparison.Ordinal))
            {
                ((JsonArray)summary["nonSuccessItems"]!).Add(new JsonObject
                {
                    ["packageId"] = result["packageId"]?.GetValue<string>(),
                    ["version"] = result["version"]?.GetValue<string>(),
                    ["status"] = stateRecord["currentStatus"]?.GetValue<string>(),
                    [ResultKey.Disposition] = result[ResultKey.Disposition]?.GetValue<string>(),
                    ["phase"] = result["phase"]?.GetValue<string>(),
                    [ResultKey.Classification] = result[ResultKey.Classification]?.GetValue<string>(),
                    ["reason"] = PromotionFailureResultSupport.GetNonSuccessReason(result, stateRecord),
                });
            }
        }

        RepositoryPackageIndexBuilder.Rebuild(repositoryRoot, writeBrowserIndex: true);

        if (!string.IsNullOrWhiteSpace(summaryOutputPath))
        {
            RepositoryPathResolver.WriteJsonFile(summaryOutputPath, summary);
        }

        var resolvedSummaryOutputPath = string.IsNullOrWhiteSpace(summaryOutputPath)
            ? null
            : Path.GetFullPath(summaryOutputPath);
        var exitCode = 0;
        if (!suppressOutput)
        {
            var output = Runtime.CreateOutput();
            exitCode = await output.WriteSuccessAsync(
                new
                {
                    batchId = summary[ResultKey.BatchId]?.GetValue<string>(),
                    targetBranch = summary["targetBranch"]?.GetValue<string>(),
                    successCount = summary["successCount"]?.GetValue<int>() ?? 0,
                    terminalNegativeCount = summary["terminalNegativeCount"]?.GetValue<int>() ?? 0,
                    retryableFailureCount = summary["retryableFailureCount"]?.GetValue<int>() ?? 0,
                    terminalFailureCount = summary["terminalFailureCount"]?.GetValue<int>() ?? 0,
                    missingCount = summary["missingCount"]?.GetValue<int>() ?? 0,
                    summaryOutputPath = resolvedSummaryOutputPath,
                },
                [
                    new SummaryRow("Batch", summary[ResultKey.BatchId]?.GetValue<string>() ?? string.Empty),
                    new SummaryRow("Success", (summary["successCount"]?.GetValue<int>() ?? 0).ToString()),
                    new SummaryRow("Terminal negative", (summary["terminalNegativeCount"]?.GetValue<int>() ?? 0).ToString()),
                    new SummaryRow("Retryable failure", (summary["retryableFailureCount"]?.GetValue<int>() ?? 0).ToString()),
                    new SummaryRow("Terminal failure", (summary["terminalFailureCount"]?.GetValue<int>() ?? 0).ToString()),
                    new SummaryRow("Missing artifacts", (summary["missingCount"]?.GetValue<int>() ?? 0).ToString()),
                ],
                json,
                cancellationToken);
        }

        return new PromotionApplyRunResult(exitCode, summary, resolvedSummaryOutputPath);
    }
}

internal sealed record PromotionApplyRunResult(
    int ExitCode,
    JsonObject Summary,
    string? SummaryOutputPath);
