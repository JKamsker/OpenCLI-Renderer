using System.Net.Http;
using InSpectra.Probe;

namespace InSpectra.Gen.Tests;

public sealed class LivePackageProbeTests
{
    private const string EnabledVariable = "INSPECTRA_RUN_LIVE_NUGET_TESTS";
    private const string PackageId = "InSpectra.Gen";
    private const string PackageVersion = "0.0.30";

    private readonly PackageProbeService service = new();

    [Fact]
    [Trait("Category", "Live")]
    public async Task Analyze_verifies_a_published_nuget_tool_when_enabled()
    {
        if (!IsEnabled())
        {
            return;
        }

        using var client = new HttpClient();
        using var response = await client.GetAsync(BuildPackageUrl());
        response.EnsureSuccessStatusCode();

        var result = service.Analyze(await response.Content.ReadAsByteArrayAsync());

        Assert.Equal("supported", result.Status);
        Assert.NotNull(result.Document);
        Assert.NotNull(result.Package);
        Assert.True(result.Package!.IsDotnetTool);
        Assert.True(result.Package.IsSpectreCli);
        Assert.Equal("static-spectre", result.Package.DocumentSource);

        var render = Assert.Single(result.Document!.Commands, command => command.Name == "render");
        Assert.Contains(render.Commands, command => command.Name == "file");
        Assert.Contains(render.Commands, command => command.Name == "exec");
    }

    private static bool IsEnabled()
    {
        return string.Equals(Environment.GetEnvironmentVariable(EnabledVariable), "1", StringComparison.Ordinal);
    }

    private static string BuildPackageUrl()
    {
        var normalizedId = PackageId.ToLowerInvariant();
        var normalizedVersion = PackageVersion.ToLowerInvariant();
        return $"https://api.nuget.org/v3-flatcontainer/{normalizedId}/{normalizedVersion}/{normalizedId}.{normalizedVersion}.nupkg";
    }
}
