namespace InSpectra.Probe.Models;

internal sealed class ProbePackageContext
{
    public required string Id { get; init; }

    public required string Version { get; init; }

    public bool IsDotnetTool { get; init; }

    public string? Description { get; init; }

    public string? CommandName { get; init; }

    public string? Runner { get; init; }

    public string? EntryPoint { get; init; }

    public string? TargetFramework { get; init; }

    public byte[]? EntryAssemblyBytes { get; init; }

    public bool HasPackagedOpenCli { get; init; }

    public OpenCliDocument? PackagedOpenCli { get; init; }
}
