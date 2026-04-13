using System.Text.Json;
using System.Text.Json.Nodes;

using InSpectra.Lib.Execution.Process;
using InSpectra.Lib.OpenCli.Validation;
using InSpectra.Lib.Tooling.Paths;
using InSpectra.Lib.Tooling.Process;
using InSpectra.Lib.Tooling.Results;

namespace InSpectra.Lib.Modes.Native.Execution;

/// <summary>
/// Runs a Spectre.Console.Cli native introspection command (<c>cli opencli</c>) on an
/// already-installed tool and writes the resulting OpenCLI artifact.
/// </summary>
internal sealed class NativeInstalledToolAnalysisSupport(IProcessRunner processRunner)
{
    private static readonly string[] OpenCliArguments = ["cli", "opencli"];

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
    };

    internal async Task AnalyzeInstalledAsync(
        InstalledToolAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        var result = request.Result;
        var tool = request.InstalledTool;

        result["phase"] = "opencli";

        ProcessResult processResult;
        try
        {
            processResult = await processRunner.RunAsync(
                tool.CommandPath,
                request.WorkingDirectory,
                OpenCliArguments,
                request.CommandTimeoutSeconds,
                tool.Environment,
                tool.CleanupRoot,
                cancellationToken);
        }
        catch (Exception ex)
        {
            NonSpectreResultSupport.ApplyTerminalFailure(
                result,
                phase: "opencli",
                classification: "native-execution-failed",
                ex.Message);
            return;
        }

        string sanitizedJson;
        try
        {
            sanitizedJson = OpenCliJsonSanitizer.Sanitize(processResult.StandardOutput);
        }
        catch (JsonException ex)
        {
            NonSpectreResultSupport.ApplyTerminalFailure(
                result,
                phase: "opencli",
                classification: "native-invalid-json",
                $"Native introspection output is not valid OpenCLI JSON: {ex.Message}");
            return;
        }

        // Hoist __default_command nodes and clean assembly-filename titles.
        try
        {
            var document = JsonNode.Parse(sanitizedJson)?.AsObject();
            if (document is not null)
            {
                OpenCliStructuralSanitizer.Sanitize(document);
                sanitizedJson = JsonSerializer.Serialize(document, SerializerOptions);
            }
        }
        catch (JsonException)
        {
            // Structural sanitization is best-effort; keep the original if it fails.
        }

        var openCliPath = Path.Combine(request.OutputDirectory, "opencli.json");
        await File.WriteAllTextAsync(openCliPath, sanitizedJson, cancellationToken);
        result["artifacts"]!.AsObject()["opencliArtifact"] = "opencli.json";
        NonSpectreResultSupport.ApplySuccess(result, "native-opencli", "native-introspection");
    }
}
