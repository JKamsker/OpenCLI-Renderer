namespace InSpectra.Gen.Acquisition.Docs.Commands;

using InSpectra.Gen.Acquisition.OpenCli.Artifacts;

internal sealed class DocsReconcileStoredOpenCliCommand : DocsArtifactRegenerationCommandBase
{
    private readonly StoredOpenCliArtifactReconciler _reconciler = new();

    protected override string ArtifactLabel => "Stored OpenCLI artifacts";

    protected override ArtifactRegenerationRunResult Regenerate(string repositoryRoot, ArtifactRegenerationScope scope, bool rebuildIndexes)
    {
        var result = _reconciler.RegenerateRepository(repositoryRoot, scope, rebuildIndexes);
        return new ArtifactRegenerationRunResult(
            result.ScannedCount,
            result.CandidateCount,
            result.RewrittenCount,
            result.UnchangedCount,
            result.FailedCount,
            result.RewrittenItems,
            result.FailedItems);
    }
}
