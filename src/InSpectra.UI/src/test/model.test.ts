import { normalizeOpenCliDocument } from "../data/normalize";
import { cloneOpenCliDocument } from "../data/openCli";
import { enrichDocumentFromXml } from "../data/xmlDoc";
import { testDocument, testXmlDoc } from "./fixtures";

describe("OpenCLI normalization and XML enrichment", () => {
  it("fills only missing descriptions from XML", () => {
    const document = cloneOpenCliDocument(testDocument);
    const alpha = document.commands[0];
    alpha.arguments[0].description = "Keep JSON description.";

    const result = enrichDocumentFromXml(document, testXmlDoc);

    expect(result.matchedCommandCount).toBeGreaterThan(0);
    expect(alpha.description).toBe("Filled from XML.");
    expect(alpha.arguments[0].description).toBe("Keep JSON description.");
    expect(alpha.commands[0].description).toBe("Leaf command.");
  });

  it("filters hidden items and resolves recursive inheritance", () => {
    const normalized = normalizeOpenCliDocument(testDocument, false);
    const alpha = normalized.commands[0];
    const leaf = alpha.commands[0];

    expect(normalized.commands.map((command) => command.path)).toEqual(["alpha"]);
    expect(alpha.commands.map((command) => command.path)).toEqual(["alpha leaf"]);
    expect(alpha.inheritedOptions.map((option) => option.option.name)).toEqual(["--profile"]);
    expect(leaf.inheritedOptions.map((option) => option.option.name)).toEqual(["--profile", "--trace"]);
  });
});
