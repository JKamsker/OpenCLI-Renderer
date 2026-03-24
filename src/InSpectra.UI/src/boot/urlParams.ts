export interface ViewerLinks {
  openCliUrl: string;
  xmlDocUrl?: string;
  directoryUrl?: string;
  xmlDocIsOptional: boolean;
}

interface LinkInput {
  openCliUrl?: string;
  xmlDocUrl?: string;
  directoryUrl?: string;
}

export function resolveViewerLinksFromSearch(search: string, baseUrl: string): ViewerLinks | null {
  const params = new URLSearchParams(search);
  return resolveViewerLinks(
    {
      openCliUrl: readQueryValue(params.get("opencli")),
      xmlDocUrl: readQueryValue(params.get("xmldoc")),
      directoryUrl: readQueryValue(params.get("dir")),
    },
    baseUrl,
  );
}

export function resolveViewerLinks(input: LinkInput, baseUrl: string): ViewerLinks | null {
  const openCliUrl = readQueryValue(input.openCliUrl);
  const xmlDocUrl = readQueryValue(input.xmlDocUrl);
  const directoryUrl = readQueryValue(input.directoryUrl);

  if (!openCliUrl && !directoryUrl) {
    return null;
  }

  if (directoryUrl) {
    const directory = ensureTrailingSlash(resolveUrl(directoryUrl, baseUrl));
    return {
      openCliUrl: openCliUrl ? resolveUrl(openCliUrl, baseUrl) : new URL("opencli.json", directory).toString(),
      xmlDocUrl: xmlDocUrl ? resolveUrl(xmlDocUrl, baseUrl) : new URL("xmldoc.xml", directory).toString(),
      directoryUrl: directory,
      xmlDocIsOptional: !xmlDocUrl,
    };
  }

  return {
    openCliUrl: resolveUrl(openCliUrl!, baseUrl),
    xmlDocUrl: xmlDocUrl ? resolveUrl(xmlDocUrl, baseUrl) : undefined,
    directoryUrl: undefined,
    xmlDocIsOptional: false,
  };
}

function resolveUrl(value: string, baseUrl: string): string {
  return new URL(value, baseUrl).toString();
}

function ensureTrailingSlash(value: string): string {
  return value.endsWith("/") ? value : `${value}/`;
}

function readQueryValue(value: string | null | undefined): string | undefined {
  if (!value) {
    return undefined;
  }

  const trimmed = value.trim();
  return trimmed.length > 0 ? trimmed : undefined;
}
