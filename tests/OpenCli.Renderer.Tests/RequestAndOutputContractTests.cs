using System.Text.Json.Nodes;
using OpenCli.Renderer.Runtime;

namespace OpenCli.Renderer.Tests;

public class RequestAndOutputContractTests
{
    [Fact]
    public void Json_single_output_requires_out_file()
    {
        var settings = new TestMarkdownSettings
        {
            Json = true,
        };

        var exception = Assert.Throws<CliUsageException>(() =>
            RenderRequestFactory.CreateOptions(settings, "single", null, null, timeoutSeconds: null, hasTimeoutSupport: false));

        Assert.Contains("requires `--out`", exception.Message);
    }

    [Fact]
    public async Task Json_output_writer_emits_versioned_success_envelope()
    {
        var original = Console.Out;
        await using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            var result = new RenderExecutionResult
            {
                Layout = MarkdownLayout.Single,
                Source = new RenderSourceInfo("file", "sample.json", null, null),
                Stats = new RenderStats(1, 2, 3, 1),
                Warnings = [],
                IsDryRun = false,
                Files = [new RenderedFile("out.md", "C:\\temp\\out.md", string.Empty)],
                Summary = null,
            };

            var exitCode = await CommandOutputHandler.ExecuteAsync(
                ResolvedOutputMode.Json,
                verbose: false,
                () => Task.FromResult(result));

            var json = JsonNode.Parse(writer.ToString());

            Assert.Equal(0, exitCode);
            Assert.NotNull(json);
            Assert.True(json!["ok"]!.GetValue<bool>());
            Assert.Equal("single", json["data"]!["layout"]!.GetValue<string>());
            Assert.Equal(1, json["meta"]!["schemaVersion"]!.GetValue<int>());
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    private sealed class TestMarkdownSettings : MarkdownCommandSettingsBase
    {
    }
}
