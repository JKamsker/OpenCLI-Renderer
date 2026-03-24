# `items explain-latest`

- Root: [index](../index.md)
- Parent: [items](index.md)

Explain why an item is or is not visible in latest

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| ID | Yes | 1 | — | — | Item ID |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --parent | — | <ID> | No | No | Declared | — | Override the parent library or folder used for latest ranking | ID · required · arity 1 |
| --probe-limit | — | <N> | No | No | Declared | — | How many latest items to inspect when computing rank | N · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --visible-limit | — | <N> | No | No | Declared | — | Visible shelf size to compare against | N · required · arity 1 |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `items explain-latest 12345`
