namespace InSpectra.Gen.Runtime.Rendering;

public sealed record RenderSourceInfo(
    string Kind,
    string OpenCliOrigin,
    string? XmlDocOrigin,
    string? ExecutablePath);
