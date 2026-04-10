namespace InSpectra.Gen.Acquisition.Promotion.State;

using InSpectra.Discovery.Tool.Analysis;

using System.Text.Json.Nodes;

internal static class PromotionStateRecordSupport
{
    public static JsonObject UpdateStateRecord(JsonObject? existingState, JsonObject result, JsonObject? indexedPaths, DateTimeOffset now)
    {
        var sameSignature =
            !string.IsNullOrWhiteSpace(existingState?["lastFailureSignature"]?.GetValue<string>()) &&
            string.Equals(existingState?["lastFailureSignature"]?.GetValue<string>(), result["failureSignature"]?.GetValue<string>(), StringComparison.Ordinal);
        var consecutiveFailures = string.Equals(result[ResultKey.Disposition]?.GetValue<string>(), AnalysisDisposition.RetryableFailure, StringComparison.Ordinal)
            ? sameSignature
                ? (existingState?["consecutiveFailureCount"]?.GetValue<int?>() ?? 0) + 1
                : 1
            : 0;
        var attemptCount = result[ResultKey.Attempt]?.GetValue<int?>() ?? 1;
        var allowTerminalEscalation =
            !string.Equals(result[ResultKey.Disposition]?.GetValue<string>(), AnalysisDisposition.RetryableFailure, StringComparison.Ordinal) ||
            !string.Equals(result[ResultKey.Classification]?.GetValue<string>(), "environment-missing-runtime", StringComparison.Ordinal);

        var status = result[ResultKey.Disposition]?.GetValue<string>() switch
        {
            AnalysisDisposition.Success => AnalysisDisposition.Success,
            AnalysisDisposition.TerminalNegative => AnalysisDisposition.TerminalNegative,
            AnalysisDisposition.TerminalFailure => AnalysisDisposition.TerminalFailure,
            _ => allowTerminalEscalation && consecutiveFailures >= 3 ? AnalysisDisposition.TerminalFailure : AnalysisDisposition.RetryableFailure,
        };

        if (CanPreserveExistingSuccessState(existingState, status, indexedPaths))
        {
            return existingState!.DeepClone().AsObject();
        }

        return new JsonObject
        {
            ["schemaVersion"] = 1,
            ["packageId"] = result["packageId"]?.GetValue<string>(),
            ["version"] = result["version"]?.GetValue<string>(),
            ["trusted"] = false,
            ["currentStatus"] = status,
            ["lastDisposition"] = result[ResultKey.Disposition]?.GetValue<string>(),
            ["attemptCount"] = attemptCount,
            ["consecutiveFailureCount"] = consecutiveFailures,
            ["lastFailureSignature"] = status.Contains("failure", StringComparison.Ordinal) ? result["failureSignature"]?.GetValue<string>() : null,
            ["lastFailurePhase"] = status.Contains("failure", StringComparison.Ordinal) ? result["phase"]?.GetValue<string>() : null,
            ["lastFailureMessage"] = status.Contains("failure", StringComparison.Ordinal) ? result[ResultKey.FailureMessage]?.GetValue<string>() : null,
            ["firstEvaluatedAt"] = existingState?["firstEvaluatedAt"]?.GetValue<string>() ?? result[ResultKey.AnalyzedAt]?.GetValue<string>(),
            ["lastEvaluatedAt"] = result[ResultKey.AnalyzedAt]?.GetValue<string>(),
            ["lastBatchId"] = result[ResultKey.BatchId]?.GetValue<string>(),
            ["retryEligible"] = status == AnalysisDisposition.RetryableFailure,
            ["nextAttemptAt"] = status == AnalysisDisposition.RetryableFailure ? now.AddHours(GetBackoffHours(attemptCount)).ToString("O") : null,
            ["lastSuccessfulAt"] = status == AnalysisDisposition.Success
                ? result[ResultKey.AnalyzedAt]?.GetValue<string>()
                : existingState?["lastSuccessfulAt"]?.GetValue<string>(),
            ["indexedPaths"] = indexedPaths?.DeepClone(),
        };
    }

    private static bool CanPreserveExistingSuccessState(JsonObject? existingState, string status, JsonObject? indexedPaths)
        => existingState is not null
           && string.Equals(status, AnalysisDisposition.Success, StringComparison.Ordinal)
           && string.Equals(existingState["currentStatus"]?.GetValue<string>(), AnalysisDisposition.Success, StringComparison.Ordinal)
           && string.Equals(existingState["lastDisposition"]?.GetValue<string>(), AnalysisDisposition.Success, StringComparison.Ordinal)
           && JsonNode.DeepEquals(existingState["indexedPaths"], indexedPaths);

    private static int GetBackoffHours(int attempt)
        => attempt switch
        {
            <= 1 => 1,
            2 => 6,
            _ => 24,
        };
}
