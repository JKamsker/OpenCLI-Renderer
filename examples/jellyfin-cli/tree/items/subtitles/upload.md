# `items subtitles upload`

- Root: [index](../../index.md)
- Parent: [items subtitles](index.md)

Upload an external subtitle file

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| ITEM_ID | Yes | 1 | — | — | Item ID |
| FILE | Yes | 1 | — | — | Subtitle file path |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --forced | — | flag | No | No | Declared | — | Mark the subtitle as forced | — |
| --format | — | <FORMAT> | No | No | Declared | — | Subtitle format, defaults to the file extension | FORMAT · required · arity 1 |
| --hearing-impaired | — | flag | No | No | Declared | — | Mark the subtitle as hearing impaired | — |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --language | — | <CODE> | No | No | Declared | — | Subtitle language code | CODE · required · arity 1 |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `items subtitles upload 12345 sample.json --language en`
