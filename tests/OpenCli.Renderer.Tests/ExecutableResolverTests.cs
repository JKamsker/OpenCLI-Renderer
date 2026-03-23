using OpenCli.Renderer.Services;
using OpenCli.Renderer.Tests.TestSupport;

namespace OpenCli.Renderer.Tests;

public class ExecutableResolverTests
{
    [Fact]
    public void Resolve_accepts_bare_name_file_name_and_full_path()
    {
        using var temp = new TempDirectory();
        var executablePath = System.IO.Path.Combine(temp.Path, "demo.cmd");
        File.WriteAllText(executablePath, "@echo off");

        var resolver = new ExecutableResolver();

        Assert.Equal(executablePath, resolver.Resolve("demo", temp.Path));
        Assert.Equal(executablePath, resolver.Resolve("demo.cmd", temp.Path));
        Assert.Equal(executablePath, resolver.Resolve(executablePath, temp.Path));
    }
}
