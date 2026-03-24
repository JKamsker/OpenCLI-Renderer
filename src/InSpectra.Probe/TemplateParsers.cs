namespace InSpectra.Probe;

internal readonly record struct OptionTemplate(string Name, List<string> Aliases, string? ValueName, bool ValueRequired)
{
    public static OptionTemplate Parse(string template)
    {
        var segments = template.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var aliasSegment = segments[0];
        var valueSegment = segments.Length > 1 ? segments[1].Trim() : null;
        var aliases = aliasSegment.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        var primary = aliases
            .OrderByDescending(alias => alias.StartsWith("--", StringComparison.Ordinal))
            .ThenByDescending(alias => alias.Length)
            .FirstOrDefault() ?? aliasSegment;
        aliases.Remove(primary);
        var normalizedAliases = aliases
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(alias => alias, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return new OptionTemplate(primary, normalizedAliases, ReadValueName(valueSegment), !IsOptional(valueSegment));
    }

    private static bool IsOptional(string? valueSegment)
    {
        return !string.IsNullOrWhiteSpace(valueSegment) &&
            valueSegment.StartsWith("[", StringComparison.Ordinal) &&
            valueSegment.EndsWith("]", StringComparison.Ordinal);
    }

    private static string? ReadValueName(string? valueSegment)
    {
        if (string.IsNullOrWhiteSpace(valueSegment))
        {
            return null;
        }

        return valueSegment.Trim().Trim('<', '>', '[', ']');
    }
}

internal readonly record struct PositionalTemplate(string Name, bool Required)
{
    public static PositionalTemplate Parse(string template)
    {
        return new PositionalTemplate(
            template.Trim().Trim('<', '>', '[', ']'),
            template.StartsWith("<", StringComparison.Ordinal));
    }
}
