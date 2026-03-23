using System.Text;
using System.Text.Json;
using OpenCli.Renderer.Models;

namespace OpenCli.Renderer.Services;

public sealed class MarkdownMetadataRenderer
{
    public void AppendRootMetadata(OpenCliDocument document, StringBuilder builder)
    {
        if (document.Metadata.Count == 0)
        {
            return;
        }

        builder.AppendLine("## Metadata Appendix");
        builder.AppendLine();
        builder.AppendLine("### Root");
        builder.AppendLine();
        AppendMetadataList(document.Metadata, builder);
    }

    public void AppendCommandMetadata(NormalizedCommand command, StringBuilder builder, int headingLevel)
    {
        if (!HasMetadata(command))
        {
            return;
        }

        builder.AppendLine($"{new string('#', headingLevel)} Metadata Appendix");
        builder.AppendLine();
        AppendMetadataSection("#### Command", command.Command.Metadata, builder);
        AppendArgumentMetadata(command.Arguments, builder);
        AppendOptionMetadata("#### Declared Options", command.DeclaredOptions, builder);
        AppendInheritedOptionMetadata(command.InheritedOptions, builder);
    }

    private static bool HasMetadata(NormalizedCommand command)
    {
        return command.Command.Metadata.Count > 0 ||
               command.Arguments.Any(argument => argument.Metadata.Count > 0) ||
               command.DeclaredOptions.Any(option => option.Metadata.Count > 0) ||
               command.InheritedOptions.Any(option => option.Option.Metadata.Count > 0);
    }

    private static void AppendMetadataSection(string heading, IEnumerable<OpenCliMetadata> metadata, StringBuilder builder)
    {
        var items = metadata.ToList();
        if (items.Count == 0)
        {
            return;
        }

        builder.AppendLine(heading);
        builder.AppendLine();
        AppendMetadataList(items, builder);
    }

    private static void AppendArgumentMetadata(IEnumerable<OpenCliArgument> arguments, StringBuilder builder)
    {
        var argumentMetadata = arguments.Where(argument => argument.Metadata.Count > 0).ToList();
        if (argumentMetadata.Count == 0)
        {
            return;
        }

        builder.AppendLine("#### Arguments");
        builder.AppendLine();
        foreach (var argument in argumentMetadata)
        {
            builder.AppendLine($"- `{argument.Name}`");
            AppendMetadataList(argument.Metadata, builder, "  ");
        }

        builder.AppendLine();
    }

    private static void AppendOptionMetadata(string heading, IEnumerable<OpenCliOption> options, StringBuilder builder)
    {
        var optionMetadata = options.Where(option => option.Metadata.Count > 0).ToList();
        if (optionMetadata.Count == 0)
        {
            return;
        }

        builder.AppendLine(heading);
        builder.AppendLine();
        foreach (var option in optionMetadata)
        {
            builder.AppendLine($"- `{option.Name}`");
            AppendMetadataList(option.Metadata, builder, "  ");
        }

        builder.AppendLine();
    }

    private static void AppendInheritedOptionMetadata(IEnumerable<ResolvedOption> options, StringBuilder builder)
    {
        var inheritedMetadata = options.Where(option => option.Option.Metadata.Count > 0).ToList();
        if (inheritedMetadata.Count == 0)
        {
            return;
        }

        builder.AppendLine("#### Inherited Options");
        builder.AppendLine();
        foreach (var option in inheritedMetadata)
        {
            builder.AppendLine($"- `{option.Option.Name}` from `{option.InheritedFromPath}`");
            AppendMetadataList(option.Option.Metadata, builder, "  ");
        }

        builder.AppendLine();
    }

    private static void AppendMetadataList(IEnumerable<OpenCliMetadata> metadata, StringBuilder builder, string indent = "")
    {
        foreach (var item in metadata)
        {
            builder.Append(indent);
            builder.Append("- `");
            builder.Append(item.Name);
            builder.Append("`: ");
            builder.AppendLine(FormatMetadataValue(item));
        }

        if (string.IsNullOrEmpty(indent))
        {
            builder.AppendLine();
        }
    }

    private static string FormatMetadataValue(OpenCliMetadata metadata)
    {
        if (metadata.Value is null)
        {
            return "null";
        }

        return metadata.Value.GetValueKind() switch
        {
            JsonValueKind.String => $"`{metadata.Value.GetValue<string>()}`",
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => $"`{metadata.Value}`",
            _ => $"```json{Environment.NewLine}{metadata.Value.ToJsonString(new JsonSerializerOptions { WriteIndented = true })}{Environment.NewLine}```",
        };
    }
}
