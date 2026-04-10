namespace InSpectra.Gen.Acquisition.App.Composition;

using InSpectra.Gen.Acquisition.Analysis;
using InSpectra.Gen.Acquisition.Catalog;
using InSpectra.Gen.Acquisition.Docs;
using InSpectra.Gen.Acquisition.Promotion;
using InSpectra.Gen.Acquisition.Queue;
using Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDiscoveryCli(this IServiceCollection services)
    {
        services.AddCatalogModule();
        services.AddQueueModule();
        services.AddAnalysisModule();
        services.AddDocsModule();
        services.AddPromotionModule();

        return services;
    }
}


