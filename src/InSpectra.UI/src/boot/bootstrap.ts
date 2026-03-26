import { toMessage } from "../utils";
import { defaultFeatureFlags, defaultViewerOptions, disabledFeatureFlags, FeatureFlags, InSpectraBootstrap, ViewerOptions } from "./contracts";
import { resolveViewerLinks, resolveViewerLinksFromSearch, ViewerLinks } from "./urlParams";

export type StartupRequest =
  | {
      kind: "inline";
      openCli: unknown;
      xmlDoc?: string;
      options: ViewerOptions;
      features: FeatureFlags;
    }
  | {
      kind: "links";
      links: ViewerLinks;
      options: ViewerOptions;
      features: FeatureFlags;
      source: "bootstrap" | "query";
    }
  | {
      kind: "empty";
      features: FeatureFlags;
    };

export function readInjectedBootstrap(documentRef: Document = document): InSpectraBootstrap | null {
  const element = documentRef.getElementById("inspectra-bootstrap");
  const payload = element?.textContent?.trim();

  if (!payload || payload === "__INSPECTRA_BOOTSTRAP__") {
    return null;
  }

  try {
    return JSON.parse(payload) as InSpectraBootstrap;
  } catch (error) {
    throw new Error(`Injected viewer bootstrap is not valid JSON: ${toMessage(error)}`);
  }
}

export function resolveStartupRequest(params: {
  documentRef?: Document;
  search: string;
  href: string;
}): StartupRequest {
  const bootstrap = readInjectedBootstrap(params.documentRef);

  if (bootstrap?.mode === "inline") {
    const features = readFeatures(bootstrap.features, true);
    return {
      kind: "inline",
      openCli: bootstrap.openCli,
      xmlDoc: bootstrap.xmlDoc,
      options: readOptions(bootstrap.options),
      features,
    };
  }

  if (bootstrap?.mode === "links") {
    const links = resolveViewerLinks(bootstrap, params.href);
    if (!links) {
      throw new Error("Injected links bootstrap must provide openCliUrl or directoryUrl.");
    }

    const features = readFeatures(bootstrap.features, true);
    return {
      kind: "links",
      links,
      options: readOptions(bootstrap.options),
      features,
      source: "bootstrap",
    };
  }

  const features = readFeatures(undefined, false);

  if (features.urlLoading) {
    const queryLinks = resolveViewerLinksFromSearch(params.search, params.href);
    if (queryLinks) {
      return {
        kind: "links",
        links: queryLinks,
        options: defaultViewerOptions(),
        features,
        source: "query",
      };
    }
  }

  return {
    kind: "empty",
    features,
  };
}

function readOptions(options: Partial<ViewerOptions> | ViewerOptions | undefined): ViewerOptions {
  const defaults = defaultViewerOptions();
  return {
    includeHidden: options?.includeHidden ?? defaults.includeHidden,
    includeMetadata: options?.includeMetadata ?? defaults.includeMetadata,
  };
}

function readFeatures(features: Partial<FeatureFlags> | undefined, hasBootstrap: boolean): FeatureFlags {
  if (!hasBootstrap) {
    return defaultFeatureFlags();
  }

  const defaults = features ? defaultFeatureFlags() : disabledFeatureFlags();
  return {
    showHome: features?.showHome ?? defaults.showHome,
    composer: features?.composer ?? defaults.composer,
    darkTheme: features?.darkTheme ?? defaults.darkTheme,
    lightTheme: features?.lightTheme ?? defaults.lightTheme,
    urlLoading: features?.urlLoading ?? defaults.urlLoading,
    nugetBrowser: features?.nugetBrowser ?? defaults.nugetBrowser,
    packageUpload: features?.packageUpload ?? defaults.packageUpload,
  };
}

