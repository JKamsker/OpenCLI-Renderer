using System.Text.Json.Nodes;

namespace InSpectra.Gen.Acquisition.Infrastructure.Commands;

internal sealed record InstalledToolAnalysisRequest(
    JsonObject Result,
    string Version,
    string CommandName,
    string OutputDirectory,
    InstalledToolContext InstalledTool,
    string WorkingDirectory,
    int CommandTimeoutSeconds);
