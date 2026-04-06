import { FileText, Menu, Package, Terminal, Upload, X, Zap } from "lucide-react";
import { useRef, useState } from "react";
import { ThemeToggle } from "./ThemeToggle";
import { GitHubIcon } from "./GitHubIcon";
import { HashRoute } from "../data/navigation";

interface SiteHeaderProps {
  route: HashRoute;
  onFilesSelected: (files: File[]) => void;
}

export function SiteHeader({ route, onFilesSelected }: SiteHeaderProps) {
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [mobileOpen, setMobileOpen] = useState(false);

  function handleImportClick() {
    fileInputRef.current?.click();
  }

  function handleFileChange(e: React.ChangeEvent<HTMLInputElement>) {
    if (e.target.files) {
      onFilesSelected(Array.from(e.target.files));
      e.target.value = "";
    }
  }

  const isHome = route.kind === "overview" || route.kind === "browse";
  const isAbout = route.kind === "about";
  const isGuide = route.kind === "guide";
  const isCli = route.kind === "package" && route.packageId.toLowerCase() === "inspectra.gen";

  return (
    <>
      <header className="site-header">
        <a href="#/" className="site-header-brand">
          <span className="brand-mark">{">_"}</span>
          <span className="site-header-brand-name">InSpectra</span>
        </a>

        <nav className="site-header-nav">
          <a href="#/" className={isHome ? "active" : ""}>
            <Package aria-hidden="true" size={15} />
            <span>Browse</span>
          </a>
          <a href="#/pkg/InSpectra.Gen" className={isCli ? "active" : ""}>
            <Terminal aria-hidden="true" size={15} />
            <span>CLI Reference</span>
          </a>
          <a href="#/guide" className={isGuide ? "active" : ""}>
            <FileText aria-hidden="true" size={15} />
            <span>CI Guide</span>
          </a>
          <a href="#/about" className={isAbout ? "active" : ""}>
            <Zap aria-hidden="true" size={15} />
            <span>Quickstart</span>
          </a>
          <a href="https://github.com/JKamsker/InSpectra" target="_blank" rel="noopener noreferrer">
            <GitHubIcon aria-hidden="true" size={15} />
            <span>GitHub</span>
          </a>
        </nav>

        <div className="site-header-actions">
          <button
            type="button"
            className="site-header-import-btn"
            onClick={handleImportClick}
            title="Import OpenCLI files"
          >
            <Upload aria-hidden="true" />
            <span>Import</span>
          </button>
          <input
            ref={fileInputRef}
            type="file"
            className="site-header-file-input"
            multiple
            accept=".json,.xml"
            onChange={handleFileChange}
          />
          <ThemeToggle />
          <button
            type="button"
            className="site-header-mobile-btn"
            onClick={() => setMobileOpen((o) => !o)}
            aria-label={mobileOpen ? "Close menu" : "Open menu"}
          >
            {mobileOpen ? <X aria-hidden="true" /> : <Menu aria-hidden="true" />}
          </button>
        </div>
      </header>

      <nav className={`site-header-mobile-nav${mobileOpen ? " open" : ""}`}>
        <a href="#/" onClick={() => setMobileOpen(false)} className={isHome ? "active" : ""}>
          <Package aria-hidden="true" />
          <span>Browse</span>
        </a>
        <a href="#/pkg/InSpectra.Gen" onClick={() => setMobileOpen(false)} className={isCli ? "active" : ""}>
          <Terminal aria-hidden="true" />
          <span>CLI Reference</span>
        </a>
        <a href="#/guide" onClick={() => setMobileOpen(false)} className={isGuide ? "active" : ""}>
          <FileText aria-hidden="true" />
          <span>CI Guide</span>
        </a>
        <a href="#/about" onClick={() => setMobileOpen(false)} className={isAbout ? "active" : ""}>
          <Zap aria-hidden="true" />
          <span>Quickstart</span>
        </a>
        <div className="site-header-divider" />
        <a href="https://github.com/JKamsker/InSpectra" target="_blank" rel="noopener noreferrer" onClick={() => setMobileOpen(false)}>
          <GitHubIcon aria-hidden="true" size={18} />
          <span>GitHub</span>
        </a>
        <button type="button" onClick={() => { handleImportClick(); setMobileOpen(false); }}>
          <Upload aria-hidden="true" />
          <span>Import OpenCLI files</span>
        </button>
      </nav>
    </>
  );
}
