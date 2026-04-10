using InSpectra.Gen.Runtime;
using InSpectra.Gen.Runtime.Rendering;

namespace InSpectra.Gen.Services;

public interface IDocumentRenderService
{
    Task<AcquiredRenderDocument> LoadFromFileAsync(
        FileRenderRequest request,
        CancellationToken cancellationToken);
}
