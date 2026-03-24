using System.IO.Compression;
using System.Text.Json;
using System.Xml.Linq;
using InSpectra.Probe.Models;

namespace InSpectra.Probe;

public sealed class PackageProbeService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly SpectreCliAnalyzer analyzer = new();

    public PackageProbeResult Analyze(byte[] packageBytes)
    {
        try
        {
            var context = ReadPackage(packageBytes);
            var result = new PackageProbeResult
            {
                Package = new PackageDescriptor
                {
                    Id = context.Id,
                    Version = context.Version,
                    IsDotnetTool = context.IsDotnetTool,
                    CommandName = context.CommandName,
                    Runner = context.Runner,
                    EntryPoint = context.EntryPoint,
                    TargetFramework = context.TargetFramework,
                    HasPackagedOpenCli = context.HasPackagedOpenCli,
                    DocumentSource = "none"
                }
            };

            if (!context.IsDotnetTool)
            {
                result.Error = "The package is not marked as DotnetTool.";
                return result;
            }

            if (context.PackagedOpenCli is not null)
            {
                result.Status = "supported";
                result.Confidence = "high";
                result.Document = context.PackagedOpenCli;
                result.Package.DocumentSource = "packaged-opencli";
                result.Warnings.Add("Loaded packaged opencli.json from the NuGet package. No tool code was executed.");
                return result;
            }

            if (context.EntryAssemblyBytes is null)
            {
                result.Error = "The package does not bundle opencli.json and the tool entry assembly could not be located for browser-side static inspection.";
                return result;
            }

            return analyzer.Analyze(context, result);
        }
        catch (Exception error)
        {
            return new PackageProbeResult
            {
                Error = error.Message
            };
        }
    }

    private static ProbePackageContext ReadPackage(byte[] packageBytes)
    {
        using var stream = new MemoryStream(packageBytes, writable: false);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);

        var nuspecEntry = archive.Entries.FirstOrDefault(entry => entry.FullName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("The package nuspec was not found.");
        var nuspec = XDocument.Load(nuspecEntry.Open());
        var metadata = nuspec.Root?.Elements().FirstOrDefault(element => element.Name.LocalName == "metadata")
            ?? throw new InvalidOperationException("The package metadata node is missing.");

        var id = ReadElement(metadata, "id");
        var version = ReadElement(metadata, "version");
        var description = ReadElement(metadata, "description");
        var isDotnetTool = metadata
            .Descendants()
            .Any(element => element.Name.LocalName == "packageType" &&
                string.Equals((string?)element.Attribute("name"), "DotnetTool", StringComparison.OrdinalIgnoreCase));

        var toolSettingsEntry = archive.Entries.FirstOrDefault(entry =>
            entry.FullName.EndsWith("/DotnetToolSettings.xml", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(entry.FullName, "DotnetToolSettings.xml", StringComparison.OrdinalIgnoreCase));

        var command = ReadToolCommand(toolSettingsEntry);
        var packagedOpenCli = TryReadPackagedOpenCli(archive);
        var entryAssembly = FindEntryAssembly(archive, command.EntryPoint);

        return new ProbePackageContext
        {
            Id = id,
            Version = version,
            Description = description,
            IsDotnetTool = isDotnetTool,
            CommandName = command.Name,
            Runner = command.Runner,
            EntryPoint = command.EntryPoint,
            TargetFramework = entryAssembly.TargetFramework,
            EntryAssemblyBytes = entryAssembly.Bytes,
            HasPackagedOpenCli = packagedOpenCli is not null,
            PackagedOpenCli = packagedOpenCli
        };
    }

    private static (string? Name, string? Runner, string? EntryPoint) ReadToolCommand(ZipArchiveEntry? entry)
    {
        if (entry is null)
        {
            return default;
        }

        var document = XDocument.Load(entry.Open());
        var command = document.Descendants().FirstOrDefault(element => element.Name.LocalName == "Command");
        return (
            (string?)command?.Attribute("Name"),
            (string?)command?.Attribute("Runner"),
            (string?)command?.Attribute("EntryPoint"));
    }

    private static (byte[]? Bytes, string? TargetFramework) FindEntryAssembly(ZipArchive archive, string? entryPoint)
    {
        if (string.IsNullOrWhiteSpace(entryPoint))
        {
            return default;
        }

        var matches = archive.Entries
            .Where(entry =>
                entry.FullName.StartsWith("tools/", StringComparison.OrdinalIgnoreCase) &&
                entry.FullName.EndsWith("/" + entryPoint, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(entry => entry.FullName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var match = matches.FirstOrDefault();
        if (match is null)
        {
            return default;
        }

        using var source = match.Open();
        using var memory = new MemoryStream();
        source.CopyTo(memory);
        var segments = match.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return (memory.ToArray(), segments.Length >= 3 ? segments[1] : null);
    }

    private static OpenCliDocument? TryReadPackagedOpenCli(ZipArchive archive)
    {
        var entry = archive.Entries.FirstOrDefault(candidate =>
            candidate.FullName.EndsWith("/opencli.json", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(candidate.Name, "opencli.json", StringComparison.OrdinalIgnoreCase));
        if (entry is null)
        {
            return null;
        }

        using var reader = new StreamReader(entry.Open());
        return JsonSerializer.Deserialize<OpenCliDocument>(reader.ReadToEnd(), JsonOptions);
    }

    private static string ReadElement(XElement parent, string name)
    {
        return parent.Elements().FirstOrDefault(element => element.Name.LocalName == name)?.Value?.Trim() ?? string.Empty;
    }
}
