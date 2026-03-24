using System.IO.Compression;
using System.Text;

namespace InSpectra.Gen.Tests.TestSupport;

internal static class ToolPackageBuilder
{
    public static byte[] CreateInspectraPackage()
    {
        var outputDirectory = FindProjectOutput("inspectra.dll");
        var assemblyPath = Path.Combine(outputDirectory, "inspectra.dll");
        var depsPath = Path.Combine(outputDirectory, "inspectra.deps.json");

        using var stream = new MemoryStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        WriteText(archive, "InSpectra.Gen.nuspec", """
            <?xml version="1.0" encoding="utf-8"?>
            <package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
              <metadata>
                <id>InSpectra.Gen</id>
                <version>0.1.0</version>
                <description>Test probe package.</description>
                <packageTypes>
                  <packageType name="DotnetTool" />
                </packageTypes>
              </metadata>
            </package>
            """);
        WriteText(archive, "tools/net10.0/any/DotnetToolSettings.xml", """
            <?xml version="1.0" encoding="utf-8"?>
            <DotNetCliTool Version="1">
              <Commands>
                <Command Name="inspectra" EntryPoint="inspectra.dll" Runner="dotnet" />
              </Commands>
            </DotNetCliTool>
            """);
        WriteBytes(archive, "tools/net10.0/any/inspectra.dll", File.ReadAllBytes(assemblyPath));
        WriteBytes(archive, "tools/net10.0/any/inspectra.deps.json", File.ReadAllBytes(depsPath));
        archive.Dispose();
        return stream.ToArray();
    }

    public static byte[] CreatePackagedOpenCliTool(string openCliJson)
    {
        using var stream = new MemoryStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        WriteText(archive, "demo.nuspec", """
            <?xml version="1.0" encoding="utf-8"?>
            <package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
              <metadata>
                <id>Demo.Tool</id>
                <version>2.0.0</version>
                <description>Packaged OpenCLI.</description>
                <packageTypes>
                  <packageType name="DotnetTool" />
                </packageTypes>
              </metadata>
            </package>
            """);
        WriteText(archive, "tools/net10.0/any/DotnetToolSettings.xml", """
            <?xml version="1.0" encoding="utf-8"?>
            <DotNetCliTool Version="1">
              <Commands>
                <Command Name="demo" EntryPoint="demo.dll" Runner="dotnet" />
              </Commands>
            </DotNetCliTool>
            """);
        WriteText(archive, "tools/net10.0/any/opencli.json", openCliJson);
        archive.Dispose();
        return stream.ToArray();
    }

    public static byte[] CreatePlainPackage()
    {
        using var stream = new MemoryStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        WriteText(archive, "plain.nuspec", """
            <?xml version="1.0" encoding="utf-8"?>
            <package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
              <metadata>
                <id>Plain.Package</id>
                <version>1.0.0</version>
                <description>Not a tool.</description>
              </metadata>
            </package>
            """);
        archive.Dispose();
        return stream.ToArray();
    }

    private static string FindProjectOutput(string fileName)
    {
        var root = Path.Combine(FixturePaths.RepoRoot, "src", "InSpectra.Gen", "bin");
        var matches = Directory.GetFiles(root, fileName, SearchOption.AllDirectories)
            .OrderByDescending(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return matches.Count > 0
            ? Path.GetDirectoryName(matches[0])!
            : throw new FileNotFoundException($"Could not locate built tool output for {fileName}.", root);
    }

    private static void WriteText(ZipArchive archive, string path, string content)
    {
        WriteBytes(archive, path, Encoding.UTF8.GetBytes(content));
    }

    private static void WriteBytes(ZipArchive archive, string path, byte[] content)
    {
        var entry = archive.CreateEntry(path);
        using var entryStream = entry.Open();
        entryStream.Write(content);
    }
}
