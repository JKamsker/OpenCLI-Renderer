# OpenCLI Renderer

**Turn any CLI's OpenCLI spec into beautiful, navigable documentation -- Markdown or interactive HTML -- in a single command.**

```
opencli-renderer render file html mycli.json --out docs.html
```

OpenCLI Renderer is a .NET 10 tool that reads [OpenCLI](https://opencli.org/) JSON exports, optionally enriches them with XML metadata, and produces polished documentation you can ship, host, or commit alongside your project.

---

## Why?

CLI docs rot. They fall out of sync with `--help`, live in wikis nobody updates, or just don't exist. OpenCLI Renderer closes that gap:

- **Single source of truth** -- your CLI already exposes its structure via `cli opencli`. Render it.
- **Zero network calls** -- everything runs locally, validates against an embedded JSON Schema, and writes to disk.
- **Two output formats** -- Markdown for GitHub/docs sites, interactive HTML for standalone hosting.
- **Two layout modes** -- single-file for quick reference, tree layout mirroring your command hierarchy.
- **Optional XML enrichment** -- merge richer descriptions from `cli xmldoc` without overwriting the spec.

---

## Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Install & Run

```bash
# Clone
git clone https://github.com/JKamsker/OpenCLI-Renderer.git
cd OpenCLI-Renderer

# Build
dotnet build --configuration Release

# Render Markdown from an OpenCLI JSON file
dotnet run --project src/OpenCli.Renderer -- \
  render file markdown examples/jellyfin-cli/opencli.json \
  --xmldoc examples/jellyfin-cli/xmldoc.xml \
  --out jellyfin-docs.md

# Render interactive HTML
dotnet run --project src/OpenCli.Renderer -- \
  render file html examples/jellyfin-cli/opencli.json \
  --xmldoc examples/jellyfin-cli/xmldoc.xml \
  --out jellyfin-docs.html
```

Open `jellyfin-docs.html` in your browser -- you get a full SPA with sidebar navigation, search, dark mode, and a command composer.

---

## Command Reference

```
opencli-renderer render [file|exec] [markdown|html] [OPTIONS]
```

### Source Modes

| Mode | Description |
|------|-------------|
| `file` | Render from saved `.json` / `.xml` files on disk |
| `exec` | Execute a live CLI process and capture its OpenCLI output |

### File Commands

```bash
render file markdown <OPENCLI_JSON> [OPTIONS]
render file html     <OPENCLI_JSON> [OPTIONS]
```

### Exec Commands

```bash
render exec markdown <SOURCE> [OPTIONS]
render exec html     <SOURCE> [OPTIONS]
```

### Key Options

| Option | Description |
|--------|-------------|
| `--xmldoc <PATH>` | XML enrichment file to merge |
| `--layout single\|tree` | Output layout (default: `single`) |
| `--out <FILE>` | Output file path |
| `--out-dir <DIR>` | Output directory (for `tree` layout) |
| `--include-hidden` | Include hidden commands and options |
| `--include-metadata` | Append metadata section |
| `--overwrite` | Overwrite existing files |
| `--dry-run` | Preview output without writing |
| `--json` | Machine-readable JSON output |
| `--verbose` / `--quiet` | Control log verbosity |
| `--timeout <SECONDS>` | Process timeout for exec mode (default: 30) |

### Exec-Specific Options

| Option | Description |
|--------|-------------|
| `--source-arg <ARG>` | Arguments passed to the source CLI |
| `--opencli-arg <ARG>` | Arguments to invoke the OpenCLI export |
| `--with-xmldoc` | Also run `cli xmldoc` for enrichment |
| `--xmldoc-arg <ARG>` | Custom arguments for xmldoc invocation |

---

## Output Formats

### Markdown

Clean, GitHub-compatible Markdown with:
- Full table of contents with anchor links
- Command sections with arguments, options, examples, and exit codes
- Inherited options clearly separated from declared ones
- Single-file or tree layout (one `.md` per command group)

### Interactive HTML

A standalone `.html` file (no external dependencies) featuring:
- **SPA Navigation** -- click through your command tree without page reloads
- **Dark / Light Theme** -- toggle with one click, persisted in localStorage
- **Real-Time Search** -- filter the command tree as you type
- **Command Composer** -- interactively build commands with a resizable side panel
- **Breadcrumb Navigation** -- always know where you are in the command hierarchy
- **Command Cards** -- visual cards for subcommands with descriptions
- **Smooth Animations** -- staggered card reveals on navigation
- **Embedded Fonts** -- JetBrains Mono for code, Plus Jakarta Sans for text

---

## Architecture

The rendering pipeline:

```
OpenCLI JSON ──┐
               ├──▶ Validate ──▶ Normalize ──▶ Enrich ──▶ Render ──▶ Output
XML Metadata ──┘     (schema)    (inheritance)   (optional)  (md/html)  (file/dir)
```

### Key Design Decisions

- **Normalization layer** -- the `OpenCliDocument` is transformed into a `NormalizedCliDocument` that resolves recursive option inheritance, filters hidden items, and flattens the command tree for renderers.
- **Schema validation** -- an embedded JSON Schema (`0.1-draft`, draft 2020-12) validates input before any processing.
- **XML enrichment is additive** -- XML metadata fills gaps but never overwrites existing JSON values.
- **Renderers are format-agnostic** -- Markdown and HTML renderers share the same normalized model, ensuring consistent output.

### Service Pipeline

| Service | Role |
|---------|------|
| `OpenCliDocumentLoader` | Load & validate JSON against schema |
| `OpenCliNormalizer` | Build normalized document with option inheritance |
| `OpenCliXmlEnricher` | Merge XML metadata (optional) |
| `MarkdownRenderer` / `HtmlRenderer` | Format-specific rendering |
| `DocumentRenderService` | Orchestrate the full pipeline |
| `ProcessRunner` | Execute external CLIs (exec mode) |
| `ExecutableResolver` | Resolve CLI paths via PATH lookup |

---

## Examples

The `examples/` directory contains rendered documentation for real CLIs:

| CLI | Description |
|-----|-------------|
| [jellyfin-cli](examples/jellyfin-cli/) | Jellyfin media server management CLI |
| [jdownloader-remotecli](examples/jdownloader-remotecli/) | JDownloader remote control CLI |

Each includes the source `opencli.json`, `xmldoc.xml`, a single-file render, and a tree render.

---

## Project Structure

```
src/OpenCli.Renderer/
├── Commands/Render/     # CLI command handlers (file/exec x markdown/html)
├── Services/            # Rendering pipeline (21 services)
├── Models/              # OpenCliDocument, NormalizedCliDocument
├── Schema/              # Embedded OpenCLI JSON Schema
├── Templates/           # HTML CSS & JS (embedded resources)
├── Common/              # DI type registration
├── Runtime/             # Error handling, contracts
└── Program.cs           # Entry point & service wiring

tests/OpenCli.Renderer.Tests/
├── MarkdownRenderServiceTests.cs
├── OpenCliEnrichmentAndRenderingTests.cs
├── ExecutableResolverTests.cs
└── ...
```

---

## Testing

```bash
dotnet test --configuration Release
```

The test suite covers:
- Schema validation with detailed error messages
- Full pipeline (load -> normalize -> enrich -> render)
- Markdown and HTML output for single and tree layouts
- Exec mode with process execution
- Overwrite protection and destructive operation guards
- Executable resolution and PATH lookup

---

## CI/CD

GitHub Actions runs on every push and PR:

1. **Build** -- `dotnet build --configuration Release`
2. **Test** -- `dotnet test --configuration Release`
3. **Render** -- generates all four output combinations (markdown/html x single/tree)
4. **Artifact** -- uploads rendered docs as a downloadable artifact

---

## Tech Stack

| Component | Library |
|-----------|---------|
| CLI Framework | [Spectre.Console.Cli](https://spectreconsole.net/) |
| Console Output | [Spectre.Console](https://spectreconsole.net/) |
| JSON Schema | [JsonSchema.Net](https://github.com/gregsdennis/json-everything) |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Runtime | .NET 10 |
| Tests | xUnit |

---

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-thing`)
3. Keep files under 300 lines where possible (500 max)
4. Add tests for new functionality
5. Submit a pull request

---

## Related Projects

- **[OpenCLI Spec](https://opencli.org/)** ([GitHub](https://github.com/spectreconsole/open-cli)) -- the JSON specification that describes CLI structure
- **[Jellyfin-Cli](https://github.com/JKamsker/Jellyfin-Cli)** -- a CLI built with OpenCLI support
- **[JDownloader-RemoteCli](https://github.com/JKamsker/JDownloader-RemoteCli)** -- another OpenCLI-enabled CLI
