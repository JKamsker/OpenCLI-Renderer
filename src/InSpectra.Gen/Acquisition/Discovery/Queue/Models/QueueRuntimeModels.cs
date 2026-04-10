namespace InSpectra.Gen.Acquisition.Queue.Models;

internal sealed record DotnetRuntimeRequirement(
    string Name,
    string Version,
    string Channel,
    string Runtime);
