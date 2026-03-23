using System.Text;
using OpenCli.Renderer.Models;

namespace OpenCli.Renderer.Services;

public sealed class CommandPathResolver
{
    public string CreateAnchorId(string value)
    {
        var builder = new StringBuilder();
        foreach (var character in value.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
            }
            else if (character is ' ' or '-' or '_')
            {
                builder.Append('-');
            }
        }

        return builder.ToString().Trim('-');
    }

    public string GetCommandRelativePath(NormalizedCommand command, string extension)
    {
        var parts = command.Path.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(SanitizePathSegment)
            .ToArray();

        if (command.Commands.Count > 0)
        {
            return Path.Combine(parts).Replace('\\', '/') + $"/index.{extension}";
        }

        var parent = parts.Length > 1 ? Path.Combine(parts[..^1]).Replace('\\', '/') : string.Empty;
        var fileName = $"{parts[^1]}.{extension}";
        return string.IsNullOrEmpty(parent) ? fileName : $"{parent}/{fileName}";
    }

    public string CreateRelativeLink(string currentPagePath, string targetPath)
    {
        var currentDirectory = Path.GetDirectoryName(currentPagePath)?.Replace('\\', '/');
        var baseDirectory = string.IsNullOrWhiteSpace(currentDirectory) ? "." : currentDirectory;
        return Path.GetRelativePath(baseDirectory, targetPath).Replace('\\', '/');
    }

    public string? GetParentRelativePath(NormalizedCommand command, string extension)
    {
        var parts = command.Path.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            return $"index.{extension}";
        }

        var parentParts = parts[..^1];
        var leadingPath = parentParts.Length == 1
            ? string.Empty
            : Path.Combine(parentParts[..^1].Select(SanitizePathSegment).ToArray()).Replace('\\', '/');
        var lastParent = SanitizePathSegment(parentParts[^1]);
        return string.IsNullOrEmpty(leadingPath)
            ? $"{lastParent}/index.{extension}"
            : $"{leadingPath}/{lastParent}/index.{extension}";
    }

    public string GetParentDisplayName(NormalizedCommand command)
    {
        var parts = command.Path.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Join(' ', parts[..^1]);
    }

    private static string SanitizePathSegment(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(value.Select(character => invalid.Contains(character) ? '-' : character).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "command" : sanitized;
    }
}
