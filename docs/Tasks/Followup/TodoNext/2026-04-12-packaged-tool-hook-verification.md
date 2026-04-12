# Todo Next: Packaged-Tool Hook Verification

- ID: `TN-2026-04-12-03`
- State: `In Progress`
- Added: `2026-04-12`
- Source: outer iteration 8 post-`g41` fresh-swarm wave 1 on hosted-green tip
  `29a526c`
- Queue entry: [Todo Next Queue](../TodoNext.md)
- Pickup rule: this is the active implementation phase selected after the
  post-`g41` wave-1 convergence.
- Exit rule: mark the queue item `Completed`, `Deferred`, or `Rejected` with
  dated rationale and update [Logbook](../Logbook.md) accordingly.

## Why This Phase Goes Next

Wave 1 after `g41` mostly reconfirmed the known remaining clusters rather than
surfacing a materially new family. This slice is the most contained next HIGH:

- `.github/workflows/ci.yml` already installs the locally packed `.nupkg`
- CI already validates the published `hooks/` layout for `dotnet publish`
- the remaining gap is that `.nupkg` validation only runs `inspectra --version`
  and never proves the installed tool still carries a usable hook payload

That keeps the write set narrow and closes a runtime-critical blind spot without
mixing frontend E2E wiring or static-HTML contract work into the same phase.

## Current Local Implementation Progress

The current local phase work is intentionally using package-layout validation
rather than a broader runtime smoke:

- the workflow now installs the just-built `.nupkg` into a temp `--tool-path`
- a temp `NuGet.Config` clears ambient sources so the install proves the local
  package rather than a remote feed
- the CI step now asserts the installed `.store/.../tools/*/any/hooks/`
  payload layout that the runtime hook resolver depends on

Commit, push, and hosted validation are still pending.

## Confirmed Problem Statement

On the hosted-validated baseline that selected this phase, the PR CI lane
verified:

- `dotnet publish` output contains `hooks/InSpectra.Gen.StartupHook.dll`
  and `hooks/0Harmony.dll`
- the locally built `.nupkg` can be installed as a global tool
- the installed command responds to `inspectra --version`

It does **not** verify that the installed package layout still contains the
required hook payload, even though runtime hook analysis depends on those
installed sibling `hooks/` files. A packaging regression can therefore ship
green if the version command still works.

Representative evidence from the post-`g41` wave:

- `.github/workflows/ci.yml` validates the packed tool by running only
  `inspectra --version`
- `HookInstalledToolAnalysisSupport` resolves the hook DLL from the installed
  `hooks/` sibling layout at runtime
- current tests around package acquisition and hook analysis mostly bypass the
  real `.nupkg` package layout

## Planned Scope

Keep the phase focused on the packaged-tool verification gap:

- add a PR-safe validation path in CI that proves the installed package still
  carries the required `hooks/` payload after `.nupkg` install
- optionally add a narrow package-layout assertion or installed-tool smoke that
  exercises the hook payload without requiring live package/network behavior
- add only the smallest regression coverage needed around the package-layout
  assumption

Expected file cluster:

- `.github/workflows/ci.yml`
- any narrow helper/assertion file needed for package-layout verification
- targeted tests only if they materially improve coverage for the installed
  package layout path

## Deferred To Later Phases

These remain open but are intentionally not mixed into this phase:

- hosted Playwright E2E wiring and Playwright stabilization
- static HTML feature-contract gaps (`--enable-url`, `--show-home`,
  `--enable-nuget-browser`, `--enable-package-upload`)
- frontend code-size policy enforcement and large TSX splits
- installed-tool multi-TFM determinism
- stdout/timeout diagnostic preservation
- architecture-scanner hardening and additional boundary assertions
