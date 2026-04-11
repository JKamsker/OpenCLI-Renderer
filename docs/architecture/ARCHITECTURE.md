# InSpectra Architecture Charter

> **Status:** Proposed Phase 1 charter, 2026-04-10.
>
> This document defines the intended module boundaries, dependency direction, and placement rules for the backend restructure. It is intentionally a charter, not a full migration map, test plan, or implementation spec.

## Core principle

**Capability first, variant second, mechanism third.**

When a tree needs more shape, prefer:

**product -> capability -> variant -> stage**

The goal is stable seams for growth. New work should land in a known owner instead of introducing a new top-level bucket.

## Module charter

### `InSpectra.Gen` — app shell

Owns the `inspectra` program as a thin orchestration layer:

- `Program.cs`
- command parsing and command settings
- app-level use cases such as generate and render
- output shaping for human and JSON modes
- DI composition
- packaging and startup-hook integration

It does **not** own analyzer implementations, OpenCLI document rules, renderer internals, or package / NuGet / dnlib internals.

A charter-aligned shape is:

```text
src/InSpectra.Gen/
  Program.cs
  Commands/
    <Capability>/
      <Variant>/
  UseCases/
    <Capability>/
  Output/
  Composition/
```

### `InSpectra.Gen.OpenCli` — canonical OpenCLI domain

Owns the OpenCLI document itself, independent of how it is acquired or rendered:

- document model
- schema
- validation
- serialization
- enrichment

A charter-aligned OpenCLI layout may use stages such as `Model`, `Schema`, `Validation`, `Serialization`, and `Enrichment`.

OpenCli owns the domain model and related document services. Exact public API shape is an implementation detail of the extraction.

### `InSpectra.Gen.Acquisition` — target to OpenCLI

Owns the logic that turns a target into an OpenCLI document:

- sources such as exec, package, and dotnet
- acquisition modes such as native, help, CliFx, static, and hook
- acquisition planning
- shared acquisition tooling such as process, packages, NuGet, and framework detection
- projection from source-specific representations into OpenCLI

A charter-aligned shape is:

```text
src/InSpectra.Gen.Acquisition/
  Contracts/
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
      Projection/
    CliFx/
      Crawling/
      Metadata/
      Projection/
    Static/
      Frameworks/
      Inspection/
      Projection/
    Hook/
      Invocation/
      Capture/
      Projection/
  Tooling/
    Process/
    NuGet/
    Packages/
    FrameworkDetection/
  Composition/
```

### `InSpectra.Gen.Rendering` — OpenCLI to docs

Owns rendering from OpenCLI into documentation outputs:

- rendering contracts
- shared rendering pipeline
- markdown and html output
- bundle and asset resolution

Keep shared rendering concerns separate from format-specific folders such as `Markdown` and `Html`.

A charter-aligned shape is:

```text
src/InSpectra.Gen.Rendering/
  Contracts/
  Pipeline/
  Markdown/
  Html/
    Bundle/
    Assets/
  Composition/
```

### `InSpectra.Gen.StartupHook`

Owns in-process capture:

- runtime capture
- reflection and runtime helpers
- framework-specific patches

Its boundary stays focused on capture and hook-specific runtime behavior.

### `InSpectra.UI`

Owns the frontend.

### `InSpectra.Gen.Core` — optional and tiny

Shared code is a last resort. A small core module is justified only for truly cross-module primitives or errors that are semantically generic and actually reused across module boundaries.

## Dependency charter

Allowed production direction:

```text
InSpectra.Gen             -> Core, OpenCli, Acquisition, Rendering, StartupHook (integration surface only)
InSpectra.Gen.Rendering   -> Core, OpenCli
InSpectra.Gen.Acquisition -> Core, OpenCli
InSpectra.Gen.OpenCli     -> Core
InSpectra.Gen.StartupHook -> Core or none
InSpectra.Gen.Core        -> none
InSpectra.UI              -> none of the backend modules
```

Key constraints:

- `Acquisition` must not depend on `Rendering`.
- `Rendering` must not depend on `Acquisition`.
- `OpenCli` must not depend on `Acquisition`, `Rendering`, the app shell, or hook internals.
- The app shell may depend on `StartupHook` for packaging and startup-hook integration, but not on hook internals.
- The app shell must stay thin: it registers modules and orchestrates use cases, but does not reference or wire deep module internals directly. In particular, `InSpectra.Gen` must not depend on concrete acquisition internals; if the app needs a capability, expose it via a public contract, a module composition entry point, or an interface owned by the module.
- Non-test `InternalsVisibleTo` is not part of the target architecture.

Target-state rule: each module that the app shell composes must expose one public composition entry point. The app shell registers modules, not concrete internals. Typical entry points include `services.AddInSpectraOpenCli()`, `services.AddInSpectraAcquisition()`, and `services.AddInSpectraRendering()`.

The app shell must not reference concrete implementation namespaces or types from module internals. Cross-module references go through module composition entry points plus explicit public surfaces exposed by each module, such as `OpenCli` domain model and document services, `Acquisition/Contracts/`, and `Rendering/Contracts/`.

If `StartupHook` contributes services or registrations that the app shell composes, it should expose a single public entry point. Otherwise, the app-shell dependency stays limited to the startup-hook integration surface.

If enforcement is added, the app-shell composition file is **not** exempt from the no-deep-internals rule.

### Intra-module dependency rules

Inside `Acquisition`:

- `Sources/` may depend on `Tooling/` and `Contracts/`.
- `Modes/` may depend on `Tooling/`, `Contracts/`, and `OpenCli`.
- One mode must not depend on another mode.
- `Tooling/` must not depend on `Modes/`.

If a narrow source-to-mode handoff abstraction is needed, keep it explicit and local to `Acquisition` rather than letting source internals leak across the module boundary.

Inside `Rendering`:

- Format implementations depend on the shared rendering pipeline and `OpenCli`.
- `Markdown/` and `Html/` do not reference each other directly.

Inside `OpenCli`:

- Pure document logic only.
- No process execution, no NuGet, no rendering, and no filesystem orchestration beyond loader / saver boundaries.

## Placement rules

When adding code, choose the owner in this order:

1. If it is about the `inspectra` program itself, it belongs in `InSpectra.Gen`.
2. If it is about the OpenCLI document itself, it belongs in `InSpectra.Gen.OpenCli`.
3. If it is about obtaining OpenCLI from a target, it belongs in `InSpectra.Gen.Acquisition`.
4. If it is about turning OpenCLI into docs, it belongs in `InSpectra.Gen.Rendering`.
5. If it executes inside the inspected process, it belongs in `InSpectra.Gen.StartupHook`.
6. Only if it is truly cross-module and generic should it move to `Core`.

Hook-specific tie-break:

- Code that runs inside the inspected process always lives in `StartupHook`.
- Hook-mode orchestration outside the inspected process belongs on the acquisition side.

Inside `Acquisition`, keep the axes distinct:

- `Sources/` answers where the target came from.
- `Modes/` answers how it was inspected.
- `Tooling/` holds reusable support for multiple sources or modes.
- All mode-specific acquisition code lives under `Acquisition/Modes/<Mode>/`.
- `Tooling/`, `Sources/`, and other shared areas stay mode-agnostic.
- In this repo, mode-specific conversion into OpenCLI lives under that mode as `Projection/`, not as another `OpenCli` root.

Inside `Rendering`, keep shared pipeline concerns separate from concrete output formats.

Inside `OpenCli`, keep the canonical domain in one home. Mode-specific or source-specific conversion code does not become part of that domain just because it produces an OpenCLI document.

## Naming rules

Naming stays principle-based:

- Prefer owner-first names that answer one question at a time.
- Namespaces must follow the owning project name plus the relative folder path. Folder moves require matching namespace moves.
- No new top-level `Runtime`, `Infrastructure`, `Models`, `Support`, `Helpers`, or `Misc` roots.
- Generic names are acceptable only one level below a concrete owner and never as the main discovery mechanism. `Commands/Common`, `Commands/Shared`, `Modes/Help/Parsing`, `Modes/Help/Documents`, and `OpenCli/Validation/Options` can be reasonable when the owner is already explicit.
- `OpenCli` is the canonical domain root. Mode-specific folders that currently use `OpenCli` should become `Projection` under their real owner mode.
- Do not repeat semantic roots such as `OpenCli`, `Execution`, or `Documents` across unrelated branches unless they truly mean the same owned concept. When used, keep them under an explicit owner rather than as standalone roots.
- Do not introduce multi-type junk files such as `*Models.cs` unless they are tiny private DTO clusters with a very strong reason.

This charter does **not** impose artificial file-shape doctrine such as exact `*Support.cs` pairing rules, hard one-primary-type-per-file rules, token heuristics for Core placement, or fixed DI method signatures.

## Extension seams

New work should land in existing seams:

- new source kind -> `Acquisition/Sources/<Name>/`
- new acquisition mode -> `Acquisition/Modes/<Name>/`
- new static-analysis adapter -> `Acquisition/Modes/Static/Frameworks/<Name>/`
- new startup-hook framework patch -> `StartupHook/Frameworks/<Name>/`
- new renderer or output format -> `Rendering/<Format>/`
- new OpenCLI transform or rule -> `OpenCli/<Stage>/` such as `Validation/` or `Enrichment/`

These seams describe the intended extension points. They do not justify inventing new top-level buckets or placing mode-specific acquisition code outside `Modes/<Mode>/`.

## Rollout

### Phase 1 — charter

Phase 1 is charter only: capture module ownership, allowed and forbidden dependencies, placement rules, naming rules, and extension seams. No code moves, no DI changes, and no project splits occur in this phase.

### Phase 2 — enforcement

Add policy checks for the charter-level rules in this document:

- namespace and folder alignment
- no forbidden top-level buckets
- dependency direction consistent with the charter
- no non-test `InternalsVisibleTo`
- no app-shell references to forbidden acquisition namespaces
- no cross-mode dependencies

These checks act as guardrails for new code and migration work. Some repo-wide rules may only turn fully green as their matching Phase 3 moves land, so Phase 2 establishes non-regressive enforcement rather than assuming the legacy tree is already fully compliant. Enforce dependency direction using checks appropriate to the current module boundaries.

For the initial acquisition-namespace rule, `InSpectra.Gen` may reference only `InSpectra.Gen.Acquisition.Composition`, `InSpectra.Gen.Acquisition.Contracts`, and intentionally exposed public service interfaces. `Modes/`, `Sources/`, and `Tooling/` remain forbidden to the app shell.

### Phase 3 — move code without changing behavior

Apply the structure to the existing codebase:

- remove catch-all roots such as `Runtime`
- introduce `Modes/` in `Acquisition`
- rename mode-specific `OpenCli` folders to `Projection/`
- consolidate process execution and related support under `Acquisition/Tooling/Process`
- move DI composition behind module-level `AddInSpectra*` entry points so the app shell registers modules instead of internals
- centralize the OpenCLI domain in its canonical home
- move rendering concerns under `Rendering`

### Phase 4 — extract assemblies if needed

If the cleaned boundaries justify it, and module-level composition entry points are already in place, extract `OpenCli`, `Rendering`, and optionally `Core` into separate projects.

## Summary

The target architecture is:

- app shell -> thin orchestration
- OpenCLI -> canonical document domain
- acquisition -> target to OpenCLI
- rendering -> OpenCLI to docs
- startup hook -> in-process capture

Inside each module: **capability first, variant second, mechanism third.**
