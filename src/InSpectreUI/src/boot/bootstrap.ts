import { defaultViewerOptions, InSpectreBootstrap, ViewerOptions } from "./contracts";
import { resolveViewerLinks, resolveViewerLinksFromSearch, ViewerLinks } from "./urlParams";

export type StartupRequest =
  | {
      kind: "inline";
      openCli: unknown;
      xmlDoc?: string;
      options: ViewerOptions;
    }
  | {
      kind: "links";
      links: ViewerLinks;
      options: ViewerOptions;
      source: "bootstrap" | "query";
    }
  | {
      kind: "empty";
    };

export function readInjectedBootstrap(documentRef: Document = document): InSpectreBootstrap | null {
  const element = documentRef.getElementById("inspectre-bootstrap");
  const payload = element?.textContent?.trim();

  if (!payload || payload === "__INSPECTRE_BOOTSTRAP__") {
    return null;
  }

  try {
    return JSON.parse(payload) as InSpectreBootstrap;
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
    return {
      kind: "inline",
      openCli: bootstrap.openCli,
      xmlDoc: bootstrap.xmlDoc,
      options: readOptions(bootstrap.options),
    };
  }

  if (bootstrap?.mode === "links") {
    const links = resolveViewerLinks(bootstrap, params.href);
    if (!links) {
      throw new Error("Injected links bootstrap must provide openCliUrl or directoryUrl.");
    }

    return {
      kind: "links",
      links,
      options: readOptions(bootstrap.options),
      source: "bootstrap",
    };
  }

  const queryLinks = resolveViewerLinksFromSearch(params.search, params.href);
  if (queryLinks) {
    return {
      kind: "links",
      links: queryLinks,
      options: defaultViewerOptions(),
      source: "query",
    };
  }

  return {
    kind: "empty",
  };
}

function readOptions(options: Partial<ViewerOptions> | ViewerOptions | undefined): ViewerOptions {
  const defaults = defaultViewerOptions();
  return {
    includeHidden: options?.includeHidden ?? defaults.includeHidden,
    includeMetadata: options?.includeMetadata ?? defaults.includeMetadata,
  };
}

function toMessage(error: unknown): string {
  return error instanceof Error ? error.message : "Unknown error.";
}
