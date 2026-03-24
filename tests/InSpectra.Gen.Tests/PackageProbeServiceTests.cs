using InSpectra.Probe;
using InSpectra.Gen.Tests.TestSupport;

namespace InSpectra.Gen.Tests;

public sealed class PackageProbeServiceTests
{
    private readonly PackageProbeService service = new();

    [Fact]
    public void Analyze_rejects_non_tool_packages()
    {
        var result = service.Analyze(ToolPackageBuilder.CreatePlainPackage());

        Assert.Equal("unsupported", result.Status);
        Assert.Equal("unsupported", result.Confidence);
        Assert.Equal("The package is not marked as DotnetTool.", result.Error);
        Assert.NotNull(result.Package);
        Assert.False(result.Package!.IsDotnetTool);
    }

    [Fact]
    public void Analyze_prefers_packaged_opencli_snapshot()
    {
        var json = File.ReadAllText(FixturePaths.OpenCliJson);

        var result = service.Analyze(ToolPackageBuilder.CreatePackagedOpenCliTool(json));

        Assert.Equal("supported", result.Status);
        Assert.Equal("high", result.Confidence);
        Assert.NotNull(result.Document);
        Assert.Equal("jdr", result.Document!.Info.Title);
        Assert.True(result.Package!.HasPackagedOpenCli);
    }

    [Fact]
    public void Analyze_recovers_inspectra_command_graph()
    {
        var result = service.Analyze(ToolPackageBuilder.CreateInspectraPackage());

        Assert.Equal("supported", result.Status);
        Assert.NotNull(result.Document);

        var render = Assert.Single(result.Document!.Commands, command => command.Name == "render");
        Assert.Equal("Render documentation from OpenCLI exports.", render.Description);

        var file = Assert.Single(render.Commands, command => command.Name == "file");
        Assert.Equal("Render docs from saved OpenCLI export files.", file.Description);
        Assert.Contains(file.Commands, command => command.Name == "markdown");
        Assert.Contains(file.Commands, command => command.Name == "html");

        var exec = Assert.Single(render.Commands, command => command.Name == "exec");
        Assert.Equal("Render docs by executing a CLI that exposes `cli opencli`.", exec.Description);
        Assert.Contains(exec.Commands, command => command.Name == "markdown");
        Assert.Contains(exec.Commands, command => command.Name == "html");

        Assert.Equal("static-spectre", result.Document.Metadata.Single(item => item.Name == "ProbeMode").Value);
        Assert.True(result.Package!.IsSpectreCli);
    }
}
