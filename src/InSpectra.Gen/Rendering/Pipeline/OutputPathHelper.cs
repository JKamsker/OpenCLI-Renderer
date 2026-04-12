using InSpectra.Gen.Core;

namespace InSpectra.Gen.Rendering.Pipeline;

public static class OutputPathHelper
{
    public static void EnsureFileWritable(string outputFile, bool overwrite)
    {
        var resolvedOutputFile = Path.GetFullPath(outputFile);
        if (Directory.Exists(resolvedOutputFile))
        {
            throw new CliUsageException($"Output file `{outputFile}` points to an existing directory.");
        }

        if (File.Exists(resolvedOutputFile) && !overwrite)
        {
            throw new CliUsageException($"Output file `{outputFile}` already exists. Use `--overwrite` to replace it.");
        }
    }

    public static async Task PublishFileAsync(
        string outputFile,
        string content,
        bool overwrite,
        CancellationToken cancellationToken)
    {
        var publication = PrepareFilePublication(outputFile, overwrite);
        try
        {
            await File.WriteAllTextAsync(publication.TempPath, content, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            publication.Commit();
        }
        finally
        {
            publication.Dispose();
        }
    }

    public static void PrepareDirectory(string outputDirectory, bool overwrite)
    {
        var resolvedOutputDirectory = NormalizeDirectoryPath(outputDirectory);
        if (File.Exists(resolvedOutputDirectory))
        {
            throw new CliUsageException($"Output directory `{outputDirectory}` points to an existing file.");
        }

        if (!Directory.Exists(resolvedOutputDirectory))
        {
            Directory.CreateDirectory(resolvedOutputDirectory);
            return;
        }

        if (!Directory.EnumerateFileSystemEntries(resolvedOutputDirectory).Any())
        {
            return;
        }

        if (!overwrite)
        {
            throw new CliUsageException($"Output directory `{outputDirectory}` is not empty. Use `--overwrite` to replace it.");
        }

        EnsureDirectoryCanBeReplaced(resolvedOutputDirectory);
        ClearDirectoryContents(resolvedOutputDirectory);
    }

    public static async Task<TResult> PublishDirectoryAsync<TResult>(
        string outputDirectory,
        bool overwrite,
        Func<string, CancellationToken, Task<TResult>> writeAsync,
        CancellationToken cancellationToken)
    {
        var publication = PrepareDirectoryPublication(outputDirectory, overwrite);
        try
        {
            var result = await writeAsync(publication.StagingPath, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            publication.Commit();
            return result;
        }
        finally
        {
            publication.Dispose();
        }
    }

    private static void EnsureDirectoryCanBeReplaced(string outputDirectory)
    {
        var directoryRoot = Path.GetPathRoot(outputDirectory);
        if (!string.IsNullOrWhiteSpace(directoryRoot)
            && string.Equals(outputDirectory, NormalizeDirectoryPath(directoryRoot), StringComparison.OrdinalIgnoreCase))
        {
            throw new CliUsageException(
                $"Refusing to replace directory `{outputDirectory}` because it resolves to a filesystem root.");
        }

        var currentDirectory = NormalizeDirectoryPath(Environment.CurrentDirectory);
        if (IsSameOrAncestor(outputDirectory, currentDirectory))
        {
            throw new CliUsageException(
                $"Refusing to replace directory `{outputDirectory}` because it is the current working directory or one of its ancestors.");
        }
    }

    private static void ClearDirectoryContents(string outputDirectory)
    {
        foreach (var file in Directory.EnumerateFiles(outputDirectory))
        {
            File.Delete(file);
        }

        foreach (var directory in Directory.EnumerateDirectories(outputDirectory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static bool IsSameOrAncestor(string candidateAncestor, string path)
    {
        if (string.Equals(candidateAncestor, path, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return path.StartsWith(candidateAncestor + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            || path.StartsWith(candidateAncestor + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeDirectoryPath(string path)
        => Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));

    private static FilePublication PrepareFilePublication(string outputFile, bool overwrite)
    {
        var resolvedOutputFile = Path.GetFullPath(outputFile);
        EnsureFileWritable(resolvedOutputFile, overwrite);

        var directory = Path.GetDirectoryName(resolvedOutputFile);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return new FilePublication(
            resolvedOutputFile,
            resolvedOutputFile + $".{Guid.NewGuid():N}.tmp",
            overwrite);
    }

    private static DirectoryPublication PrepareDirectoryPublication(string outputDirectory, bool overwrite)
    {
        var resolvedOutputDirectory = NormalizeDirectoryPath(outputDirectory);
        if (File.Exists(resolvedOutputDirectory))
        {
            throw new CliUsageException($"Output directory `{outputDirectory}` points to an existing file.");
        }

        var targetExists = Directory.Exists(resolvedOutputDirectory);
        if (targetExists)
        {
            var hasEntries = Directory.EnumerateFileSystemEntries(resolvedOutputDirectory).Any();
            if (hasEntries && !overwrite)
            {
                throw new CliUsageException($"Output directory `{outputDirectory}` is not empty. Use `--overwrite` to replace it.");
            }

            if (hasEntries)
            {
                EnsureDirectoryCanBeReplaced(resolvedOutputDirectory);
            }
        }

        var parentDirectory = Path.GetDirectoryName(resolvedOutputDirectory);
        if (!string.IsNullOrWhiteSpace(parentDirectory))
        {
            Directory.CreateDirectory(parentDirectory);
        }

        var stagingPath = resolvedOutputDirectory + $".{Guid.NewGuid():N}.tmp";
        Directory.CreateDirectory(stagingPath);
        return new DirectoryPublication(resolvedOutputDirectory, stagingPath, overwrite, targetExists);
    }

    private sealed class FilePublication : IDisposable
    {
        private readonly string _targetPath;
        private readonly string _tempPath;
        private readonly bool _overwrite;
        private string? _backupPath;
        private bool _committed;

        public FilePublication(string targetPath, string tempPath, bool overwrite)
        {
            _targetPath = targetPath;
            _tempPath = tempPath;
            _overwrite = overwrite;
        }

        public string TempPath => _tempPath;

        public void Commit()
        {
            if (_overwrite && File.Exists(_targetPath))
            {
                _backupPath = _targetPath + $".{Guid.NewGuid():N}.bak";
                File.Move(_targetPath, _backupPath, overwrite: false);
            }

            try
            {
                File.Move(_tempPath, _targetPath, overwrite: false);
                _committed = true;
            }
            catch
            {
                RestoreBackup();
                throw;
            }
        }

        public void Dispose()
        {
            if (File.Exists(_tempPath))
            {
                File.Delete(_tempPath);
            }

            if (_committed && !string.IsNullOrWhiteSpace(_backupPath) && File.Exists(_backupPath))
            {
                TryDeleteFile(_backupPath);
            }
        }

        private void RestoreBackup()
        {
            if (string.IsNullOrWhiteSpace(_backupPath) || !File.Exists(_backupPath))
            {
                return;
            }

            File.Move(_backupPath, _targetPath, overwrite: false);
        }
    }

    private sealed class DirectoryPublication : IDisposable
    {
        private readonly string _targetPath;
        private readonly string _stagingPath;
        private readonly bool _overwrite;
        private readonly bool _targetExists;
        private string? _backupPath;
        private bool _committed;

        public DirectoryPublication(string targetPath, string stagingPath, bool overwrite, bool targetExists)
        {
            _targetPath = targetPath;
            _stagingPath = stagingPath;
            _overwrite = overwrite;
            _targetExists = targetExists;
        }

        public string StagingPath => _stagingPath;

        public void Commit()
        {
            if (_targetExists && Directory.Exists(_targetPath))
            {
                if (Directory.EnumerateFileSystemEntries(_targetPath).Any())
                {
                    if (!_overwrite)
                    {
                        throw new CliUsageException($"Output directory `{_targetPath}` is not empty. Use `--overwrite` to replace it.");
                    }

                    _backupPath = _targetPath + $".{Guid.NewGuid():N}.bak";
                    Directory.Move(_targetPath, _backupPath);
                }
                else
                {
                    PromoteStagingContentsToExistingDirectory();
                    _committed = true;
                    return;
                }
            }

            try
            {
                Directory.Move(_stagingPath, _targetPath);
                _committed = true;
            }
            catch
            {
                RestoreBackup();
                throw;
            }
        }

        public void Dispose()
        {
            if (Directory.Exists(_stagingPath))
            {
                Directory.Delete(_stagingPath, recursive: true);
            }

            if (_committed && !string.IsNullOrWhiteSpace(_backupPath) && Directory.Exists(_backupPath))
            {
                TryDeleteDirectory(_backupPath);
            }
        }

        private void PromoteStagingContentsToExistingDirectory()
        {
            foreach (var directory in Directory.EnumerateDirectories(_stagingPath))
            {
                Directory.Move(directory, Path.Combine(_targetPath, Path.GetFileName(directory)));
            }

            foreach (var file in Directory.EnumerateFiles(_stagingPath))
            {
                File.Move(file, Path.Combine(_targetPath, Path.GetFileName(file)), overwrite: false);
            }

            Directory.Delete(_stagingPath);
        }

        private void RestoreBackup()
        {
            if (string.IsNullOrWhiteSpace(_backupPath) || !Directory.Exists(_backupPath) || Directory.Exists(_targetPath))
            {
                return;
            }

            Directory.Move(_backupPath, _targetPath);
        }
    }

    private static void TryDeleteFile(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            Directory.Delete(path, recursive: true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
