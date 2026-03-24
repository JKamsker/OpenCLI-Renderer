import {
  Eye,
  EyeOff,
  FileUp,
  Search,
  Sidebar,
  Sparkles,
  TerminalSquare,
} from "lucide-react";
import { startTransition, useDeferredValue, useEffect, useRef, useState } from "react";
import { resolveStartupRequest } from "./boot/bootstrap";
import { defaultViewerOptions, ViewerOptions } from "./boot/contracts";
import { CommandPanel } from "./components/CommandPanel";
import { CommandTree } from "./components/CommandTree";
import { ImportScreen } from "./components/ImportScreen";
import { OverviewPanel } from "./components/OverviewPanel";
import { loadFromFiles, loadFromStartupRequest, LoadedSource } from "./data/loadSource";
import { buildCommandHash, parseHashRoute } from "./data/navigation";
import { findCommandByPath, normalizeOpenCliDocument, NormalizedCliDocument } from "./data/normalize";

interface LoadState {
  status: "loading" | "ready" | "empty";
  message?: string;
}

export function InSpectreApp() {
  const pickerRef = useRef<HTMLInputElement>(null);
  const [loadState, setLoadState] = useState<LoadState>({ status: "loading", message: "Resolving viewer boot mode." });
  const [error, setError] = useState<string | null>(null);
  const [warnings, setWarnings] = useState<string[]>([]);
  const [sourceLabel, setSourceLabel] = useState<string>("");
  const [viewerOptions, setViewerOptions] = useState<ViewerOptions>(defaultViewerOptions());
  const [document, setDocument] = useState<NormalizedCliDocument | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [routePath, setRoutePath] = useState<string | undefined>(parseHashRoute(window.location.hash).commandPath);
  const deferredSearch = useDeferredValue(searchTerm);

  useEffect(() => {
    const controller = new AbortController();
    void initialize(controller.signal);

    return () => controller.abort();
  }, []);

  useEffect(() => {
    function handleHashChange() {
      const route = parseHashRoute(window.location.hash);
      setRoutePath(route.commandPath);
    }

    window.addEventListener("hashchange", handleHashChange);
    return () => window.removeEventListener("hashchange", handleHashChange);
  }, []);

  useEffect(() => {
    if (!document) {
      return;
    }

    const route = parseHashRoute(window.location.hash);
    if (route.kind === "command" && !findCommandByPath(document.commands, route.commandPath)) {
      window.location.hash = "#/";
    }
  }, [document]);

  async function initialize(signal: AbortSignal) {
    try {
      const request = resolveStartupRequest({
        search: window.location.search,
        href: window.location.href,
      });
      const loaded = await loadFromStartupRequest(request, signal);

      if (!loaded) {
        setLoadState({ status: "empty" });
        return;
      }

      applyLoadedSource(loaded);
    } catch (loadError) {
      setError(toMessage(loadError));
      setLoadState({ status: "empty" });
    }
  }

  function applyLoadedSource(source: LoadedSource) {
    setWarnings(source.warnings);
    setSourceLabel(source.label);
    setViewerOptions(source.options);
    setDocument(normalizeOpenCliDocument(source.document, source.options.includeHidden));
    setSearchTerm("");
    setError(null);
    setLoadState({ status: "ready" });
  }

  async function handleFiles(files: File[]) {
    try {
      setLoadState({ status: "loading", message: "Importing local files." });
      const loaded = await loadFromFiles(files, viewerOptions);
      applyLoadedSource(loaded);
      window.location.hash = "#/";
    } catch (loadError) {
      setError(toMessage(loadError));
      setLoadState(document ? { status: "ready" } : { status: "empty" });
    }
  }

  function toggleOption(option: keyof ViewerOptions) {
    if (!document) {
      return;
    }

    startTransition(() => {
      setViewerOptions((current) => {
        const next = {
          ...current,
          [option]: !current[option],
        };

        setDocument(normalizeOpenCliDocument(document.source, next.includeHidden));
        return next;
      });
    });
  }

  if (loadState.status !== "ready" || !document) {
    return (
      <ImportScreen
        error={error}
        loading={loadState.status === "loading"}
        onFilesSelected={handleFiles}
      />
    );
  }

  const activeCommand = findCommandByPath(document.commands, routePath);

  return (
    <div className="app-shell">
      <header className="topbar">
        <div className="brand-block">
          <div className="brand-mark">
            <Sidebar aria-hidden="true" />
          </div>
          <div>
            <div className="brand-title">{document.source.info.title || "OpenCLI Viewer"}</div>
            <div className="brand-subtitle">
              <span>{sourceLabel}</span>
              <span>OpenCLI {document.source.opencli || "unknown"}</span>
              <span>{document.source.info.version || "0.0.0"}</span>
            </div>
          </div>
        </div>

        <div className="toolbar">
          <label className="search-field">
            <Search aria-hidden="true" />
            <input
              type="search"
              placeholder="Filter command tree"
              value={searchTerm}
              onChange={(event) => setSearchTerm(event.target.value)}
            />
          </label>

          <button type="button" className="toolbar-button" onClick={() => toggleOption("includeHidden")}>
            {viewerOptions.includeHidden ? <EyeOff aria-hidden="true" /> : <Eye aria-hidden="true" />}
            <span>{viewerOptions.includeHidden ? "Hide hidden" : "Show hidden"}</span>
          </button>

          <button type="button" className="toolbar-button" onClick={() => toggleOption("includeMetadata")}>
            <Sparkles aria-hidden="true" />
            <span>{viewerOptions.includeMetadata ? "Hide metadata" : "Show metadata"}</span>
          </button>

          <button type="button" className="toolbar-button" onClick={() => pickerRef.current?.click()}>
            <FileUp aria-hidden="true" />
            <span>Import files</span>
          </button>
          <input
            ref={pickerRef}
            className="visually-hidden"
            type="file"
            multiple
            accept=".json,.xml"
            onChange={(event) => void handleFiles(Array.from(event.target.files ?? []))}
          />
        </div>
      </header>

      <div className="app-grid">
        <aside className="sidebar panel">
          <div className="sidebar-heading">
            <TerminalSquare aria-hidden="true" />
            <h2>Command graph</h2>
          </div>
          <button
            type="button"
            className={`tree-row overview-row ${!activeCommand ? "selected" : ""}`}
            onClick={() => {
              window.location.hash = "#/";
            }}
          >
            <span className="tree-toggle">
              <Sidebar aria-hidden="true" />
            </span>
            <span className="tree-copy">
              <strong>Overview</strong>
              <span>Project summary, root arguments, and top-level commands</span>
            </span>
          </button>
          <CommandTree
            commands={document.commands}
            searchTerm={deferredSearch}
            selectedPath={activeCommand?.path}
            onSelect={(path) => {
              window.location.hash = buildCommandHash(path);
            }}
          />
        </aside>

        <main className="content-column">
          {warnings.length > 0 ? (
            <div className="warning-banner panel" role="status">
              {warnings.map((warning) => (
                <p key={warning}>{warning}</p>
              ))}
            </div>
          ) : null}

          {error ? (
            <div className="warning-banner panel" role="alert">
              <p>{error}</p>
            </div>
          ) : null}

          {activeCommand ? (
            <CommandPanel
              command={activeCommand}
              includeMetadata={viewerOptions.includeMetadata}
              onCommandSelect={(path) => {
                window.location.hash = buildCommandHash(path);
              }}
            />
          ) : (
            <OverviewPanel
              document={document}
              includeMetadata={viewerOptions.includeMetadata}
              onCommandSelect={(path) => {
                window.location.hash = buildCommandHash(path);
              }}
            />
          )}
        </main>
      </div>
    </div>
  );
}

function toMessage(error: unknown): string {
  return error instanceof Error ? error.message : "Unknown error.";
}
