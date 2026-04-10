namespace InSpectra.Gen.Runtime;

public sealed record RenderSourceInfo(
    string Kind,
    string OpenCliOrigin,
    string? XmlDocOrigin,
    string? ExecutablePath);
