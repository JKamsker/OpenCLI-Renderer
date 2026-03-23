using System.Reflection;

namespace OpenCli.Renderer.Services;

public sealed class HtmlAssetProvider
{
    private readonly Lazy<string> _styles = new(() => LoadResource("OpenCli.Renderer.Templates.HtmlRenderer.css"));
    private readonly Lazy<string> _script = new(() => LoadResource("OpenCli.Renderer.Templates.HtmlRenderer.js"));

    public string GetStyles() => _styles.Value;

    public string GetScript() => _script.Value;

    private static string LoadResource(string resourceName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Missing embedded resource `{resourceName}`.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
