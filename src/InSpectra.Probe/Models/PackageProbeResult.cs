namespace InSpectra.Probe.Models;

public sealed class PackageProbeResult
{
    public string Status { get; set; } = "unsupported";

    public string Confidence { get; set; } = "unsupported";

    public string? Error { get; set; }

    public List<string> Warnings { get; set; } = [];

    public PackageDescriptor? Package { get; set; }

    public OpenCliDocument? Document { get; set; }
}

public sealed class PackageDescriptor
{
    public string Id { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public bool IsDotnetTool { get; set; }

    public bool IsSpectreCli { get; set; }

    public string? CommandName { get; set; }

    public string? Runner { get; set; }

    public string? EntryPoint { get; set; }

    public string? TargetFramework { get; set; }

    public bool HasPackagedOpenCli { get; set; }
}
