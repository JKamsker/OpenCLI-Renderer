import { ViewerOptions } from "../boot/contracts";
import { createLoadedSource, LoadedSource } from "./loadSource";
import { downloadNugetPackage } from "./nugetTools";
import { ProbeDiagnostics, probePackage, toProbeDiagnostics, toProbeSummary } from "./toolProbe";

export interface NugetToolRequest {
  id: string;
  version: string;
}

export class NugetToolProbeError extends Error {
  readonly diagnostics: ProbeDiagnostics;

  constructor(message: string, diagnostics: ProbeDiagnostics) {
    super(message);
    this.name = "NugetToolProbeError";
    this.diagnostics = diagnostics;
  }
}

export async function loadFromNugetTool(request: NugetToolRequest, options: ViewerOptions): Promise<LoadedSource> {
  const packageBytes = await downloadNugetPackage(request.id, request.version);
  const result = await probePackage(packageBytes);
  if (!result.document) {
    throw new NugetToolProbeError(
      result.error ??
        "The package does not bundle opencli.json and could not be recovered from browser-side static Spectre.Console.Cli analysis.",
      toProbeDiagnostics(result),
    );
  }

  return createLoadedSource({
    document: result.document,
    options,
    label: `NuGet: ${result.package?.id ?? request.id} ${result.package?.version ?? request.version}`,
    mode: "generated",
    warnings: result.warnings,
    probeSummary: toProbeSummary(result),
  });
}
