using System.Text.Json.Nodes;

namespace InSpectra.Lib.OpenCli.Validation;

/// <summary>
/// Performs structural transformations on an OpenCLI JSON document produced by
/// native introspection (e.g. Spectre.Console.Cli's <c>cli opencli</c>).
///
/// <list type="bullet">
///   <item>
///     <description>
///       Hoists <c>__default_command</c> nodes at every level: their options,
///       arguments, description, and examples are merged into the parent command
///       (or the document root), then the sentinel node is removed.
///     </description>
///   </item>
///   <item>
///     <description>
///       Cleans <c>info.title</c> values that look like assembly filenames
///       (e.g. <c>"metalama.dll"</c> → <c>"metalama"</c>).
///     </description>
///   </item>
/// </list>
/// </summary>
internal static class OpenCliStructuralSanitizer
{
    private const string DefaultCommandName = "__default_command";

    public static void Sanitize(JsonObject document)
    {
        SanitizeTitle(document);
        HoistDefaultCommands(document);
    }

    private static void SanitizeTitle(JsonObject document)
    {
        if (document["info"] is not JsonObject info)
        {
            return;
        }

        if (info["title"] is not JsonValue titleValue
            || !titleValue.TryGetValue<string>(out var title)
            || string.IsNullOrWhiteSpace(title))
        {
            return;
        }

        var cleaned = StripAssemblyExtension(title.Trim());
        if (!string.Equals(cleaned, title, StringComparison.Ordinal))
        {
            info["title"] = cleaned;
        }
    }

    private static string StripAssemblyExtension(string title)
    {
        if (title.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
            || title.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            return title[..^4];
        }

        return title;
    }

    /// <summary>
    /// Recursively walks the command tree and hoists every
    /// <c>__default_command</c> node into its parent.
    /// </summary>
    private static void HoistDefaultCommands(JsonObject parentNode)
    {
        if (parentNode["commands"] is not JsonArray commands)
        {
            return;
        }

        // First recurse into children so nested defaults are hoisted bottom-up.
        foreach (var child in commands.OfType<JsonObject>())
        {
            HoistDefaultCommands(child);
        }

        // Collect indices of __default_command entries (iterate backwards for safe removal).
        for (var i = commands.Count - 1; i >= 0; i--)
        {
            if (commands[i] is not JsonObject commandNode)
            {
                continue;
            }

            if (!IsDefaultCommand(commandNode))
            {
                continue;
            }

            MergeIntoParent(parentNode, commandNode);
            commands.RemoveAt(i);
        }
    }

    private static bool IsDefaultCommand(JsonObject command)
    {
        if (command["name"] is not JsonValue nameValue
            || !nameValue.TryGetValue<string>(out var name))
        {
            return false;
        }

        return string.Equals(name, DefaultCommandName, StringComparison.Ordinal);
    }

    private static void MergeIntoParent(JsonObject parent, JsonObject defaultCommand)
    {
        MergeArrayProperty(parent, defaultCommand, "options");
        MergeArrayProperty(parent, defaultCommand, "arguments");
        MergeArrayProperty(parent, defaultCommand, "examples");

        // Adopt description if parent doesn't have one.
        if (defaultCommand["description"] is JsonValue descValue
            && descValue.TryGetValue<string>(out var desc)
            && !string.IsNullOrWhiteSpace(desc))
        {
            if (parent["description"] is not JsonValue existingDesc
                || !existingDesc.TryGetValue<string>(out var existing)
                || string.IsNullOrWhiteSpace(existing))
            {
                parent["description"] = desc;
            }
        }
    }

    private static void MergeArrayProperty(JsonObject parent, JsonObject source, string propertyName)
    {
        if (source[propertyName] is not JsonArray sourceArray || sourceArray.Count == 0)
        {
            return;
        }

        if (parent[propertyName] is not JsonArray parentArray)
        {
            parentArray = [];
            parent[propertyName] = parentArray;
        }

        // Build a set of existing names/values to avoid duplicates.
        var existing = new HashSet<string>(StringComparer.Ordinal);
        foreach (var item in parentArray)
        {
            var key = GetMergeKey(item);
            if (key is not null)
            {
                existing.Add(key);
            }
        }

        // Clone items from source into parent, skipping duplicates.
        foreach (var item in sourceArray)
        {
            var key = GetMergeKey(item);
            if (key is not null && !existing.Add(key))
            {
                continue;
            }

            parentArray.Add(item?.DeepClone());
        }
    }

    /// <summary>
    /// Returns a dedup key for an array element: the <c>name</c> property for
    /// objects (options/arguments), or the string value for string arrays (examples).
    /// </summary>
    private static string? GetMergeKey(JsonNode? node)
    {
        if (node is JsonValue scalar && scalar.TryGetValue<string>(out var text))
        {
            return text;
        }

        if (node is JsonObject obj
            && obj["name"] is JsonValue nameValue
            && nameValue.TryGetValue<string>(out var name))
        {
            return name;
        }

        return null;
    }
}
