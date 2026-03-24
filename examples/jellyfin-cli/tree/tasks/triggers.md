# `tasks triggers`

- Root: [index](../index.md)
- Parent: [tasks](index.md)

Show task triggers or replace them with 'tasks triggers set <TASK_ID>'

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| TASK_ID_OR_ACTION | Yes | 1 | — | — | Task ID or key, or 'set' to update triggers |
| TASK_ID | No | 1 | — | — | Task ID when using the 'set' action |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --clear | — | flag | No | No | Declared | — | Replace all triggers with an empty list | — |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --daily | — | <TIME> | No | No | Declared | — | Add a daily trigger, repeatable | TIME · required · arity 1 |
| --data | — | <JSON> | No | No | Declared | — | Inline JSON trigger list | JSON · required · arity 1 |
| --file | — | <FILE> | No | No | Declared | — | Load a trigger list from JSON | FILE · required · arity 1 |
| --interval | — | <TIMESPAN> | No | No | Declared | — | Add an interval trigger, repeatable | TIMESPAN · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --max-runtime | — | <TIMESPAN> | No | No | Declared | — | Apply one max runtime to all flag-built triggers | TIMESPAN · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --startup | — | flag | No | No | Declared | — | Add a startup trigger | — |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --weekly | — | <SCHEDULE> | No | No | Declared | — | Add a weekly trigger, repeatable | SCHEDULE · required · arity 1 |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `tasks triggers 12345 12345`
