import { afterEach, describe, expect, it } from "vitest";
import {
  ToolProbeUnavailableError,
  probePackage,
  setProbeModuleLoaderForTests,
  toProbeDiagnostics,
} from "../data/toolProbe";

describe("toolProbe", () => {
  afterEach(() => {
    setProbeModuleLoaderForTests(null);
  });

  it("wraps missing probe assets in a friendly browser probe error", async () => {
    setProbeModuleLoaderForTests(async () => {
      throw new Error("missing chunk");
    });

    await expect(probePackage(new Uint8Array([1, 2, 3]))).rejects.toBeInstanceOf(ToolProbeUnavailableError);
  });

  it("normalizes probe diagnostics from the package payload", () => {
    const diagnostics = toProbeDiagnostics({
      status: "unsupported",
      confidence: "unsupported",
      error: "No document",
      warnings: ["No tool code was executed."],
      package: {
        id: "Demo.Tool",
        version: "1.2.3",
        isDotnetTool: true,
        isSpectreCli: false,
        commandName: "demo",
        runner: "dotnet",
        entryPoint: "demo.dll",
        targetFramework: "net10.0",
        hasPackagedOpenCli: false,
        documentSource: "none",
      },
    });

    expect(diagnostics.summary?.id).toBe("Demo.Tool");
    expect(diagnostics.summary?.documentSource).toBe("none");
    expect(diagnostics.warnings).toEqual(["No tool code was executed."]);
  });
});
