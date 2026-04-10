namespace InSpectra.Gen.Acquisition.Docs.Commands;

using InSpectra.Gen.Acquisition.StaticAnalysis.Artifacts;

using InSpectra.Gen.Acquisition.OpenCli.Artifacts;

internal sealed class DocsRegenerateStaticCrawlsCommand : DocsArtifactRegenerationCommandBase
{
    private readonly StaticAnalysisCrawlArtifactRegenerator _regenerator = new();

    protected override string ArtifactLabel => "Static-analysis artifacts";

    protected override ArtifactRegenerationRunResult Regenerate(string repositoryRoot, ArtifactRegenerationScope scope, bool rebuildIndexes)
    {
        var result = _regenerator.RegenerateRepository(repositoryRoot, scope, rebuildIndexes);
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

