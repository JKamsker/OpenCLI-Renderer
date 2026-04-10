using InSpectra.Gen.Acquisition.Runtime;
using InSpectra.Gen.Runtime;

namespace InSpectra.Gen.Services;

internal static class OpenCliArtifactWriter
{
    public static OpenCliArtifactOptions WriteArtifacts(
        OpenCliArtifactOptions requested,
        string openCliJson,
        string? crawlJson)
    {
        var openCliPath = WriteArtifact(requested.OpenCliOutputPath, openCliJson);
        var crawlPath = WriteArtifact(requested.CrawlOutputPath, crawlJson);
        return new OpenCliArtifactOptions(openCliPath, crawlPath);
    }

    public static void EnsureRequestedArtifactsAvailable(
        OpenCliArtifactOptions requested,
        string? crawlJson)
    {
        if (!string.IsNullOrWhiteSpace(requested.CrawlOutputPath) && string.IsNullOrWhiteSpace(crawlJson))
        {
            throw new CliUsageException(
                "`--crawl-out` is only available for crawl-backed acquisition modes.");
        }
    }

    private static string? WriteArtifact(string? path, string? content)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        if (content is null)
        {
            return null;
        }

        var fullPath = Path.GetFullPath(path);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(fullPath, content);
        return fullPath;
    }
}
