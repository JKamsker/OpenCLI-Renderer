namespace InSpectra.Gen.Acquisition.Modes.Help.OpenCli;


internal sealed record CommandNode(
    string FullName,
    string DisplayName,
    string? Description)
{
    public IReadOnlyList<CommandNode> Children { get; init; } = [];
}
