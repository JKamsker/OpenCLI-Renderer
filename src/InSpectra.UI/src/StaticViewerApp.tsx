import { useEffect, useState } from "react";
import { CliViewer } from "./components/CliViewer";
import { PackageLoadingScreen } from "./components/PackageLoadingScreen";
import { resolveStartupRequest } from "./boot/bootstrap";
import { defaultFeatureFlags, defaultViewerOptions, FeatureFlags, ViewerOptions } from "./boot/contracts";
import { useThemeEnforcement } from "./hooks/useThemeEnforcement";
import { loadFromStartupRequest, LoadedSource } from "./data/loadSource";
import { buildCommandHash, HashRoute, parseHashRoute } from "./data/navigation";
import { normalizeOpenCliDocument, NormalizedCliDocument } from "./data/normalize";
import { toMessage } from "./utils";

export function StaticViewerApp() {
  const [loadState, setLoadState] = useState<{ status: "loading" | "ready" | "error"; message?: string }>({
    status: "loading",
    message: "Resolving viewer boot mode.",
  });
  const [error, setError] = useState<string | null>(null);
  const [warnings, setWarnings] = useState<string[]>([]);
  const [sourceLabel, setSourceLabel] = useState("");
  const [viewerOptions, setViewerOptions] = useState<ViewerOptions>(defaultViewerOptions());
  const [featureFlags, setFeatureFlags] = useState<FeatureFlags>(defaultFeatureFlags());
  const [document, setDocument] = useState<NormalizedCliDocument | null>(null);
  const [route, setRoute] = useState<HashRoute>(() => parseHashRoute(window.location.hash));

  useEffect(() => {
    const controller = new AbortController();
    void initialize(controller.signal);
    return () => controller.abort();
  }, []);

  useEffect(() => {
    const handle = () => setRoute(parseHashRoute(window.location.hash));
    window.addEventListener("hashchange", handle);
    return () => window.removeEventListener("hashchange", handle);
  }, []);

  useThemeEnforcement(featureFlags, viewerOptions);

  async function initialize(signal: AbortSignal) {
    try {
      const request = await resolveStartupRequest({ search: window.location.search, href: window.location.href });
      setFeatureFlags(request.features);
      const loaded = await loadFromStartupRequest(request, signal);
      if (loaded) {
        applyLoadedSource(loaded);
        return;
      }
      setLoadState({ status: "error", message: "No bootstrap data found." });
    } catch (err) {
      setError(toMessage(err));
      setLoadState({ status: "error" });
    }
  }

  function applyLoadedSource(source: LoadedSource) {
    setWarnings(source.warnings);
    setSourceLabel(source.label);
    setViewerOptions(source.options);
    setFeatureFlags(source.features);
    setDocument(normalizeOpenCliDocument(source.document, source.options.includeHidden));
    setError(null);
    setLoadState({ status: "ready" });
  }

  if (loadState.status !== "ready" || !document) {
    if (loadState.status === "loading") {
      return <PackageLoadingScreen message={loadState.message} />;
    }
    return (
      <main className="ds-content-screen">
        <section className="ds-hero-panel panel">
          <div className="eyebrow">InSpectraUI</div>
          <h1>Failed to load</h1>
          {error && <p className="ds-inline-alert" role="alert">{error}</p>}
          {loadState.message && <p className="ds-inline-alert" role="alert">{loadState.message}</p>}
        </section>
      </main>
    );
  }

  const commandPath = route.kind === "command" ? route.commandPath : undefined;

  function handleNavigate(path?: string) {
    window.location.hash = path ? buildCommandHash(path) : "#/";
  }

  return (
    <CliViewer
      document={document}
      viewerOptions={viewerOptions}
      featureFlags={featureFlags}
      packageContext={null}
      sourceLabel={sourceLabel}
      warnings={warnings}
      error={error}
      commandPath={commandPath}
      onNavigate={handleNavigate}
    />
  );
}
