using System.Text.Json.Nodes;
using System.Xml.Linq;
using InSpectra.Gen.Tests.TestSupport;

namespace InSpectra.Gen.Tests;

public class SelfDocumentationSnapshotTests
{
    [Fact]
    public void Opencli_snapshot_keeps_render_file_only_and_generate_source_modes()
    {
        var path = Path.Combine(FixturePaths.RepoRoot, "docs", "inspectra-gen", "opencli.json");
        var document = JsonNode.Parse(File.ReadAllText(path))!.AsObject();

        var render = FindCommand(document, "render");
        var generate = FindCommand(document, "generate");

        var renderCommands = render["commands"]!.AsArray()
            .Select(command => command!["name"]!.GetValue<string>())
            .ToArray();
        var generateCommands = generate["commands"]!.AsArray()
            .Select(command => command!["name"]!.GetValue<string>())
            .ToArray();

        Assert.Equal(["file"], renderCommands);
        Assert.Equal(
            ["dotnet", "exec", "package"],
            generateCommands.OrderBy(name => name, StringComparer.Ordinal).ToArray());
        AssertHtmlCommand(render, "file");
    }

    [Fact]
    public void Xmldoc_snapshot_keeps_render_and_generate_settings_types_in_sync()
    {
        var path = Path.Combine(FixturePaths.RepoRoot, "docs", "inspectra-gen", "xmldoc.xml");
        var model = XDocument.Load(path);

        var render = FindCommand(model, "render");
        var generate = FindCommand(model, "generate");
        var file = FindChildCommand(render, "file");
        var fileHtml = FindChildCommand(file, "html");
        var exec = FindChildCommand(generate, "exec");

        Assert.Equal("InSpectra.Gen.Commands.Render.FileHtmlSettings", fileHtml.Attribute("Settings")?.Value);
        Assert.Equal("InSpectra.Gen.Commands.Generate.ExecGenerateSettings", exec.Attribute("Settings")?.Value);

        AssertHtmlOptions(fileHtml);
        AssertGenerateOptions(exec);
    }

    [Fact]
    public void Xmldoc_snapshot_documents_every_cli_parameter()
    {
        var path = Path.Combine(FixturePaths.RepoRoot, "docs", "inspectra-gen", "xmldoc.xml");
        var model = XDocument.Load(path);

        var undocumented = model.Descendants()
            .Where(element => element.Name.LocalName is "Argument" or "Option")
            .Where(element => string.IsNullOrWhiteSpace(element.Element("Description")?.Value))
            .Select(DescribeParameter)
            .ToArray();

        Assert.True(
            undocumented.Length == 0,
            "Undocumented CLI parameters:" + Environment.NewLine + string.Join(Environment.NewLine, undocumented));
    }

    private static void AssertHtmlCommand(JsonObject render, string branchName)
    {
        var branch = render["commands"]!.AsArray()
            .Single(command => command!["name"]!.GetValue<string>() == branchName)!;
        var html = branch["commands"]!.AsArray()
            .Single(command => command!["name"]!.GetValue<string>() == "html")!
            .AsObject();

        var optionNames = html["options"]!.AsArray()
            .Select(option => option!["name"]!.GetValue<string>())
            .ToArray();

        Assert.Contains("--out-dir", optionNames);
        Assert.DoesNotContain("--out", optionNames);
        Assert.DoesNotContain("--layout", optionNames);
        Assert.Contains("HTML app bundle", html["description"]!.GetValue<string>());
    }

    private static JsonObject FindCommand(JsonObject document, string commandName)
    {
        return document["commands"]!.AsArray()
            .Single(command => command!["name"]!.GetValue<string>() == commandName)!
            .AsObject();
    }

    private static XElement FindCommand(XDocument model, string commandName)
    {
        return model.Root!.Elements("Command")
            .Single(command => command.Attribute("Name")?.Value == commandName);
    }

    private static XElement FindChildCommand(XElement parent, string commandName)
    {
        return parent.Elements("Command")
            .Single(command => command.Attribute("Name")?.Value == commandName);
    }

    private static void AssertHtmlOptions(XElement command)
    {
        var optionNames = command
            .Descendants("Option")
            .Select(option => option.Attribute("Long")?.Value)
            .Where(value => value is not null)
            .ToArray();

        Assert.Contains("out-dir", optionNames);
        Assert.DoesNotContain("out", optionNames);
        Assert.DoesNotContain("layout", optionNames);
    }

    private static void AssertGenerateOptions(XElement command)
    {
        var optionNames = command
            .Descendants("Option")
            .Select(option => option.Attribute("Long")?.Value)
            .Where(value => value is not null)
            .ToArray();

        Assert.Contains("out", optionNames);
        Assert.Contains("opencli-mode", optionNames);
        Assert.Contains("with-xmldoc", optionNames);
        Assert.Contains("xmldoc-arg", optionNames);
        Assert.DoesNotContain("out-dir", optionNames);
    }

    private static string DescribeParameter(XElement parameter)
    {
        var commandPath = string.Join(
            " ",
            parameter.Ancestors("Command")
                .Reverse()
                .Select(command => command.Attribute("Name")?.Value)
                .Where(name => !string.IsNullOrWhiteSpace(name)));

        var label = parameter.Name.LocalName switch
        {
            "Argument" => parameter.Attribute("Name")?.Value,
            "Option" => "--" + parameter.Attribute("Long")?.Value,
            _ => parameter.Attribute("Name")?.Value,
        };

        return $"{commandPath} {label}".Trim();
    }
}
