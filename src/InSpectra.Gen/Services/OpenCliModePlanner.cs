using InSpectra.Discovery.Tool.Frameworks;
using InSpectra.Gen.Runtime;

namespace InSpectra.Gen.Services;

internal static class OpenCliModePlanner
{
    public static IReadOnlyList<OpenCliAcquisitionAttempt> BuildAutoPlan(
        string? cliFramework,
        string? hookCliFramework)
    {
        var attempts = new List<OpenCliAcquisitionAttempt>();
        var hookFrameworks = CliFrameworkProviderRegistry.ResolveFrameworkNames(hookCliFramework)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var provider in CliFrameworkProviderRegistry.ResolveAnalysisProviders(cliFramework))
        {
            if (provider.SupportsCliFxAnalysis)
            {
                attempts.Add(new OpenCliAcquisitionAttempt("clifx", provider.Name, "planned"));
            }

            if (provider.SupportsHookAnalysis && hookFrameworks.Contains(provider.Name))
            {
                attempts.Add(new OpenCliAcquisitionAttempt("hook", provider.Name, "planned"));
            }

            if (provider.StaticAnalysisAdapter is not null)
            {
                attempts.Add(new OpenCliAcquisitionAttempt("static", provider.Name, "planned"));
            }
        }

        if (attempts.Count == 0)
        {
            attempts.Add(new OpenCliAcquisitionAttempt("help", null, "planned"));
            return attempts;
        }

        attempts.Add(new OpenCliAcquisitionAttempt("help", null, "planned"));
        return attempts;
    }

    public static string ToModeValue(OpenCliMode mode)
        => mode switch
        {
            OpenCliMode.Native => "native",
            OpenCliMode.Auto => "auto",
            OpenCliMode.Help => "help",
            OpenCliMode.CliFx => "clifx",
            OpenCliMode.Static => "static",
            OpenCliMode.Hook => "hook",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
}
