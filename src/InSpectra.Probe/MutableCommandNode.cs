using InSpectra.Probe.Models;

namespace InSpectra.Probe;

internal sealed class MutableCommandNode
{
    public required string Name { get; init; }

    public string? Description { get; set; }

    public bool Hidden { get; set; }

    public string? CommandTypeName { get; set; }

    public string? SettingsTypeName { get; set; }

    public List<string> Aliases { get; } = [];

    public List<string> Examples { get; } = [];

    public List<OpenCliOption> Options { get; } = [];

    public List<OpenCliArgument> Arguments { get; } = [];

    public List<MutableCommandNode> Commands { get; } = [];

    public OpenCliCommand ToOpenCli()
    {
        return new OpenCliCommand
        {
            Name = Name,
            Aliases = [.. Aliases],
            Options = [.. Options],
            Arguments = [.. Arguments],
            Commands = Commands.Select(command => command.ToOpenCli()).ToList(),
            ExitCodes = [],
            Description = Description,
            Hidden = Hidden,
            Examples = [.. Examples],
            Interactive = false,
            Metadata = BuildMetadata()
        };
    }

    private List<OpenCliMetadata> BuildMetadata()
    {
        var metadata = new List<OpenCliMetadata>();
        if (!string.IsNullOrWhiteSpace(CommandTypeName))
        {
            metadata.Add(new OpenCliMetadata { Name = "ClrType", Value = CommandTypeName });
        }

        if (!string.IsNullOrWhiteSpace(SettingsTypeName))
        {
            metadata.Add(new OpenCliMetadata { Name = "SettingsType", Value = SettingsTypeName });
        }

        return metadata;
    }
}
