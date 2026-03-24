using System.Diagnostics;
using System.Text.RegularExpressions;
using OpenCli.Renderer.Tests.TestSupport;

namespace OpenCli.Renderer.Tests;

public class CliHelpSurfaceTests
{
    [Theory]
    [InlineData("file")]
    [InlineData("exec")]
    public async Task Html_help_only_exposes_bundle_output_option(string mode)
    {
        var result = await RunRendererAsync(["render", mode, "html", "--help"]);
        var helpText = NormalizeHelpOutput(result.StandardOutput);

        Assert.Equal(0, result.ExitCode);
        AssertOption(helpText, "--out-dir", "<DIR>");
        AssertNoOption(helpText, "--out", "<FILE>");
        AssertNoOption(helpText, "--layout", "<LAYOUT>");
    }

    [Theory]
    [InlineData("file")]
    [InlineData("exec")]
    public async Task Markdown_help_keeps_single_and_tree_output_options(string mode)
    {
        var result = await RunRendererAsync(["render", mode, "markdown", "--help"]);
        var helpText = NormalizeHelpOutput(result.StandardOutput);

        Assert.Equal(0, result.ExitCode);
        AssertOption(helpText, "--layout", "<LAYOUT>");
        AssertOption(helpText, "--out", "<FILE>");
        AssertOption(helpText, "--out-dir", "<DIR>");
    }

    private static async Task<ProcessResult> RunRendererAsync(IReadOnlyList<string> arguments)
    {
        var dllPath = Path.Combine(
            FixturePaths.RepoRoot,
            "src",
            "OpenCli.Renderer",
            "bin",
            "Release",
            "net10.0",
            "opencli-renderer.dll");

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = FixturePaths.RepoRoot,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        startInfo.ArgumentList.Add(dllPath);
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        startInfo.Environment["NO_COLOR"] = "1";

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var stdoutTask = process.StandardOutput.ReadToEndAsync(timeout.Token);
        var stderrTask = process.StandardError.ReadToEndAsync(timeout.Token);

        await process.WaitForExitAsync(timeout.Token);

        return new ProcessResult(
            process.ExitCode,
            await stdoutTask,
            await stderrTask);
    }

    private sealed record ProcessResult(int ExitCode, string StandardOutput, string StandardError);

    private static string NormalizeHelpOutput(string helpText)
    {
        return Regex.Replace(
            helpText,
            @"\x1B\[[0-9;?]*[ -/]*[@-~]",
            string.Empty);
    }

    private static void AssertOption(string helpText, string optionName, string argumentName)
    {
        Assert.Matches(BuildOptionPattern(optionName, argumentName), helpText);
    }

    private static void AssertNoOption(string helpText, string optionName, string argumentName)
    {
        Assert.DoesNotMatch(BuildOptionPattern(optionName, argumentName), helpText);
    }

    private static Regex BuildOptionPattern(string optionName, string argumentName)
    {
        return new Regex(
            $@"(?<!\S){Regex.Escape(optionName)}\s+{Regex.Escape(argumentName)}(?!\S)",
            RegexOptions.CultureInvariant);
    }
}
