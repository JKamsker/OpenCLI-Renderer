using System.Text;
using OpenCli.Renderer.Models;

namespace OpenCli.Renderer.Services;

public sealed class MarkdownTableRenderer(RenderModelFormatter formatter)
{
    public void AppendArgumentTable(IEnumerable<OpenCliArgument> arguments, StringBuilder builder)
    {
        builder.AppendLine("| Name | Required | Arity | Accepted Values | Group | Description |");
        builder.AppendLine("| --- | --- | --- | --- | --- | --- |");
        foreach (var argument in arguments)
        {
            builder.Append("| ");
            builder.Append(EscapeCell(argument.Hidden ? $"{argument.Name} (hidden)" : argument.Name));
            builder.Append(" | ");
            builder.Append(argument.Required ? "Yes" : "No");
            builder.Append(" | ");
            builder.Append(EscapeCell(formatter.FormatArity(argument)));
            builder.Append(" | ");
            builder.Append(EscapeCell(argument.AcceptedValues.Count == 0 ? "—" : string.Join(", ", argument.AcceptedValues)));
            builder.Append(" | ");
            builder.Append(EscapeCell(argument.Group ?? "—"));
            builder.Append(" | ");
            builder.Append(EscapeCell(argument.Description ?? "—"));
            builder.AppendLine(" |");
        }

        builder.AppendLine();
    }

    public void AppendOptionTable(IEnumerable<ResolvedOption> options, StringBuilder builder)
    {
        builder.AppendLine("| Name | Aliases | Value | Required | Recursive | Scope | Group | Description |");
        builder.AppendLine("| --- | --- | --- | --- | --- | --- | --- | --- |");
        foreach (var resolved in options)
        {
            var option = resolved.Option;
            builder.Append("| ");
            builder.Append(EscapeCell(option.Hidden ? $"{option.Name} (hidden)" : option.Name));
            builder.Append(" | ");
            builder.Append(EscapeCell(option.Aliases.Count == 0 ? "—" : string.Join(", ", option.Aliases)));
            builder.Append(" | ");
            builder.Append(EscapeCell(formatter.FormatOptionValue(option)));
            builder.Append(" | ");
            builder.Append(option.Required ? "Yes" : "No");
            builder.Append(" | ");
            builder.Append(option.Recursive ? "Yes" : "No");
            builder.Append(" | ");
            builder.Append(EscapeCell(resolved.IsInherited ? $"Inherited from {resolved.InheritedFromPath}" : "Declared"));
            builder.Append(" | ");
            builder.Append(EscapeCell(option.Group ?? "—"));
            builder.Append(" | ");
            builder.Append(EscapeCell(option.Description ?? "—"));
            builder.AppendLine(" |");
        }

        builder.AppendLine();
    }

    public void AppendExitCodeTable(IEnumerable<OpenCliExitCode> exitCodes, StringBuilder builder)
    {
        builder.AppendLine("| Code | Description |");
        builder.AppendLine("| --- | --- |");
        foreach (var exitCode in exitCodes)
        {
            builder.AppendLine($"| `{exitCode.Code}` | {EscapeCell(exitCode.Description ?? "—")} |");
        }

        builder.AppendLine();
    }

    private static string EscapeCell(string value)
    {
        return value.Replace("\r", string.Empty, StringComparison.Ordinal)
            .Replace('\n', ' ')
            .Replace("|", "\\|", StringComparison.Ordinal);
    }
}
