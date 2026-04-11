namespace InSpectra.Gen.Acquisition.Tooling.FrameworkDetection;

internal sealed record CliFrameworkProvider(
    string Name,
    IReadOnlyList<string> LabelAliases,
    IReadOnlyList<string> DependencyIds,
    IReadOnlyList<string> PackageAssemblyNames,
    IReadOnlyList<string> RuntimeAssemblyNames,
    bool SupportsCliFxAnalysis,
    bool SupportsHookAnalysis,
    StaticAnalysisFrameworkAdapter? StaticAnalysisAdapter)
{
    public bool Matches(IReadOnlySet<string> dependencyIds, IReadOnlySet<string> assemblyNames)
        => DependencyIds.Any(dependencyIds.Contains) || PackageAssemblyNames.Any(assemblyNames.Contains);

    public IEnumerable<string> EnumerateLabels()
    {
        yield return Name;

        foreach (var alias in LabelAliases)
        {
            if (!string.IsNullOrWhiteSpace(alias))
            {
                yield return alias;
            }
        }
    }
}

/// <summary>
/// Type-erased carrier for a Static-mode attribute reader. The Registry keeps the reader
/// as <see cref="object"/> on purpose so that <c>Tooling/</c> has no compile-time
/// dependency on <c>Modes.Static.Attributes</c>.
///
/// <para>
/// The type erasure is intentional. A strongly-typed <c>IStaticAttributeReader</c> here
/// would require either (a) a <c>Tooling → Modes</c> dependency (forbidden by the
/// architecture charter) or (b) promoting <c>IStaticAttributeReader</c> into
/// <c>Contracts/</c>, which would in turn force a <c>Contracts → Modes</c> dependency
/// because the interface signature references <c>StaticCommandDefinition</c> and
/// <c>ScannedModule</c> (the latter wraps <c>dnlib.DotNet.ModuleDefMD</c> and therefore
/// cannot be cleanly promoted). <c>Contracts/</c> is the foundational layer and must
/// stay free of any <c>Modes.*</c> reference — a <c>Contracts → Modes</c> leak is
/// strictly worse than the current <c>object</c> erasure.
/// </para>
///
/// <para>
/// Consumers in Static mode cast <see cref="Reader"/> back to
/// <c>IStaticAttributeReader</c> at the single use site in
/// <c>StaticAnalysisAssemblyInspectionSupport</c>.
/// </para>
/// </summary>
internal sealed record StaticAnalysisFrameworkAdapter(
    string FrameworkName,
    string AssemblyName,
    object Reader);
