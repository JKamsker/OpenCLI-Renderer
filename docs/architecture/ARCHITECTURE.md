# InSpectra Architecture Charter

> **Status:** Adopted 2026-04-10. This is the binding charter for all backend code under `src/InSpectra.Gen*`. Every folder, namespace, and project reference must conform. Violations block merge.

The one-line version:

> **Capability first, variant second, mechanism third.**

And inside a module:

> **product → capability → variant → stage.**

The long-form rationale lives in `docs/Tasks/Restructure/Task.md`. This file is the enforceable summary.

---

## 1. Module ownership

InSpectra has exactly these backend modules. Each module has one reason to exist. Nothing else gets a top-level bucket.

| Module | Reason to exist | Owns |
|---|---|---|
| `InSpectra.Gen` | The `inspectra` CLI program | `Program.cs`, commands, settings, DI composition, JSON vs human output, app-level use cases, packaging glue |
| `InSpectra.Gen.OpenCli` | Canonical OpenCLI document domain | `OpenCliDocument` and sibling model types, schema provider, loader, serializer, cloner, compatibility sanitization, XML enrichment, publishability / structural validation, option/structure sanitizers |
| `InSpectra.Gen.Acquisition` | Turn a target into an OpenCLI document | Executable resolution, target materialization, dotnet build output resolution, package installation, process execution, NuGet/package inspection, framework detection, acquisition planning, mode analyzers (help, cliFx, static, hook, native) |
| `InSpectra.Gen.Rendering` | Turn an OpenCLI document into human-readable docs | Render contracts, normalized render model, markdown and html formatters, render stats, html bundle / asset resolution |
| `InSpectra.Gen.StartupHook` | Run inside the inspected app and capture CLI metadata | Runtime capture, reflection/runtime helpers, framework-specific patches |
| `InSpectra.Gen.Core` *(tiny, optional)* | Cross-cutting primitives shared by every other module | `CliException`, `CliUsageException`, `CliDataException`, `CliSourceExecutionException`, and nothing else until there is real reuse |
| `InSpectra.UI` | Frontend viewer for HTML output | TypeScript/Vite app. Does not reference backend projects. |

Until assemblies are split, these modules live as **top-level folders inside `InSpectra.Gen`** (with matching namespaces). Phase 4 extracts them as separate projects. The ownership rules apply immediately; the assembly boundary follows later.

### What does *not* get its own module

- Convenience buckets like `Common`, `Shared`, `Utils`, `Helpers`, `Support`, `Misc`.
- "Infrastructure" as a generic catch-all.
- "Runtime" as a generic catch-all for DTOs and options.
- "Models" as a generic catch-all mixing domain and view models.

---

## 2. Dependency rules

### 2.1 Allowed directions

```text
InSpectra.Gen              → Core, OpenCli, Acquisition, Rendering
InSpectra.Gen.Rendering    → Core, OpenCli
InSpectra.Gen.Acquisition  → Core, OpenCli
InSpectra.Gen.OpenCli      → Core
InSpectra.Gen.StartupHook  → Core            (ideally nothing)
InSpectra.Gen.Core         → nothing
InSpectra.UI               → nothing          (no backend references)
```

### 2.2 Forbidden

- `Acquisition → Rendering`
- `Rendering → Acquisition`
- `OpenCli → Acquisition | Rendering | App`
- `Core → anything`
- `StartupHook → Acquisition | Rendering | App`
- `App → deep acquisition internals` (see §2.3)
- **Any non-test `InternalsVisibleTo`**

Today's known violations (to be fixed in Phase 3):

- `src/InSpectra.Gen.Acquisition/Properties/AssemblyInfo.cs:4` exposes internals to `inspectra`. Must be removed. Only `InSpectra.Gen.Acquisition.Tests` is allowed.

### 2.3 What "deep acquisition internals" means

The app shell must not `using` or register concrete analyzer implementation types. It must depend on:

- Public contracts exposed by the module, or
- A single composition entry point per module.

The charter requires one composition method per module:

```csharp
services.AddInSpectraOpenCli();
services.AddInSpectraAcquisition();
services.AddInSpectraRendering();
```

After Phase 2, `InSpectra.Gen/Composition/ServiceCollectionExtensions.cs` must contain only those three calls plus command registration. The app shell does not know about `CliFxMetadataInspector`, `DnlibAssemblyScanner`, `HookInstalledToolAnalysisSupport`, `InstalledToolAnalyzer`, or any other analyzer type name.

### 2.4 Intra-module rules

**Inside `Acquisition`:**

- `Sources` may depend on `Tooling` and `Contracts`.
- `Modes` may depend on `Tooling`, `Contracts`, and `OpenCli`.
- **One mode must not depend on another mode.** Help does not depend on CliFx. Static does not depend on Hook. Etc.
- `Tooling` must not depend on `Modes`.
- `Contracts` depends on nothing inside Acquisition.

**Inside `Rendering`:**

- Format implementations (`Markdown/`, `Html/`) depend on `Pipeline/`, `Contracts/`, and `OpenCli`.
- `Markdown/` and `Html/` do not reference each other directly.
- `Pipeline/` does not know about any specific format.

**Inside `OpenCli`:**

- Pure document logic only.
- No process execution, no filesystem orchestration beyond the loader/saver boundary, no NuGet, no rendering, no DI wiring of non-OpenCli types.

---

## 3. Placement rules

When someone adds a file, they ask these questions in order and stop at the first yes.

1. **Is it about the `inspectra` program itself — argv, DI, output envelope, a command class?**
   → `InSpectra.Gen`.
2. **Is it about the OpenCLI document itself, independent of how it was obtained or rendered?**
   → `InSpectra.Gen.OpenCli`.
3. **Is it about turning a target into an OpenCLI document?**
   → `InSpectra.Gen.Acquisition`.
    - **New source kind** (exec, package, dotnet, …) → `Acquisition/Sources/<Name>/`.
    - **New acquisition mode** (native, help, cliFx, static, hook, …) → `Acquisition/Modes/<Name>/`.
    - **New static-analysis framework adapter** → `Acquisition/Modes/Static/Frameworks/<Name>/`.
4. **Is it about turning an OpenCLI document into docs?**
   → `InSpectra.Gen.Rendering`.
    - **New output format** → `Rendering/<Format>/`.
5. **Does the code execute inside the inspected application process?**
   → `InSpectra.Gen.StartupHook`.
    - **New patched framework** → `StartupHook/Frameworks/<Name>/`.
6. **Is it truly shared by ≥ 2 modules, semantically generic, and likely to stay generic?**
   → `InSpectra.Gen.Core`. Otherwise keep it next to its owner.

### 3.1 Canonical internal shapes

**`InSpectra.Gen`**

```text
src/InSpectra.Gen/
  Program.cs
  Composition/
  Commands/
    Common/                 # shared settings bases
    Generate/
      Exec/
      Package/
      Dotnet/
    Render/
      File/
  UseCases/
    Generate/
    Render/
  Output/
    Json/
```

**`InSpectra.Gen.OpenCli`** (today: folder inside `InSpectra.Gen`)

```text
OpenCli/
  Model/                    # OpenCliDocument, OpenCliCommand, OpenCliOption, …
  Schema/
  Serialization/            # loader, serializer, cloner
  Validation/
    Documents/
    Options/
    Structure/
  Enrichment/               # XML enrichment, normalization
```

**`InSpectra.Gen.Acquisition`** (today: separate project)

```text
src/InSpectra.Gen.Acquisition/
  Contracts/                # public interfaces, DTOs, results, exceptions re-exported from Core
  Sources/
    Exec/
    Package/
    Dotnet/
    Targets/
  Modes/
    Native/
    Help/
      Crawling/
      Parsing/
      Inference/
      Signatures/
      Projection/           # was Help/OpenCli/
    CliFx/
      Crawling/
      Metadata/
      Execution/
      Projection/           # was Analysis/CliFx/OpenCli/
    Static/
      Attributes/
      Inspection/
      Frameworks/
      Projection/           # was StaticAnalysis/OpenCli/
    Hook/
      Invocation/
      Capture/
      Projection/
  Tooling/
    Process/
    NuGet/
    Packages/
    FrameworkDetection/
    Paths/
    Json/
  Composition/              # AddInSpectraAcquisition
```

**`InSpectra.Gen.Rendering`** (today: folder inside `InSpectra.Gen`)

```text
Rendering/
  Contracts/
  Pipeline/
    Model/                  # NormalizedCliDocument, NormalizedCommand, ResolvedOption
  Markdown/
  Html/
    Bundle/                 # was Viewer/
    Assets/
  Composition/
```

**`InSpectra.Gen.StartupHook`**

```text
src/InSpectra.Gen.StartupHook/
  StartupHook.cs
  Frameworks/
    <Framework>/
      ...
```

---

## 4. Naming rules

### 4.1 Namespaces follow folders

Every namespace must equal `{AssemblyName}.{RelativeFolderPath.Replaced('/', '.')}`. File-scoped namespaces are mandatory. This is already the repo norm; the rule is to keep it.

### 4.2 Banned folder names as top-level or semantic roots

These names are not allowed as the primary discovery mechanism at the root of a module:

- `Runtime`
- `Infrastructure`
- `Models`
- `Support`
- `Helpers`
- `Misc`
- `Common` *(except directly inside `Commands/` as a settings-base bucket)*

They may appear one level below a real owner if they describe a specific thing (e.g. `Modes/Static/Inspection/` is fine because `Static` is the owner). They must never appear at the top of a module.

### 4.3 `OpenCli` is a reserved name

`OpenCli` names the canonical document domain. It may only appear as:

- The top-level folder of `InSpectra.Gen.OpenCli`, and
- Model type prefixes (`OpenCliDocument`, `OpenCliOption`, …).

Mode-specific folders that project a source representation into OpenCLI must be named `Projection`, not `OpenCli`. Examples: `Modes/Help/Projection/`, `Modes/CliFx/Projection/`, `Modes/Static/Projection/`.

### 4.4 File/type rules

- One primary type per file. File name equals primary type name.
- No multi-type junk files like `*Models.cs` containing unrelated DTO clusters. Small related record clusters (e.g. one discriminated union) are acceptable when there is a strong reason.
- `*Support.cs` is discouraged. It usually means "I did not name the concept". Only keep when the concept really is an adapter/helper tightly bound to a sibling primary type in the same folder.
- Interfaces go in the same folder as their owning concept. No `Abstractions/` folder unless it truly houses contracts shared across modules.

### 4.5 No `InternalsVisibleTo` to non-test assemblies

If the app needs a type from Acquisition, promote it to a public contract or expose it through the module's composition method. No exceptions.

---

## 5. Allowed extension seams

Extensions must land in a known seam. Inventing a new top-level bucket is a policy violation.

| New thing | Where it goes |
|---|---|
| New source kind (e.g. `WinGet`) | `Acquisition/Sources/WinGet/` |
| New acquisition mode (e.g. `Completions`) | `Acquisition/Modes/Completions/` |
| New static-analysis framework adapter | `Acquisition/Modes/Static/Frameworks/<Name>/` |
| New startup-hook framework patch | `StartupHook/Frameworks/<Name>/` |
| New output format (e.g. `Man`, `Json`) | `Rendering/<Format>/` |
| New OpenCLI transform | `OpenCli/Enrichment/` or `OpenCli/Validation/` |
| New command class | `Commands/<Capability>/<Variant>/` |
| New acquisition tool helper (process, NuGet, package, path) | `Acquisition/Tooling/<Category>/` |
| New shared exception type | `Core/Errors/` |

If none of the existing seams fit, the proposal must amend this charter *before* the code lands. Adding code first and "noting the new bucket later" is not allowed.

---

## 6. Enforcement

Phase 2 adds repository policy tests under `tests/InSpectra.Gen.Tests/` (alongside the existing `RepositoryCodeFilePolicyTests.cs`). Target set:

1. **Namespace equals folder path** — every `.cs` file's declared namespace matches its relative folder.
2. **No banned top-level folders** — no root-level `Runtime/`, `Infrastructure/`, `Models/`, `Support/`, `Helpers/`, `Misc/` inside any `src/InSpectra.Gen*` project.
3. **No non-test `InternalsVisibleTo`** — scan `AssemblyInfo.cs` and csproj for `InternalsVisibleTo` attributes; only `*.Tests` targets allowed.
4. **Project reference direction** — parse each csproj, assert allowed edges from §2.1 only.
5. **App does not reference deep acquisition namespaces** — `InSpectra.Gen` files may not `using` any namespace matching `InSpectra.Gen.Acquisition.(Modes|Sources|Tooling|Analysis|Help|StaticAnalysis|NuGet|Packages|Frameworks|Infrastructure).*`. Only `InSpectra.Gen.Acquisition.Contracts` and `InSpectra.Gen.Acquisition.Composition` are allowed.
6. **Modes do not reference other modes** — files under `Acquisition/Modes/<X>/` may not `using` `InSpectra.Gen.Acquisition.Modes.<Y>.*` for any `Y ≠ X`.
7. **Reserved `OpenCli` namespace** — `OpenCli` may only appear as a namespace segment inside the OpenCli module. Mode-specific projections must use `Projection`.

Each test lives as a separate `[Fact]` so failures are targeted and diffable. Use the existing `FixturePaths` helper to locate the repo root.

---

## 7. Rollout (reference)

This charter corresponds to Phase 1 of the restructure plan in `docs/Tasks/Restructure/Task.md`.

1. **Phase 1 — Charter.** This document. No code changes.
2. **Phase 2 — Enforce.** Add the policy tests from §6. They initially fail against the known violations; that is the baseline. New code must not add new failures.
3. **Phase 3 — Move files without behaviour changes.** Apply the move-map from `Task.md`. Kill `Runtime/` as a catch-all, introduce `Modes/`, rename mode-specific `OpenCli/` folders to `Projection/`, consolidate process execution into `Acquisition/Tooling/Process/`, centralize the OpenCLI domain.
4. **Phase 4 — Split assemblies.** Extract `InSpectra.Gen.OpenCli`, `InSpectra.Gen.Rendering`, and (optionally) `InSpectra.Gen.Core` as separate projects once their internal boundaries are clean.

Steps 2 and 3 can overlap: start moving files as soon as the corresponding policy test exists to keep the target shape honest.

---

## 8. The one-sentence restatement

**App shell** orchestrates thinly. **OpenCLI** owns the document. **Acquisition** turns targets into documents. **Rendering** turns documents into docs. **Startup hook** captures from inside. Inside each: **capability first, variant second, mechanism third.**

New work lands in a known seam. When no seam fits, amend this charter first.
