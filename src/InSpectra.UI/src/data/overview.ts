import { NormalizedCliDocument, NormalizedCommand } from "./normalize";

const areaLabels = new Map<string, string>([
  ["auth", "authentication"],
  ["items", "items"],
  ["library", "library management"],
  ["server", "server administration"],
  ["users", "users"],
  ["playlists", "playlists"],
  ["sessions", "sessions"],
  ["plugins", "plugins"],
  ["packages", "packages"],
  ["livetv", "Live TV"],
  ["syncplay", "SyncPlay"],
  ["raw", "raw endpoint access"],
  ["devices", "devices"],
  ["collections", "collections"],
  ["backups", "backups"],
  ["artists", "artists"],
  ["genres", "genres"],
  ["persons", "people"],
  ["studios", "studios"],
  ["tasks", "scheduled tasks"],
]);

const areaPriority = new Map<string, number>([
  ["auth", 0],
  ["items", 1],
  ["library", 2],
  ["server", 3],
  ["users", 4],
  ["playlists", 5],
  ["sessions", 6],
  ["plugins", 7],
  ["packages", 8],
  ["livetv", 9],
  ["syncplay", 10],
  ["raw", 11],
]);

export function buildSummary(document: NormalizedCliDocument): string | undefined {
  if (document.source.info.summary?.trim()) {
    return document.source.info.summary;
  }

  const commandCount = countCommands(document.commands);
  if (commandCount === 0) {
    return undefined;
  }

  const areas = buildAreaLabels(document.commands).slice(0, 6);
  if (isLikelyJellyfin(document)) {
    return joinSentences(
      "Manage your Jellyfin server from the command line.",
      buildAreaSentence(areas, document.commands.length > areas.length),
    );
  }

  const title = document.source.info.title.trim()
    ? `Command-line reference for ${document.source.info.title}.`
    : "Command-line reference.";

  return joinSentences(title, buildAreaSentence(areas, document.commands.length > areas.length));
}

export function buildFacts(document: NormalizedCliDocument): Array<[string, string]> {
  const commandCount = countCommands(document.commands);
  if (commandCount === 0) {
    return [];
  }

  const facts: Array<[string, string]> = [
    ["Top-level command groups", `${document.commands.length}`],
    ["Documented commands", `${commandCount}`],
  ];

  const leafCount = countLeafCommands(document.commands);
  if (leafCount !== commandCount) {
    facts.push(["Leaf commands", `${leafCount}`]);
  }

  if (document.source.examples.length > 0) {
    facts.push(["Quick-start examples", `${document.source.examples.length}`]);
  }

  return facts;
}

function buildAreaLabels(commands: NormalizedCommand[]): string[] {
  return commands
    .map((command, index) => ({
      label: areaLabels.get(command.command.name.toLowerCase()) ?? command.command.name.replaceAll("-", " "),
      priority: areaPriority.get(command.command.name.toLowerCase()) ?? 100,
      weight: countCommands(command.commands),
      index,
    }))
    .sort((left, right) => {
      if (left.priority !== right.priority) {
        return left.priority - right.priority;
      }

      if (left.weight !== right.weight) {
        return right.weight - left.weight;
      }

      return left.index - right.index;
    })
    .map((item) => item.label);
}

function buildAreaSentence(areas: string[], truncated: boolean): string {
  if (areas.length === 0) {
    return "";
  }

  return truncated
    ? `Available command areas include ${areas.join(", ")}, and more.`
    : `Available command areas include ${formatHumanList(areas)}.`;
}

function formatHumanList(items: string[]): string {
  if (items.length === 1) {
    return items[0];
  }

  if (items.length === 2) {
    return `${items[0]} and ${items[1]}`;
  }

  return `${items.slice(0, -1).join(", ")}, and ${items.at(-1)}`;
}

function isLikelyJellyfin(document: NormalizedCliDocument): boolean {
  return enumerateText(document).some((text) => text.toLowerCase().includes("jellyfin"));
}

function enumerateText(document: NormalizedCliDocument): string[] {
  const values = [document.source.info.title, document.source.info.description ?? ""];
  return values.concat(document.commands.flatMap((command) => enumerateCommandText(command)));
}

function enumerateCommandText(command: NormalizedCommand): string[] {
  return [
    command.command.name,
    command.command.description ?? "",
    ...command.commands.flatMap((child) => enumerateCommandText(child)),
  ];
}

function joinSentences(lead: string, tail: string): string {
  return tail ? `${lead} ${tail}` : lead;
}

export function countCommands(commands: NormalizedCommand[]): number {
  return commands.reduce((total, command) => total + 1 + countCommands(command.commands), 0);
}

function countLeafCommands(commands: NormalizedCommand[]): number {
  return commands.reduce(
    (total, command) => total + (command.commands.length === 0 ? 1 : countLeafCommands(command.commands)),
    0,
  );
}
