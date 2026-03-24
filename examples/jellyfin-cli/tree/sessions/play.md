# `sessions play`

- Root: [index](../index.md)
- Parent: [sessions](index.md)

Start playback on a session

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| SESSION_ID | Yes | 1 | — | — | Target session id |
| ITEM_IDS | Yes | 1 | — | — | Comma-separated item ids to play |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --audio-stream | — | <INDEX> | No | No | Declared | — | Audio stream index | INDEX · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --play-command | — | <COMMAND> | No | No | Declared | — | Play command: PlayNow (default), PlayNext, PlayLast, PlayInstantMix, PlayShuffle | COMMAND · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --start-position | — | <TICKS> | No | No | Declared | — | Starting position in ticks | TICKS · required · arity 1 |
| --subtitle-stream | — | <INDEX> | No | No | Declared | — | Subtitle stream index | INDEX · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `sessions play 12345 12345 --play-command command`
