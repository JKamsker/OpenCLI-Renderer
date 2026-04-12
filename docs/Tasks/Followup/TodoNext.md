# Todo Next Queue

This queue is for tasks that must be picked up by the next follow-up iteration
before the normal fresh investigation swarm proceeds.

Use it for:

- user-injected tasks engineered outside the normal swarm flow
- carry-forward items intentionally queued for the next iteration
- large refactor briefs that need explicit triage before the loop resumes

## Queue Rules

1. Read this file before every new outer iteration.
2. Any item in `Ready`, `Carry`, or `In Progress` must be triaged before
   starting a fresh investigation swarm.
3. A queued item may be handled in one of four ways:
   - `Completed`: the work landed; include commit SHA(s) or a logbook ref.
   - `Deferred`: not done this iteration; add dated rationale and keep it in
     the queue.
   - `Rejected`: not accepted; add dated rationale and a logbook ref.
   - `In Progress`: actively being worked in the current iteration.
4. Do not silently skip queue items. Every non-completed item needs an explicit
   state and rationale.
5. Large items should live in dedicated detail files under `TodoNext/` and be
   linked from the queue row.
6. Update [Logbook](Logbook.md) whenever a queued item materially changes the
   active work, lands, is deferred, or is rejected.

## Active Queue

| ID | State | Added | Source | Summary | Detail | Exit rule |
|---|---|---|---|---|---|---|
| `TN-2026-04-12-01` | `Ready` | `2026-04-12` | User-injected via `tmp.txt` | Finalize the thin-shell architecture: make `InSpectra.Gen` a true shell, move backend logic behind it, and rename `InSpectra.Gen.Acquisition` to an engine-shaped backend if justified by the code. | [2026-04-12-thin-shell-architecture.md](TodoNext/2026-04-12-thin-shell-architecture.md) | Must be triaged before the next fresh investigation swarm. Mark `Completed`, `Deferred`, or `Rejected` with dated rationale. |
