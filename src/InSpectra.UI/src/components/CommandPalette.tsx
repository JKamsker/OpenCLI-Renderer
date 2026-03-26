import { collectCommandPaths, findCommandByPath, NormalizedCommand } from "../data/normalize";
import { SearchPalette, SearchPaletteItem } from "./SearchPalette";

interface CommandPaletteProps {
  commands: NormalizedCommand[];
  open: boolean;
  onClose: () => void;
  onSelect: (path: string) => void;
}

export function CommandPalette({ commands, open, onClose, onSelect }: CommandPaletteProps) {
  const items = buildIndex(commands);
  return (
    <SearchPalette
      items={items}
      open={open}
      onClose={onClose}
      onSelect={onSelect}
      placeholder="Search commands…"
      emptyText="No commands found."
      ariaLabel="Command palette"
    />
  );
}

function buildIndex(commands: NormalizedCommand[]): SearchPaletteItem[] {
  return collectCommandPaths(commands).map((path) => {
    const cmd = findCommandByPath(commands, path);
    return { key: path, title: path, description: cmd?.command.description ?? "" };
  });
}
