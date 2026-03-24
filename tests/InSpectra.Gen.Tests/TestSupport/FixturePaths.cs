namespace InSpectra.Gen.Tests.TestSupport;

internal static class FixturePaths
{
    public static string RepoRoot { get; } = FindRepoRoot();

    public static string OpenCliJson => Path.Combine(RepoRoot, "assets", "testfiles", "opencli.json");

    public static string XmlDoc => Path.Combine(RepoRoot, "assets", "testfiles", "xmldoc.xml");

    private static string FindRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "InSpectra.Gen.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Repository root could not be located.");
    }
}
