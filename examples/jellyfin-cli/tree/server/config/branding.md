# `server config branding`

- Root: [index](../../index.md)
- Parent: [server config](index.md)

View or update branding configuration

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --custom-css | — | <TEXT> | No | No | Declared | — | Override the custom CSS | TEXT · required · arity 1 |
| --data | — | <JSON> | No | No | Declared | — | Inline branding JSON | JSON · required · arity 1 |
| --file | — | <FILE> | No | No | Declared | — | Read branding JSON from a file | FILE · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --login-disclaimer | — | <TEXT> | No | No | Declared | — | Override the login disclaimer | TEXT · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --splashscreen-disabled | — | flag | No | No | Declared | — | Disable the splash screen | — |
| --splashscreen-enabled | — | flag | No | No | Declared | — | Enable the splash screen | — |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `server config branding --file sample.json`
