import { fireEvent, render, screen, waitFor, within } from "@testing-library/react";
import { StaticViewerApp } from "../StaticViewerApp";
import { buildPackageIndexUrl, resetDiscoveryCacheForTests } from "../data/nugetDiscovery";
import { testDocument } from "./fixtures";

describe("StaticViewerApp", () => {
  beforeEach(() => {
    document.body.innerHTML = "";
    window.history.replaceState({}, "", "https://example.test/static.html#/");
  });

  afterEach(() => {
    resetDiscoveryCacheForTests();
    vi.unstubAllGlobals();
  });

  it("shows only the static toolbar routes enabled by feature flags", async () => {
    setBootstrap({
      mode: "inline",
      openCli: testDocument,
      features: {
        showHome: true,
        nugetBrowser: true,
        packageUpload: false,
      },
    });

    render(<StaticViewerApp />);

    expect(screen.queryByRole("link", { name: "Home" })).not.toBeInTheDocument();
    expect(screen.queryByRole("link", { name: "Browse" })).not.toBeInTheDocument();
    expect(await screen.findByRole("link", { name: "Home" })).toBeInTheDocument();
    expect(await screen.findByRole("link", { name: "Browse" })).toBeInTheDocument();
    expect(screen.queryByRole("link", { name: "Import" })).not.toBeInTheDocument();
  });

  it("keeps a clear error when no bootstrap data is available on the default route", async () => {
    setBootstrap();

    render(<StaticViewerApp />);

    expect(await screen.findByRole("heading", { name: "Failed to load" })).toBeInTheDocument();
    expect(await screen.findByRole("alert")).toHaveTextContent("No bootstrap data found.");
  });

  it("supports the static import route without bootstrap data", async () => {
    setBootstrap();
    window.history.replaceState({}, "", "https://example.test/static.html#/import");

    render(<StaticViewerApp />);

    expect(await screen.findByRole("button", { name: "Import OpenCLI snapshot" })).toBeInTheDocument();
    expect(screen.queryByRole("link", { name: "Home" })).not.toBeInTheDocument();
    expect(screen.queryByRole("heading", { name: "Failed to load" })).not.toBeInTheDocument();
  });

  it("supports the static browse route when the feature is enabled", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/browse");
    stubDiscoveryFetch();

    render(<StaticViewerApp />);

    expect(await screen.findByRole("heading", { name: "Explore .NET CLI tools" })).toBeInTheDocument();
    expect(await screen.findByText("Alpha.Tool")).toBeInTheDocument();
  });

  it.each([
    ["browse", "#/browse"],
    ["import", "#/import"],
    ["package", "#/pkg/Alpha.Tool"],
  ])("redirects disabled static %s routes back to the embedded overview", async (_label, hash) => {
    setInlineBootstrap({
      showHome: false,
      nugetBrowser: false,
      packageUpload: false,
    });
    window.history.replaceState({}, "", `https://example.test/static.html${hash}`);

    const fetchMock = vi.fn();
    vi.stubGlobal("fetch", fetchMock);

    render(<StaticViewerApp />);

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(await screen.findByRole("heading", { name: "demo" })).toBeInTheDocument();
    expect(fetchMock).not.toHaveBeenCalled();
  });

  it.each([
    ["browse without showHome", "#/browse", { showHome: false, nugetBrowser: true, packageUpload: true }],
    ["package without showHome", "#/pkg/Alpha.Tool", { showHome: false, nugetBrowser: true, packageUpload: true }],
    ["browse without nugetBrowser", "#/browse", { showHome: true, nugetBrowser: false, packageUpload: true }],
    ["package without nugetBrowser", "#/pkg/Alpha.Tool", { showHome: true, nugetBrowser: false, packageUpload: true }],
    ["import without packageUpload", "#/import", { showHome: true, nugetBrowser: true, packageUpload: false }],
  ])("keeps gated static routes unavailable for %s", async (_label, hash, features) => {
    setInlineBootstrap(features);
    window.history.replaceState({}, "", `https://example.test/static.html${hash}`);

    const fetchMock = vi.fn();
    vi.stubGlobal("fetch", fetchMock);

    render(<StaticViewerApp />);

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(await screen.findByRole("heading", { name: "demo" })).toBeInTheDocument();
    expect(fetchMock).not.toHaveBeenCalled();
  });

  it.each([
    ["browse", "#/browse"],
    ["import", "#/import"],
  ])("keeps the standalone %s route self-contained with a Home link", async (_label, hash) => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: true });
    window.history.replaceState({}, "", `https://example.test/static.html${hash}`);

    if (hash === "#/browse") {
      stubDiscoveryFetch();
    }

    render(<StaticViewerApp />);

    if (hash === "#/browse") {
      expect(await screen.findByRole("heading", { name: "Explore .NET CLI tools" })).toBeInTheDocument();
    } else {
      expect(await screen.findByRole("button", { name: "Import OpenCLI snapshot" })).toBeInTheDocument();
    }

    fireEvent.click(await screen.findByRole("link", { name: "Home" }));

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(await screen.findByRole("heading", { name: "demo" })).toBeInTheDocument();
  });

  it.each([
    ["#/pkg/Alpha.Tool", "package"],
    ["#/pkg/Alpha.Tool/1.0.0", "versioned-package"],
    ["#/pkg/Alpha.Tool/command/beta", "latest-command"],
    ["#/pkg/Alpha.Tool/1.0.0/command/alpha", "versioned-command"],
  ])("supports the direct package deep link %s and restores the embedded viewer on Home", async (hash, mode) => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", `https://example.test/static.html${hash}`);
    stubPackageFetch();

    render(<StaticViewerApp />);

    if (mode === "versioned-command") {
      expect(await screen.findByRole("heading", { name: "alpha" })).toBeInTheDocument();
      expect(await screen.findByText("Alpha.Tool v1.0.0")).toBeInTheDocument();
    } else if (mode === "versioned-package") {
      expect(await screen.findByText("Alpha.Tool", { selector: ".brand-title" })).toBeInTheDocument();
      expect(await screen.findByText("Alpha.Tool v1.0.0")).toBeInTheDocument();
    } else if (mode === "latest-command") {
      expect(await screen.findByRole("heading", { name: "beta" })).toBeInTheDocument();
      expect(await screen.findByText("Alpha.Tool v2.0.0")).toBeInTheDocument();
    } else {
      expect(await screen.findByText("Alpha.Tool", { selector: ".brand-title" })).toBeInTheDocument();
      expect(await screen.findByText("Alpha.Tool v2.0.0")).toBeInTheDocument();
    }
    expect(window.location.hash).toBe(hash);

    fireEvent.click(await screen.findByRole("link", { name: "Home" }));

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(await screen.findByRole("heading", { name: "demo" })).toBeInTheDocument();
  });

  it("reloads the latest package route after viewing an explicit older version", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool/1.0.0");
    stubPackageFetch();

    render(<StaticViewerApp />);

    expect(await screen.findByText("Alpha.Tool v1.0.0")).toBeInTheDocument();

    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool");
    window.dispatchEvent(new HashChangeEvent("hashchange"));

    expect(await screen.findByText("Alpha.Tool v2.0.0")).toBeInTheDocument();
    expect(screen.queryByText("Alpha.Tool v1.0.0")).not.toBeInTheDocument();
  });

  it("returns to the embedded viewer when leaving a package route during load", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool");
    stubPendingPackageIndexFetch();

    render(<StaticViewerApp />);

    fireEvent.click(await screen.findByRole("link", { name: "Home" }));

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(await screen.findByRole("heading", { name: "demo" })).toBeInTheDocument();
  });

  it("returns to the embedded viewer when leaving a package route during document load", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool");
    const delayedOpenCli = createDeferredResponse();

    vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
      const url = input.toString();
      if (url === buildPackageIndexUrl("Alpha.Tool")) {
        return new Response(JSON.stringify(createDiscoveryPackageDetail()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/packages/alpha.tool/2.0.0/opencli.json") {
        return delayedOpenCli.promise;
      }

      throw new Error(`Unexpected fetch: ${url}`);
    }));

    render(<StaticViewerApp />);

    fireEvent.click(await screen.findByRole("link", { name: "Home" }));

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(await screen.findByRole("heading", { name: "demo" })).toBeInTheDocument();

    delayedOpenCli.resolve(new Response(JSON.stringify(createPackageDocument("2.0.0", "beta")), { status: 200 }));

    await waitFor(() => expect(window.location.hash).toBe("#/"));
    expect(screen.queryByText("Alpha.Tool v2.0.0")).not.toBeInTheDocument();
  });

  it("shows a clear error when a direct package deep link fails to load", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool");
    vi.stubGlobal("fetch", vi.fn(async () => new Response("missing", { status: 404, statusText: "Not Found" })));

    render(<StaticViewerApp />);

    expect(await screen.findByRole("heading", { name: "Failed to load" })).toBeInTheDocument();
    expect(await screen.findByRole("alert")).toHaveTextContent("Failed to load package index for Alpha.Tool");
  });

  it("shows a clear error when a direct package route requests a missing version", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool/9.9.9");

    vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
      const url = input.toString();
      if (url === buildPackageIndexUrl("Alpha.Tool")) {
        return new Response(JSON.stringify(createDiscoveryPackageDetail()), { status: 200 });
      }

      throw new Error(`Unexpected fetch: ${url}`);
    }));

    render(<StaticViewerApp />);

    expect(await screen.findByRole("heading", { name: "Failed to load" })).toBeInTheDocument();
    expect(await screen.findByRole("alert")).toHaveTextContent("Version \"9.9.9\" not found for package \"Alpha.Tool\".");
  });

  it("does not retry a failed direct package load until the route changes", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/pkg/Alpha.Tool");
    const fetchMock = vi.fn(async () => new Response("missing", { status: 404, statusText: "Not Found" }));
    vi.stubGlobal("fetch", fetchMock);

    render(<StaticViewerApp />);

    expect(await screen.findByRole("heading", { name: "Failed to load" })).toBeInTheDocument();
    await waitFor(() => expect(fetchMock).toHaveBeenCalledTimes(1));
  });

  it("loads the selected version from the browse detail page", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/browse/Alpha.Tool");
    stubPackageFetch();

    render(<StaticViewerApp />);

    const versionRow = (await screen.findByText("v1.0.0")).closest(".ver-row");
    expect(versionRow).not.toBeNull();
    fireEvent.click(within(versionRow as HTMLElement).getByRole("button", { name: "Inspect" }));

    await waitFor(() => expect(window.location.hash).toBe("#/pkg/Alpha.Tool/1.0.0"));
    expect(await screen.findByText("Alpha.Tool", { selector: ".brand-title" })).toBeInTheDocument();
    expect(await screen.findByText("Alpha.Tool v1.0.0")).toBeInTheDocument();
  });

  it("shows a clear error when a browse-detail inspect fails", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/browse/Alpha.Tool");

    vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
      const url = input.toString();
      if (url === buildPackageIndexUrl("Alpha.Tool")) {
        return new Response(JSON.stringify(createDiscoveryPackageDetail()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/index.min.json" || url === "https://inspectra-data.kamsker.at/index.json") {
        return new Response(JSON.stringify(createDiscoverySummaryIndex()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/packages/alpha.tool/1.0.0/opencli.json") {
        return new Response("missing", { status: 404, statusText: "Not Found" });
      }

      throw new Error(`Unexpected fetch: ${url}`);
    }));

    render(<StaticViewerApp />);

    const versionRow = (await screen.findByText("v1.0.0")).closest(".ver-row");
    expect(versionRow).not.toBeNull();
    fireEvent.click(within(versionRow as HTMLElement).getByRole("button", { name: "Inspect" }));

    expect(await screen.findByRole("alert")).toHaveTextContent("Failed to load https://inspectra-data.kamsker.at/packages/alpha.tool/1.0.0/opencli.json");
    expect(window.location.hash).toBe("#/browse/Alpha.Tool");
    expect(screen.queryByRole("heading", { name: "Failed to load" })).not.toBeInTheDocument();
  });

  it("abandons a stale browse-detail inspect after returning to the package list", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/browse/Alpha.Tool");
    const delayedOpenCli = createDeferredResponse();

    vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
      const url = input.toString();
      if (url === buildPackageIndexUrl("Alpha.Tool")) {
        return new Response(JSON.stringify(createDiscoveryPackageDetail()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/index.min.json" || url === "https://inspectra-data.kamsker.at/index.json") {
        return new Response(JSON.stringify(createDiscoverySummaryIndex()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/packages/alpha.tool/1.0.0/opencli.json") {
        return delayedOpenCli.promise;
      }

      throw new Error(`Unexpected fetch: ${url}`);
    }));

    render(<StaticViewerApp />);

    const versionRow = (await screen.findByText("v1.0.0")).closest(".ver-row");
    expect(versionRow).not.toBeNull();
    fireEvent.click(within(versionRow as HTMLElement).getByRole("button", { name: "Inspect" }));
    fireEvent.click(await screen.findByRole("button", { name: "All packages" }));

    expect(await screen.findByRole("heading", { name: "Explore .NET CLI tools" })).toBeInTheDocument();

    delayedOpenCli.resolve(new Response(JSON.stringify(createPackageDocument("1.0.0", "alpha")), { status: 200 }));

    await waitFor(() => expect(window.location.hash).toBe("#/browse"));
    expect(screen.queryByRole("heading", { name: "alpha" })).not.toBeInTheDocument();
  });

  it("abandons a stale browse-detail inspect failure after returning to the package list", async () => {
    setInlineBootstrap({ showHome: true, nugetBrowser: true, packageUpload: false });
    window.history.replaceState({}, "", "https://example.test/static.html#/browse/Alpha.Tool");
    const delayedOpenCli = createDeferredResponse();

    vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
      const url = input.toString();
      if (url === buildPackageIndexUrl("Alpha.Tool")) {
        return new Response(JSON.stringify(createDiscoveryPackageDetail()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/index.min.json" || url === "https://inspectra-data.kamsker.at/index.json") {
        return new Response(JSON.stringify(createDiscoverySummaryIndex()), { status: 200 });
      }

      if (url === "https://inspectra-data.kamsker.at/packages/alpha.tool/1.0.0/opencli.json") {
        return delayedOpenCli.promise;
      }

      throw new Error(`Unexpected fetch: ${url}`);
    }));

    render(<StaticViewerApp />);

    const versionRow = (await screen.findByText("v1.0.0")).closest(".ver-row");
    expect(versionRow).not.toBeNull();
    fireEvent.click(within(versionRow as HTMLElement).getByRole("button", { name: "Inspect" }));
    fireEvent.click(await screen.findByRole("button", { name: "All packages" }));

    expect(await screen.findByRole("heading", { name: "Explore .NET CLI tools" })).toBeInTheDocument();

    delayedOpenCli.resolve(new Response("missing", { status: 404, statusText: "Not Found" }));

    await waitFor(() => expect(window.location.hash).toBe("#/browse"));
    expect(screen.queryByRole("alert")).not.toBeInTheDocument();
  });
});

function setBootstrap(payload?: unknown) {
  document.body.innerHTML = `
    <script id="inspectra-bootstrap" type="application/json">${payload ? JSON.stringify(payload) : "__INSPECTRA_BOOTSTRAP__"}</script>
  `;
}

function setInlineBootstrap(features?: Record<string, boolean>) {
  setBootstrap({
    mode: "inline",
    openCli: testDocument,
    features,
  });
}

function stubDiscoveryFetch() {
  const summaryIndex = createDiscoverySummaryIndex();

  vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
    const url = input.toString();
    if (url === "https://inspectra-data.kamsker.at/index.min.json" || url === "https://inspectra-data.kamsker.at/index.json") {
      return new Response(JSON.stringify(summaryIndex), { status: 200 });
    }

    throw new Error(`Unexpected fetch: ${url}`);
  }));
}

function stubPackageFetch() {
  const packageDetail = createDiscoveryPackageDetail();

  vi.stubGlobal("fetch", vi.fn(async (input: RequestInfo | URL) => {
    const url = input.toString();
    if (url === buildPackageIndexUrl("Alpha.Tool")) {
      return new Response(JSON.stringify(packageDetail), { status: 200 });
    }

    if (url === "https://inspectra-data.kamsker.at/packages/alpha.tool/2.0.0/opencli.json") {
      return new Response(JSON.stringify(createPackageDocument("2.0.0", "beta")), { status: 200 });
    }

    if (url === "https://inspectra-data.kamsker.at/packages/alpha.tool/1.0.0/opencli.json") {
      return new Response(JSON.stringify(createPackageDocument("1.0.0", "alpha")), { status: 200 });
    }

    throw new Error(`Unexpected fetch: ${url}`);
  }));
}

function stubPendingPackageIndexFetch() {
  vi.stubGlobal("fetch", vi.fn((input: RequestInfo | URL, init?: RequestInit) => {
    const url = input.toString();
    if (url === buildPackageIndexUrl("Alpha.Tool")) {
      return new Promise<Response>((_resolve, reject) => {
        init?.signal?.addEventListener(
          "abort",
          () => reject(new DOMException("Aborted", "AbortError")),
          { once: true },
        );
      });
    }

    throw new Error(`Unexpected fetch: ${url}`);
  }));
}

function createPackageDocument(version: string, commandName: string) {
  return {
    ...testDocument,
    info: {
      ...testDocument.info,
      title: "Alpha.Tool",
      version,
    },
    commands: [
      {
        ...testDocument.commands[0],
        name: commandName,
        path: undefined,
      },
    ],
  };
}

function createDiscoverySummaryIndex() {
  return {
    schemaVersion: 1,
    generatedAt: "2026-03-28T00:00:00Z",
    packageCount: 1,
    packages: [{
      packageId: "Alpha.Tool",
      commandName: "alpha",
      versionCount: 2,
      latestVersion: "2.0.0",
      createdAt: "2026-03-01T00:00:00Z",
      updatedAt: "2026-04-01T00:00:00Z",
      completeness: "full" as const,
      totalDownloads: 100,
      commandCount: 10,
      commandGroupCount: 4,
    }],
  };
}

function createDiscoveryPackageDetail() {
  return {
    schemaVersion: 1,
    packageId: "Alpha.Tool",
    trusted: true,
    totalDownloads: 100,
    latestVersion: "2.0.0",
    latestStatus: "ok" as const,
    latestPaths: {
      metadataPath: "packages/alpha.tool/2.0.0/metadata.json",
      opencliPath: "packages/alpha.tool/2.0.0/opencli.json",
      xmldocPath: null,
    },
    versions: [
      {
        version: "2.0.0",
        publishedAt: "2026-04-01T00:00:00Z",
        evaluatedAt: "2026-04-01T00:00:00Z",
        status: "ok" as const,
        command: "beta",
        timings: {
          totalMs: 100,
          installMs: 25,
          opencliMs: 25,
          xmldocMs: null,
        },
        paths: {
          metadataPath: "packages/alpha.tool/2.0.0/metadata.json",
          opencliPath: "packages/alpha.tool/2.0.0/opencli.json",
          xmldocPath: null,
        },
      },
      {
        version: "1.0.0",
        publishedAt: "2026-03-28T00:00:00Z",
        evaluatedAt: "2026-03-28T00:00:00Z",
        status: "ok" as const,
        command: "alpha",
        timings: {
          totalMs: 100,
          installMs: 25,
          opencliMs: 25,
          xmldocMs: null,
        },
        paths: {
          metadataPath: "packages/alpha.tool/1.0.0/metadata.json",
          opencliPath: "packages/alpha.tool/1.0.0/opencli.json",
          xmldocPath: null,
        },
      },
    ],
  };
}

function createDeferredResponse() {
  let resolve!: (response: Response) => void;
  const promise = new Promise<Response>((resolver) => {
    resolve = resolver;
  });

  return { promise, resolve };
}
