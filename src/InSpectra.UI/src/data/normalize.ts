import {
  OpenCliArgument,
  OpenCliDocument,
  OpenCliMetadata,
  OpenCliOption,
} from "./openCli";

export interface NormalizedCliDocument {
  source: OpenCliDocument;
  rootArguments: OpenCliArgument[];
  rootOptions: OpenCliOption[];
  commands: NormalizedCommand[];
}

export interface NormalizedCommand {
  path: string;
  command: import("./openCli").OpenCliCommand;
  arguments: OpenCliArgument[];
  declaredOptions: OpenCliOption[];
  inheritedOptions: ResolvedOption[];
  commands: NormalizedCommand[];
}

export interface ResolvedOption {
  option: OpenCliOption;
  isInherited: boolean;
  inheritedFromPath?: string;
}

interface InheritedOption {
  option: OpenCliOption;
  sourcePath: string;
}

export function normalizeOpenCliDocument(document: OpenCliDocument, includeHidden: boolean): NormalizedCliDocument {
  const implicitRoot = splitImplicitRootCommand(document.commands);
  const rootArguments = mergeByName(
    document.arguments.filter((argument) => includeHidden || !argument.hidden),
    implicitRoot.command?.arguments.filter((argument) => includeHidden || !argument.hidden) ?? [],
  );
  const rootOptions = mergeByName(
    document.options.filter((option) => includeHidden || !option.hidden),
    implicitRoot.command?.options.filter((option) => includeHidden || !option.hidden) ?? [],
  );
  const inherited = rootOptions
    .filter((option) => option.recursive)
    .map((option) => ({ option, sourcePath: "<root>" }));

  return {
    source: document,
    rootArguments,
    rootOptions,
    commands: [
      ...implicitRoot.command?.commands
        .filter((command) => includeHidden || !command.hidden)
        .map((command) => normalizeCommand(command, undefined, inherited, includeHidden)) ?? [],
      ...implicitRoot.commands
      .filter((command) => includeHidden || !command.hidden)
      .map((command) => normalizeCommand(command, undefined, inherited, includeHidden)),
    ],
  };
}

export function collectCommandPaths(commands: NormalizedCommand[]): string[] {
  return commands.flatMap((command) => [command.path, ...collectCommandPaths(command.commands)]);
}

export function findCommandByPath(
  commands: NormalizedCommand[],
  path: string | null | undefined,
): NormalizedCommand | undefined {
  if (!path) {
    return undefined;
  }

  for (const command of commands) {
    if (command.path === path) {
      return command;
    }

    const nested = findCommandByPath(command.commands, path);
    if (nested) {
      return nested;
    }
  }

  return undefined;
}

export function getMetadataValue(metadata: OpenCliMetadata[], name: string): string | undefined {
  const match = metadata.find((item) => item.name.toLowerCase() === name.toLowerCase());
  return typeof match?.value === "string" ? match.value : undefined;
}

export function formatArity(argument: OpenCliArgument): string {
  const minimum = argument.arity?.minimum ?? (argument.required ? 1 : 0);
  const maximum = argument.arity?.maximum;

  if (maximum === undefined || maximum === null) {
    return `${minimum}..n`;
  }

  return minimum === maximum ? `${minimum}` : `${minimum}..${maximum}`;
}

export function formatOptionValue(option: OpenCliOption): string {
  if (option.arguments.length === 0) {
    return "flag";
  }

  return option.arguments
    .map((argument) => {
      const maximum = argument.arity?.maximum;
      return maximum === undefined || maximum === null || maximum > 1
        ? `<${argument.name}...>`
        : `<${argument.name}>`;
    })
    .join(" ");
}

function normalizeCommand(
  command: import("./openCli").OpenCliCommand,
  parentPath: string | undefined,
  inheritedOptions: InheritedOption[],
  includeHidden: boolean,
): NormalizedCommand {
  const path = parentPath ? `${parentPath} ${command.name}` : command.name;
  const argumentsList = command.arguments.filter((argument) => includeHidden || !argument.hidden);
  const declaredOptions = command.options.filter((option) => includeHidden || !option.hidden);
  const resolvedInheritedOptions = resolveInheritedOptions(inheritedOptions, declaredOptions);
  const nextInherited = [
    ...inheritedOptions,
    ...declaredOptions
      .filter((option) => option.recursive)
      .map((option) => ({ option, sourcePath: path })),
  ];

  return {
    path,
    command,
    arguments: argumentsList,
    declaredOptions,
    inheritedOptions: resolvedInheritedOptions,
    commands: command.commands
      .filter((child) => includeHidden || !child.hidden)
      .map((child) => normalizeCommand(child, path, nextInherited, includeHidden)),
  };
}

function resolveInheritedOptions(
  inheritedOptions: InheritedOption[],
  declaredOptions: OpenCliOption[],
): ResolvedOption[] {
  const seen = new Set(declaredOptions.map((option) => option.name.toLowerCase()));
  const buffer: ResolvedOption[] = [];

  for (let index = inheritedOptions.length - 1; index >= 0; index -= 1) {
    const inherited = inheritedOptions[index];
    const key = inherited.option.name.toLowerCase();
    if (seen.has(key)) {
      continue;
    }

    seen.add(key);
    buffer.push({
      option: inherited.option,
      isInherited: true,
      inheritedFromPath: inherited.sourcePath,
    });
  }

  return buffer.reverse();
}

function splitImplicitRootCommand(commands: import("./openCli").OpenCliCommand[]) {
  const command = commands.find((candidate) => candidate.name === "__default_command" && candidate.hidden);

  return {
    command,
    commands: command ? commands.filter((candidate) => candidate !== command) : commands,
  };
}

function mergeByName<T extends { name: string }>(primary: T[], secondary: T[]): T[] {
  const merged = [...primary];
  const seen = new Set(primary.map((item) => item.name.toLowerCase()));

  for (const item of secondary) {
    const key = item.name.toLowerCase();
    if (seen.has(key)) {
      continue;
    }

    seen.add(key);
    merged.push(item);
  }

  return merged;
}
