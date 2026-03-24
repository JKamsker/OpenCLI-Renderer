import {
  OpenCliArgument,
  OpenCliCommand,
  OpenCliDocument,
  OpenCliOption,
  isBlank,
} from "./openCli";

interface XmlCommandInfo {
  description?: string | null;
  parameters: XmlParameterInfo[];
}

interface XmlParameterInfo {
  kind: string;
  longName?: string | null;
  shortName?: string | null;
  name?: string | null;
  description?: string | null;
}

export interface XmlEnrichmentResult {
  matchedCommandCount: number;
  enrichedDescriptionCount: number;
  warnings: string[];
}

export function enrichDocumentFromXml(
  document: OpenCliDocument,
  xml: string,
  sourceLabel = "xmldoc.xml",
): XmlEnrichmentResult {
  const parsed = parseXml(xml, sourceLabel);
  const root = parsed.documentElement;

  if (root.tagName !== "Model") {
    throw new Error(`XML enrichment source "${sourceLabel}" does not contain a <Model> root element.`);
  }

  const index = new Map<string, XmlCommandInfo>();
  for (const element of Array.from(root.children)) {
    if (element.tagName === "Command") {
      indexCommand(element, undefined, index);
    }
  }

  const summary: XmlEnrichmentResult = {
    matchedCommandCount: 0,
    enrichedDescriptionCount: 0,
    warnings: [],
  };

  for (const command of document.commands) {
    enrichCommand(command, undefined, index, summary);
  }

  if (summary.matchedCommandCount === 0) {
    summary.warnings.push(`No XML command descriptions from "${sourceLabel}" matched the OpenCLI document.`);
  }

  return summary;
}

function parseXml(xml: string, sourceLabel: string): XMLDocument {
  const parsed = new DOMParser().parseFromString(xml, "application/xml");
  const errorNode = parsed.querySelector("parsererror");

  if (errorNode) {
    throw new Error(`XML enrichment source "${sourceLabel}" is not valid XML.`);
  }

  return parsed;
}

function indexCommand(element: Element, parentPath: string | undefined, index: Map<string, XmlCommandInfo>): void {
  const name = element.getAttribute("Name")?.trim();
  if (!name) {
    return;
  }

  const path = parentPath ? `${parentPath} ${name}` : name;
  const parameters = readParameters(element);

  index.set(path, {
    description: normalizeText(element.querySelector(":scope > Description")?.textContent),
    parameters,
  });

  for (const child of Array.from(element.children)) {
    if (child.tagName === "Command") {
      indexCommand(child, path, index);
    }
  }
}

function readParameters(element: Element): XmlParameterInfo[] {
  const parametersRoot = Array.from(element.children).find((child) => child.tagName === "Parameters");
  if (!parametersRoot) {
    return [];
  }

  return Array.from(parametersRoot.children).map((parameter) => ({
    kind: parameter.tagName,
    longName: parameter.getAttribute("Long"),
    shortName: parameter.getAttribute("Short"),
    name: parameter.getAttribute("Name"),
    description: normalizeText(parameter.querySelector(":scope > Description")?.textContent),
  }));
}

function enrichCommand(
  command: OpenCliCommand,
  parentPath: string | undefined,
  index: ReadonlyMap<string, XmlCommandInfo>,
  summary: XmlEnrichmentResult,
): void {
  const path = parentPath ? `${parentPath} ${command.name}` : command.name;
  const xmlCommand = index.get(path);

  if (xmlCommand) {
    summary.matchedCommandCount += 1;

    if (isBlank(command.description) && !isBlank(xmlCommand.description)) {
      command.description = xmlCommand.description;
      summary.enrichedDescriptionCount += 1;
    }

    for (const option of command.options) {
      const match = matchOption(option, xmlCommand.parameters);
      if (match && isBlank(option.description) && !isBlank(match.description)) {
        option.description = match.description;
        summary.enrichedDescriptionCount += 1;
      }
    }

    for (const argument of command.arguments) {
      const match = matchArgument(argument, xmlCommand.parameters);
      if (match && isBlank(argument.description) && !isBlank(match.description)) {
        argument.description = match.description;
        summary.enrichedDescriptionCount += 1;
      }
    }
  }

  for (const child of command.commands) {
    enrichCommand(child, path, index, summary);
  }
}

function matchOption(option: OpenCliOption, parameters: XmlParameterInfo[]): XmlParameterInfo | undefined {
  const longName = option.name.replace(/^-+/, "");
  return parameters.find((parameter) => {
    if (parameter.kind.toLowerCase() !== "option") {
      return false;
    }

    if (equalsIgnoreCase(parameter.longName, longName)) {
      return true;
    }

    return option.aliases.some((alias) => equalsIgnoreCase(alias.replace(/^-+/, ""), parameter.shortName));
  });
}

function matchArgument(argument: OpenCliArgument, parameters: XmlParameterInfo[]): XmlParameterInfo | undefined {
  return parameters.find(
    (parameter) =>
      parameter.kind.toLowerCase() === "argument" &&
      parameter.name === argument.name,
  );
}

function normalizeText(value: string | null | undefined): string | undefined {
  if (isBlank(value)) {
    return undefined;
  }

  const normalized = value ?? "";
  return normalized
    .split(/\s+/)
    .filter((part) => part.length > 0)
    .join(" ");
}

function equalsIgnoreCase(left: string | null | undefined, right: string | null | undefined): boolean {
  return left?.toLowerCase() === right?.toLowerCase();
}
