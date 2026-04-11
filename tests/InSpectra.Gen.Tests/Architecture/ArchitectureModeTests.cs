using System.Text.RegularExpressions;

namespace InSpectra.Gen.Tests.Architecture;

/// <summary>
/// Charter rules for acquisition modes (docs/architecture/ARCHITECTURE.md,
/// "Intra-module dependency rules" and "Placement rules"):
///
/// <list type="bullet">
///   <item>"One mode must not depend on another mode."</item>
///   <item>"In this repo, mode-specific conversion into OpenCLI lives under that mode as
///         <c>Projection/</c>, not as another <c>OpenCli</c> root."</item>
/// </list>
/// </summary>
public sealed class ArchitectureModeTests
{
    /// <summary>Absolute path to <c>src/InSpectra.Gen.Acquisition/Modes</c>.</summary>
    private static readonly string ModesRoot = Path.Combine(
        ArchitecturePolicyScanner.SrcRoot,
        ArchitecturePolicyScanner.AcquisitionProjectName,
        "Modes");

    /// <summary>
    /// Matches <c>using InSpectra.Gen.Acquisition.Modes.&lt;ModeName&gt;</c> at any depth.
    /// Captures the first segment after <c>Modes.</c> so the test can identify the target mode.
    /// </summary>
    private static readonly Regex ModeUsingDirective = new(
        @"^\s*using\s+InSpectra\.Gen\.Acquisition\.Modes\.(?<mode>[A-Za-z_][A-Za-z0-9_]*)",
        RegexOptions.Multiline | RegexOptions.Compiled);

    /// <summary>
    /// Positive anchor for the modes scan: at least one file in each mode subtree should
    /// declare a matching <c>InSpectra.Gen.Acquisition.Modes.&lt;Mode&gt;...</c> namespace.
    /// Without that anchor, the test could scan the wrong surface and still pass.
    /// </summary>
    private static readonly Regex ModeNamespaceDeclaration = new(
        @"^\s*namespace\s+InSpectra\.Gen\.Acquisition\.Modes\.(?<mode>[A-Za-z_][A-Za-z0-9_]*)(?:\.[A-Za-z_][A-Za-z0-9_]*)*\s*[;{]",
        RegexOptions.Multiline | RegexOptions.Compiled);

    [Fact]
    public void No_cross_mode_dependencies()
    {
        Assert.True(Directory.Exists(ModesRoot), $"Expected Modes root at '{ModesRoot}' to exist.");
        var modeDirectories = Directory.EnumerateDirectories(ModesRoot).OrderBy(path => path, StringComparer.Ordinal).ToArray();
        Assert.NotEmpty(modeDirectories);

        var violations = new List<string>();
        var filesScannedByMode = new Dictionary<string, int>(StringComparer.Ordinal);
        var matchingNamespacesByMode = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (var modeDirectory in modeDirectories)
        {
            var modeName = Path.GetFileName(modeDirectory);
            filesScannedByMode[modeName] = 0;
            matchingNamespacesByMode[modeName] = 0;

            foreach (var filePath in Directory.EnumerateFiles(modeDirectory, "*.cs", SearchOption.AllDirectories))
            {
                if (IsIgnoredPath(filePath))
                {
                    continue;
                }

                filesScannedByMode[modeName]++;
                var text = File.ReadAllText(filePath);
                foreach (Match namespaceMatch in ModeNamespaceDeclaration.Matches(text))
                {
                    if (string.Equals(namespaceMatch.Groups["mode"].Value, modeName, StringComparison.Ordinal))
                    {
                        matchingNamespacesByMode[modeName]++;
                    }
                }

                foreach (Match match in ModeUsingDirective.Matches(text))
                {
                    var referencedMode = match.Groups["mode"].Value;
                    if (!string.Equals(referencedMode, modeName, StringComparison.Ordinal))
                    {
                        violations.Add(
                            $"- {ArchitecturePolicyScanner.GetRelativeRepoPath(filePath)} (mode '{modeName}')"
                            + $" references mode '{referencedMode}'");
                    }
                }
            }
        }

        var modesWithoutFiles = filesScannedByMode
            .Where(entry => entry.Value == 0)
            .Select(entry => entry.Key)
            .ToArray();
        Assert.True(
            modesWithoutFiles.Length == 0,
            modesWithoutFiles.Length == 0
                ? null
                : "Expected every mode subtree to contain at least one tracked .cs file, but found none for: "
                  + string.Join(", ", modesWithoutFiles));

        var modesWithoutMatchingNamespace = matchingNamespacesByMode
            .Where(entry => entry.Value == 0)
            .Select(entry => entry.Key)
            .ToArray();
        Assert.True(
            modesWithoutMatchingNamespace.Length == 0,
            modesWithoutMatchingNamespace.Length == 0
                ? null
                : "Expected every mode subtree to encounter at least one matching namespace declaration, but found none for: "
                  + string.Join(", ", modesWithoutMatchingNamespace));

        Assert.True(
            violations.Count == 0,
            violations.Count == 0
                ? null
                : "Modes must not depend on other modes, but found:"
                  + Environment.NewLine
                  + string.Join(Environment.NewLine, violations));
    }

    [Fact]
    public void Mode_specific_OpenCli_folders_are_renamed_to_Projection()
    {
        Assert.True(Directory.Exists(ModesRoot), $"Expected Modes root at '{ModesRoot}' to exist.");
        var modeDirectories = Directory.EnumerateDirectories(ModesRoot).OrderBy(path => path, StringComparer.Ordinal).ToArray();
        Assert.NotEmpty(modeDirectories);

        var violations = new List<string>();
        var modesWithoutProjection = modeDirectories
            .Where(modeDirectory => !Directory.Exists(Path.Combine(modeDirectory, "Projection")))
            .Select(Path.GetFileName)
            .ToArray();

        foreach (var modeDirectory in modeDirectories)
        {
            foreach (var child in Directory.EnumerateDirectories(modeDirectory, "*", SearchOption.AllDirectories))
            {
                if (IsIgnoredPath(child))
                {
                    continue;
                }

                var directoryName = Path.GetFileName(child);
                if (string.Equals(directoryName, "OpenCli", StringComparison.Ordinal))
                {
                    violations.Add($"- {ArchitecturePolicyScanner.GetRelativeRepoPath(child)}");
                }
            }
        }

        Assert.True(
            modesWithoutProjection.Length == 0,
            modesWithoutProjection.Length == 0
                ? null
                : "Expected every mode subtree to contain at least one Projection directory, but found none for: "
                  + string.Join(", ", modesWithoutProjection));

        Assert.True(
            violations.Count == 0,
            violations.Count == 0
                ? null
                : "Mode-specific OpenCli folders must be renamed to Projection, but found:"
                  + Environment.NewLine
                  + string.Join(Environment.NewLine, violations));
    }

    private static bool IsIgnoredPath(string path)
    {
        var segments = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return segments.Any(segment =>
            string.Equals(segment, "bin", StringComparison.OrdinalIgnoreCase)
            || string.Equals(segment, "obj", StringComparison.OrdinalIgnoreCase));
    }
}
