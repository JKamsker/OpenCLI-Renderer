namespace InSpectra.Gen.Acquisition.Modes.Static.Attributes;

using InSpectra.Gen.Acquisition.Modes.Static.Models;

using InSpectra.Gen.Acquisition.Modes.Static.Inspection;

internal interface IStaticAttributeReader
{
    IReadOnlyDictionary<string, StaticCommandDefinition> Read(IReadOnlyList<ScannedModule> modules);
}

