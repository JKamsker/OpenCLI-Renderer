using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using InSpectra.Probe;
using InSpectra.Probe.Models;

namespace InSpectra.Probe.Wasm;

public static partial class ProbeExports
{
    private static readonly PackageProbeService Service = new();
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [JSExport]
    public static string AnalyzePackage(string base64Package)
    {
        var bytes = Convert.FromBase64String(base64Package);
        var result = Service.Analyze(bytes);
        return JsonSerializer.Serialize(result, JsonOptions);
    }
}
