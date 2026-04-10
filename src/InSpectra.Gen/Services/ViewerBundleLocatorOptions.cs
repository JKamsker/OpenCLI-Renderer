namespace InSpectra.Gen.Services;

public sealed class ViewerBundleLocatorOptions
{
    public string? PackagedRootPath { get; init; }

    public string? RepositoryRootPath { get; init; }

    public string? NpmExecutablePath { get; init; }

    public int NpmTimeoutSeconds { get; init; } = 300;
}
