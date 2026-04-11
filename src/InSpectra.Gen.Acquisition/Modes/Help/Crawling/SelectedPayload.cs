namespace InSpectra.Gen.Acquisition.Modes.Help.Crawling;

using InSpectra.Gen.Acquisition.Modes.Help.Documents;


internal sealed record SelectedPayload(
    Document? Document,
    string? Payload,
    bool IsTerminalNonHelp,
    string? GuardrailFailureMessage = null);
