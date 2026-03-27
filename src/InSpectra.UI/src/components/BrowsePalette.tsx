import { DiscoveryPackageSummary } from "../data/nugetDiscovery";
import { SearchPalette, SearchPaletteItem } from "./SearchPalette";

interface BrowsePaletteProps {
  packages: DiscoveryPackageSummary[];
  open: boolean;
  onClose: () => void;
  onSelect: (packageId: string) => void;
}

export function BrowsePalette({ packages, open, onClose, onSelect }: BrowsePaletteProps) {
  const items: SearchPaletteItem[] = packages.map((pkg) => ({
    key: pkg.packageId,
    title: pkg.packageId,
    description: pkg.commandName,
  }));
  return (
    <SearchPalette
      items={items}
      open={open}
      onClose={onClose}
      onSelect={onSelect}
      placeholder="Search packages…"
      emptyText="No packages found."
      ariaLabel="Package search"
    />
  );
}
