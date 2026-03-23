using OpenCli.Renderer.Models;
using OpenCli.Renderer.Runtime;

namespace OpenCli.Renderer.Services;

public interface IDocumentRenderer
{
    DocumentFormat Format { get; }

    string RenderSingle(NormalizedCliDocument document, bool includeMetadata);

    IReadOnlyList<RelativeRenderedFile> RenderTree(NormalizedCliDocument document, bool includeMetadata);
}

public sealed record RelativeRenderedFile(string RelativePath, string Content);
