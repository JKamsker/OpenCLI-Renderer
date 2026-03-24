# `library folders add`

- Root: [index](../../index.md)
- Parent: [library folders](index.md)

Create a virtual folder

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| NAME | Yes | 1 | — | — | Virtual folder name |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --options-data | — | <JSON> | No | No | Declared | — | Inline JSON with LibraryOptions | JSON · required · arity 1 |
| --options-file | — | <FILE> | No | No | Declared | — | JSON file with LibraryOptions | FILE · required · arity 1 |
| --path | — | <PATH> | No | No | Declared | — | Media path, repeat for multiple paths | PATH · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --refresh | — | flag | No | No | Declared | — | Refresh the library after creating the folder | — |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --type | — | <TYPE> | No | No | Declared | — | Collection type: movies, tvshows, music, musicvideos, homevideos, boxsets, books, mixed | TYPE · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `library folders add demo --path ./sample`
