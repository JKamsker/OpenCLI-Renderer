namespace InSpectra.Lib.Tests.Live;

using InSpectra.Lib.Contracts;

using Xunit;

[Collection("LiveToolAnalysis")]
public sealed class AutoHookFallbackLiveTests
{
    public static TheoryData<HookFallbackToolCase> Cases()
    {
        var data = new TheoryData<HookFallbackToolCase>();
        data.Add(new HookFallbackToolCase(
            "System.CommandLine + Argu",
            "csharp-ls",
            "0.22.0",
            "csharp-ls",
            "csharp-ls",
            "0.22.0",
            "hook-no-assembly-loaded"));
        data.Add(new HookFallbackToolCase(
            "System.CommandLine",
            "DotNetAnalyzer",
            "1.5.0",
            "dotnet-analyzer",
            "DotNetAnalyzer - .NET MCP Server for Claude Code",
            "1.5.0",
            "hook-no-assembly-loaded",
            expectedAnalysisMode: "help",
            expectedArtifactSource: "crawled-from-help"));
        data.Add(new HookFallbackToolCase(
            "System.CommandLine",
            "SoftwareExtravaganza.Whizbang.CLI",
            "0.54.2-alpha.76",
            "whizbang",
            "Whizbang CLI - Command-line tool for Whizbang",
            "0.54.2-alpha.76",
            "hook-no-assembly-loaded",
            expectedAnalysisMode: "help",
            expectedArtifactSource: "crawled-from-help"));
        data.Add(new HookFallbackToolCase(
            "System.CommandLine",
            "SaigonMio.Generata",
            "1.1.36",
            "generata",
            "Mio.Generata",
            "1.1.36",
            "hook-no-patchable-method",
            expectedAnalysisMode: "help",
            expectedArtifactSource: "crawled-from-help"));
        data.Add(new HookFallbackToolCase(
            "McMaster.Extensions.CommandLineUtils",
            "sqlite-global-tool",
            "1.2.2",
            "sqlite-tool",
            "=> Welcome to sqlite .net core global tool version",
            "1.2.2",
            "hook-no-assembly-loaded",
            expectedAnalysisMode: "help",
            expectedArtifactSource: "crawled-from-help",
            requireHookFailure: false));
        return data;
    }

    public static TheoryData<HookTerminalFailureToolCase> TerminalFailureCases()
    {
        var data = new TheoryData<HookTerminalFailureToolCase>();
        data.Add(new HookTerminalFailureToolCase(
            "McMaster.Extensions.CommandLineUtils",
            "Meadow.Cli",
            "0.3.225",
            "meadow",
            "help",
            "help-crawl-empty",
            "invalid-opencli-artifact",
            "custom-parser-no-attributes"));
        return data;
    }

    [Theory]
    [MemberData(nameof(Cases))]
    [Trait("Category", "Live")]
    public async Task RunAsync_Falls_Back_To_Expected_Mode_For_Real_World_Hook_Regressions(HookFallbackToolCase testCase)
    {
        if (!HookLiveTestSupport.ShouldRun())
        {
            return;
        }

        var report = await AutoAcquisitionLiveTestSupport.RunAsync(
            testCase.PackageId,
            testCase.Version,
            testCase.CommandName,
            timeoutSeconds: 300,
            CancellationToken.None);

        Assert.True(report.Success, BuildFailureMessage(testCase.PackageId, testCase.Version, report));
        Assert.Equal("static", report.Descriptor.PreferredAnalysisMode);
        Assert.Equal(testCase.Framework, report.Descriptor.CliFramework);
        Assert.True(
            string.Equals(testCase.ExpectedAnalysisMode, report.SelectedMode, StringComparison.Ordinal),
            BuildModeMismatchMessage(testCase, report));
        Assert.Equal(testCase.ExpectedOpenCliTitle, report.OpenCliDocument?["info"]?["title"]?.GetValue<string>());
        Assert.Equal(testCase.ExpectedOpenCliVersion, report.OpenCliDocument?["info"]?["version"]?.GetValue<string>());
        if (testCase.ExpectedArtifactSource is not null)
        {
            Assert.Equal(testCase.ExpectedArtifactSource, report.OpenCliDocument?["x-inspectra"]?["artifactSource"]?.GetValue<string>());
        }

        if (testCase.RequireHookFailure)
        {
            AssertContainsFailureClassification(
                report.Attempts,
                mode: AnalysisMode.Hook,
                testCase.ExpectedHookFailureClassifications);
        }

        if (testCase.AssertSnapshot)
        {
            HookOpenCliSnapshotSupport.AssertMatchesFixture(testCase.PackageId, testCase.Version, report.OpenCliDocument);
        }
    }

    [Theory]
    [MemberData(nameof(TerminalFailureCases))]
    [Trait("Category", "Live")]
    public async Task RunAsync_Reports_Honest_Terminal_Failures_When_No_Mode_Can_Produce_OpenCli(HookTerminalFailureToolCase testCase)
    {
        if (!HookLiveTestSupport.ShouldRun())
        {
            return;
        }

        var report = await AutoAcquisitionLiveTestSupport.RunAsync(
            testCase.PackageId,
            testCase.Version,
            testCase.CommandName,
            timeoutSeconds: 300,
            CancellationToken.None);

        Assert.False(report.Success, $"Expected terminal failure for {testCase.PackageId} {testCase.Version}.");
        Assert.Equal("static", report.Descriptor.PreferredAnalysisMode);
        Assert.Equal(testCase.Framework, report.Descriptor.CliFramework);
        AssertContainsFailureClassification(report.Attempts, testCase.ExpectedFinalMode, [testCase.ExpectedFinalClassification]);
        AssertContainsFailureClassification(report.Attempts, AnalysisMode.Hook, [testCase.ExpectedHookFailureClassification]);
        AssertContainsFailureClassification(report.Attempts, AnalysisMode.Static, [testCase.ExpectedStaticFailureClassification]);
    }

    private static void AssertContainsFailureClassification(
        IEnumerable<AutoAcquisitionAttemptReport> attempts,
        string mode,
        IReadOnlyList<string> expectedClassifications)
    {
        var candidates = attempts.Where(attempt =>
            string.Equals(attempt.Mode, mode, StringComparison.Ordinal)
            && string.Equals(attempt.Outcome, AnalysisDisposition.Failed, StringComparison.Ordinal)).ToArray();
        Assert.True(
            candidates.Length > 0,
            $"Expected at least one failed {mode} attempt. Attempts:{Environment.NewLine}{string.Join(Environment.NewLine, attempts.Select(FormatAttempt))}");

        foreach (var candidate in candidates)
        {
            if (expectedClassifications.Any(expected => Matches(candidate, expected)))
            {
                return;
            }
        }

        Assert.Fail(
            $"None of the {mode} failures matched the expected classifications: {string.Join(", ", expectedClassifications)}."
            + Environment.NewLine
            + string.Join(Environment.NewLine, candidates.Select(FormatAttempt)));
    }

    private static bool Matches(AutoAcquisitionAttemptReport attempt, string expectedClassification)
        => string.Equals(attempt.Classification, expectedClassification, StringComparison.Ordinal)
            || (!string.IsNullOrWhiteSpace(attempt.Message)
                && attempt.Message.Contains(expectedClassification, StringComparison.Ordinal));

    private static string BuildModeMismatchMessage(HookFallbackToolCase testCase, AutoAcquisitionReport report)
        => BuildFailureMessage(testCase.PackageId, testCase.Version, report)
            + Environment.NewLine
            + $"Expected selected mode '{testCase.ExpectedAnalysisMode}' but got '{report.SelectedMode ?? "<none>"}'.";

    private static string BuildFailureMessage(string packageId, string version, AutoAcquisitionReport report)
    {
        var lines = new List<string>
        {
            $"Auto acquisition did not match the expected discovery contract for {packageId} {version}.",
            $"Preferred mode: {report.Descriptor.PreferredAnalysisMode} ({report.Descriptor.SelectionReason})",
        };

        lines.AddRange(report.Attempts.Select(FormatAttempt));
        return string.Join(Environment.NewLine, lines);
    }

    private static string FormatAttempt(AutoAcquisitionAttemptReport attempt)
        => $"{attempt.Mode} [{attempt.Framework ?? "<none>"}]: {attempt.Outcome}"
            + FormatTail(attempt.Classification)
            + FormatTail(attempt.ArtifactSource)
            + FormatTail(attempt.Message);

    private static string FormatTail(string? value)
        => string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : $" | {value}";

    public sealed record HookFallbackToolCase(
        string Framework,
        string PackageId,
        string Version,
        string CommandName,
        string ExpectedOpenCliTitle,
        string ExpectedOpenCliVersion,
        string expectedHookFailureClassification,
        string expectedAnalysisMode = "static",
        string? expectedArtifactSource = "static-analysis",
        bool assertSnapshot = true,
        bool requireHookFailure = true,
        params string[] expectedHookFailureClassifications)
    {
        public IReadOnlyList<string> ExpectedHookFailureClassifications { get; } = expectedHookFailureClassifications.Length == 0
            ? [expectedHookFailureClassification]
            : expectedHookFailureClassifications;

        public string ExpectedAnalysisMode { get; } = expectedAnalysisMode;
        public string? ExpectedArtifactSource { get; } = expectedArtifactSource;
        public bool AssertSnapshot { get; } = assertSnapshot;
        public bool RequireHookFailure { get; } = requireHookFailure;

        public override string ToString()
            => $"{PackageId} {Version}";
    }

    public sealed record HookTerminalFailureToolCase(
        string Framework,
        string PackageId,
        string Version,
        string CommandName,
        string ExpectedFinalMode,
        string ExpectedFinalClassification,
        string ExpectedHookFailureClassification,
        string ExpectedStaticFailureClassification)
    {
        public override string ToString()
            => $"{PackageId} {Version}";
    }
}
