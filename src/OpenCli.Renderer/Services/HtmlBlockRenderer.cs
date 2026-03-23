using System.Text;
using OpenCli.Renderer.Models;

namespace OpenCli.Renderer.Services;

public sealed class HtmlBlockRenderer(
    HtmlContentFormatter contentFormatter,
    RenderModelFormatter formatter)
{
    public void AppendArgumentTable(IEnumerable<OpenCliArgument> arguments, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"table-wrap\"><table><thead><tr><th>Name</th><th>Required</th><th>Arity</th><th>Accepted values</th><th>Group</th><th>Description</th></tr></thead><tbody>");
        foreach (var argument in arguments)
        {
            builder.AppendLine($"<tr><td><code>{contentFormatter.Encode(argument.Hidden ? $"{argument.Name} (hidden)" : argument.Name)}</code></td><td>{contentFormatter.Encode(argument.Required ? "Yes" : "No")}</td><td>{contentFormatter.Encode(formatter.FormatArity(argument))}</td><td>{contentFormatter.Encode(argument.AcceptedValues.Count == 0 ? "—" : string.Join(", ", argument.AcceptedValues))}</td><td>{contentFormatter.Encode(argument.Group ?? "—")}</td><td>{contentFormatter.Encode(argument.Description ?? "—")}</td></tr>");
        }

        builder.AppendLine("</tbody></table></div>");
    }

    public void AppendOptionCards(IEnumerable<ResolvedOption> options, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"option-list\">");
        foreach (var resolved in options)
        {
            AppendOptionCard(resolved, builder);
        }

        builder.AppendLine("</div>");
    }

    public void AppendExamples(IEnumerable<string> examples, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"example-list\">");
        foreach (var example in examples)
        {
            builder.AppendLine($"<pre><code>$ {contentFormatter.Encode(example)}</code></pre>");
        }

        builder.AppendLine("</div>");
    }

    public void AppendExitCodeTable(IEnumerable<OpenCliExitCode> exitCodes, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"table-wrap\"><table><thead><tr><th>Code</th><th>Description</th></tr></thead><tbody>");
        foreach (var exitCode in exitCodes)
        {
            builder.AppendLine($"<tr><td><code>{exitCode.Code}</code></td><td>{contentFormatter.Encode(exitCode.Description ?? "—")}</td></tr>");
        }

        builder.AppendLine("</tbody></table></div>");
    }

    public void AppendMetadataPanel(IEnumerable<OpenCliMetadata> metadata, StringBuilder builder)
    {
        builder.AppendLine("<dl class=\"metadata-list\">");
        foreach (var item in metadata)
        {
            builder.AppendLine($"<div><dt>{contentFormatter.Encode(item.Name)}</dt><dd>{contentFormatter.FormatMetadataValue(item)}</dd></div>");
        }

        builder.AppendLine("</dl>");
    }

    public void AppendCommandMetadata(NormalizedCommand command, StringBuilder builder)
    {
        if (!HasMetadata(command))
        {
            return;
        }

        builder.AppendLine("<div class=\"detail-block\"><h3>Metadata appendix</h3>");
        AppendMetadataSection("Command", command.Command.Metadata, builder);

        foreach (var argument in command.Arguments.Where(argument => argument.Metadata.Count > 0))
        {
            AppendMetadataSection($"Argument <code>{contentFormatter.Encode(argument.Name)}</code>", argument.Metadata, builder);
        }

        foreach (var option in command.DeclaredOptions.Where(option => option.Metadata.Count > 0))
        {
            AppendMetadataSection($"Option <code>{contentFormatter.Encode(option.Name)}</code>", option.Metadata, builder);
        }

        foreach (var option in command.InheritedOptions.Where(option => option.Option.Metadata.Count > 0))
        {
            AppendMetadataSection($"Inherited <code>{contentFormatter.Encode(option.Option.Name)}</code> from <code>{contentFormatter.Encode(option.InheritedFromPath)}</code>", option.Option.Metadata, builder);
        }

        builder.AppendLine("</div>");
    }

    private void AppendOptionCard(ResolvedOption resolved, StringBuilder builder)
    {
        var option = resolved.Option;
        var aliases = string.Join(" ", option.Aliases.Select(alias => $"<code>{contentFormatter.Encode(alias)}</code>"));
        builder.AppendLine("<article class=\"panel option-card\">");
        builder.AppendLine($"<div class=\"option-head\"><div><strong><code>{contentFormatter.Encode(option.Hidden ? $"{option.Name} (hidden)" : option.Name)}</code></strong></div><div class=\"option-aliases\">{aliases}</div></div>");
        builder.AppendLine("<div class=\"badge-row\">");
        builder.AppendLine($"<span class=\"badge badge-primary\">{contentFormatter.Encode(formatter.FormatOptionValue(option))}</span>");
        if (option.Required) builder.AppendLine("<span class=\"badge badge-danger\">Required</span>");
        if (option.Recursive) builder.AppendLine("<span class=\"badge badge-success\">Recursive</span>");
        builder.AppendLine($"<span class=\"badge\">{contentFormatter.Encode(resolved.IsInherited ? $"Inherited from {resolved.InheritedFromPath}" : "Declared")}</span>");
        if (!string.IsNullOrWhiteSpace(option.Group)) builder.AppendLine($"<span class=\"badge\">Group {contentFormatter.Encode(option.Group)}</span>");

        foreach (var argument in option.Arguments)
        {
            builder.AppendLine($"<span class=\"badge\">{contentFormatter.Encode(argument.Name)} · {contentFormatter.Encode(formatter.FormatArity(argument))}</span>");
            var clrType = formatter.TryGetClrType(argument.Metadata);
            if (clrType is not null) builder.AppendLine($"<span class=\"badge badge-warning\">{contentFormatter.Encode(clrType)}</span>");
        }

        builder.AppendLine("</div>");
        builder.AppendLine($"<p>{contentFormatter.EncodeOrFallback(option.Description, "No description provided.")}</p>");
        builder.AppendLine("</article>");
    }

    private void AppendMetadataSection(string heading, IEnumerable<OpenCliMetadata> metadata, StringBuilder builder)
    {
        var items = metadata.ToList();
        if (items.Count == 0)
        {
            return;
        }

        builder.AppendLine($"<section class=\"panel metadata-panel\"><h4>{heading}</h4>");
        AppendMetadataPanel(items, builder);
        builder.AppendLine("</section>");
    }

    private static bool HasMetadata(NormalizedCommand command)
    {
        return command.Command.Metadata.Count > 0 ||
               command.Arguments.Any(argument => argument.Metadata.Count > 0) ||
               command.DeclaredOptions.Any(option => option.Metadata.Count > 0) ||
               command.InheritedOptions.Any(option => option.Option.Metadata.Count > 0);
    }
}
