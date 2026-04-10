namespace InSpectra.Gen.Services;

using System.Diagnostics;

internal sealed class TemporaryWorkspace : IDisposable
{
    public TemporaryWorkspace(string prefix)
    {
        RootPath = Path.Combine(Path.GetTempPath(), $"{prefix}-{Guid.NewGuid():N}");
        Directory.CreateDirectory(RootPath);
    }

    public string RootPath { get; }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
        catch (IOException ex)
        {
            Trace.TraceWarning($"Failed to clean up temporary workspace '{RootPath}': {ex.Message}");
        }
    }
}
