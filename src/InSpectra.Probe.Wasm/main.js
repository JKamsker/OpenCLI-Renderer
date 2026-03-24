import { dotnet } from "./_framework/dotnet.js";

const runtime = await dotnet.withDiagnosticTracing(false).create();
const config = runtime.getConfig();
const exports = await runtime.getAssemblyExports(config.mainAssemblyName);

await dotnet.run();

export function analyzePackage(base64Package) {
  return exports.InSpectra.Probe.Wasm.ProbeExports.AnalyzePackage(base64Package);
}
