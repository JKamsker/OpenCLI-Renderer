using System.Text.RegularExpressions;

namespace InSpectra.Gen.Tests.Architecture;

/// <summary>
/// Charter rule (docs/architecture/ARCHITECTURE.md, "Intra-module dependency rules"):
/// the capability layers inside <c>InSpectra.Gen</c> flow in one direction:
/// <c>Commands -&gt; UseCases -&gt; Rendering -&gt; OpenCli -&gt; Core</c>.
/// Backward edges form cycles and invert the architecture, so they must not exist.
/// The 4 invariants below codify the cycles that phase f2 (commit c9fc3b6) broke
/// by hand via ad-hoc greps. Keeping them as tests prevents silent regression the
/// next time files move between subtrees.
///
/// The tests scan <c>using</c> directives (not <c>global::</c> qualifiers, not doc
/// comments) — the same shape as <see cref="ArchitectureAppShellTests"/> and
/// <see cref="ArchitectureModeTests"/>.
/// </summary>
public sealed class ArchitectureGenInternalLayeringTests
{
    private static readonly string GenProjectRoot = Path.Combine(
        ArchitecturePolicyScanner.SrcRoot,
        ArchitecturePolicyScanner.AppShellProjectName);

    [Fact]
    public void OpenCli_does_not_depend_on_Rendering()
        => AssertNoUpstreamImport(
            "OpenCli",
            forbiddenPrefixes: new[] { "InSpectra.Gen.Rendering" });

    [Fact]
    public void OpenCli_does_not_depend_on_UseCases_or_Commands()
        => AssertNoUpstreamImport(
            "OpenCli",
            forbiddenPrefixes: new[] { "InSpectra.Gen.UseCases", "InSpectra.Gen.Commands" });

    [Fact]
    public void Rendering_does_not_depend_on_UseCases_or_Commands()
        => AssertNoUpstreamImport(
            "Rendering",
            forbiddenPrefixes: new[] { "InSpectra.Gen.UseCases", "InSpectra.Gen.Commands" });

    [Fact]
    public void UseCases_does_not_depend_on_Commands()
        => AssertNoUpstreamImport(
            "UseCases",
            forbiddenPrefixes: new[] { "InSpectra.Gen.Commands" });

    /// <summary>
    /// Scans every non-generated <c>*.cs</c> file under
    /// <c>src/InSpectra.Gen/&lt;subtree&gt;/</c> for any <c>using</c> directive whose
    /// namespace starts with one of <paramref name="forbiddenPrefixes"/>. A hit is
    /// an upstream import and a charter violation.
    /// </summary>
    private static void AssertNoUpstreamImport(string subtree, IReadOnlyList<string> forbiddenPrefixes)
    {
        var subtreeRoot = Path.Combine(GenProjectRoot, subtree);
        Assert.True(
            Directory.Exists(subtreeRoot),
            $"Expected subtree '{subtree}' at '{subtreeRoot}' to exist.");

        var violations = new List<string>();
        var filesScanned = 0;

        foreach (var filePath in Directory.EnumerateFiles(subtreeRoot, "*.cs", SearchOption.AllDirectories))
        {
            if (IsIgnoredPath(filePath))
            {
                continue;
            }

            filesScanned++;
            var text = File.ReadAllText(filePath);

            foreach (Match match in UsingDirective.Matches(text))
            {
                var ns = match.Groups["ns"].Value;
                foreach (var prefix in forbiddenPrefixes)
                {
                    if (ns.Equals(prefix, StringComparison.Ordinal)
                        || ns.StartsWith(prefix + ".", StringComparison.Ordinal))
                    {
                        violations.Add(
                            $"- {ArchitecturePolicyScanner.GetRelativeRepoPath(filePath)} imports '{ns}'");
                        break;
                    }
                }
            }
        }

        // Guard against a vacuous green. If the directory was empty or got deleted,
        // the test must fail loudly instead of silently passing on zero iterations.
        Assert.True(
            filesScanned > 0,
            $"Expected '{subtreeRoot}' to contain at least one *.cs file but found none.");

        Assert.True(
            violations.Count == 0,
            violations.Count == 0
                ? null
                : $"'{subtree}' must not depend on {string.Join(" / ", forbiddenPrefixes)}, but found:"
                  + Environment.NewLine
                  + string.Join(Environment.NewLine, violations));
    }

    /// <summary>Matches <c>using InSpectra.Gen.&lt;namespace&gt;;</c> on its own line.</summary>
    private static readonly Regex UsingDirective = new(
        @"^\s*using\s+(?<ns>InSpectra\.Gen(?:\.[A-Za-z_][A-Za-z0-9_]*)*)\s*;",
        RegexOptions.Multiline | RegexOptions.Compiled);

    private static bool IsIgnoredPath(string path)
    {
        var segments = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return segments.Any(segment =>
            string.Equals(segment, "bin", StringComparison.OrdinalIgnoreCase)
            || string.Equals(segment, "obj", StringComparison.OrdinalIgnoreCase));
    }
}
