namespace InSpectra.Gen.Acquisition.OpenCli.Options.Collisions;


using System.Text.Json.Nodes;

internal sealed record OpenCliOptionCollisionEntry(JsonObject Option, IReadOnlySet<string> Tokens);

