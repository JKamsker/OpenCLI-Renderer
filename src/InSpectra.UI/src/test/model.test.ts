import { normalizeOpenCliDocument } from "../data/normalize";
import { cloneOpenCliDocument } from "../data/openCli";
import type { OpenCliDocument } from "../data/openCli";
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

  it("treats a hidden __default_command as implicit root surface", () => {
    const document: OpenCliDocument = {
      opencli: "0.1-draft",
      info: { title: "nupu", version: "1.0.50" },
      arguments: [],
      options: [],
      commands: [
        {
          name: "__default_command",
          aliases: [],
          options: [
            {
              name: "--directory",
              required: false,
              aliases: ["-d"],
              arguments: [],
              description: "A root directory to search.",
              recursive: false,
              hidden: false,
              metadata: [],
            },
          ],
          arguments: [],
          commands: [],
          exitCodes: [],
          description: null,
          hidden: true,
          examples: [],
          interactive: false,
          metadata: [],
        },
      ],
      exitCodes: [],
      examples: [],
      interactive: false,
      metadata: [],
    };

    const normalized = normalizeOpenCliDocument(document, false);

    expect(normalized.rootOptions.map((option) => option.name)).toEqual(["--directory"]);
    expect(normalized.commands).toEqual([]);
  });
});
