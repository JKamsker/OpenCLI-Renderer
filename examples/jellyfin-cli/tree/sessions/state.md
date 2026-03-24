# `sessions state`

- Root: [index](../index.md)
- Parent: [sessions](index.md)

Send pause/resume/stop/seek commands

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| ACTION | Yes | 1 | — | — | Playstate action: pause, resume, stop, next, prev, seek, rewind, ff, toggle |
| SESSION_ID | Yes | 1 | — | — | Target session id |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --position | — | <TICKS> | No | No | Declared | — | Seek position in ticks (required for seek) | TICKS · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `sessions state action 12345 --position 1`
