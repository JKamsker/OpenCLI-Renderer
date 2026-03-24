# `items remote-search`

- Root: [index](../index.md)
- Parent: [items](index.md)

Search external metadata providers (TMDb, AniDB, IMDB, ...)

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| TERM | Yes | 1 | — | — | Search term |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --all | — | flag | No | No | Declared | — | Auto-page until all results are fetched | — |
| --api-key | — | <KEY> | No | No | Declared | — | API key | KEY · required · arity 1 |
| --config | — | <PATH> | No | No | Declared | — | Path to config file (overrides default location) | PATH · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit JSON instead of table output | — |
| --limit | — | <N> | No | No | Declared | — | Limit result count | N · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Profile name on the resolved host | NAME · required · arity 1 |
| --provider | — | <NAME> | No | No | Declared | — | Search a specific provider (e.g. TheMovieDb, AniDB, "The Open Movie Database") | NAME · required · arity 1 |
| --server | — | <URL> | No | No | Declared | — | Jellyfin server URL or hostname | URL · required · arity 1 |
| --start | — | <N> | No | No | Declared | — | Start index for paged queries | N · required · arity 1 |
| --token | — | <TOKEN> | No | No | Declared | — | Access token | TOKEN · required · arity 1 |
| --type | — | <TYPE> | No | No | Declared | — | Media type to search (Movie, Series, BoxSet, Person, MusicArtist, MusicAlbum, MusicVideo, Book, Trailer) | TYPE · required · arity 1 |
| --user | — | <ID> | No | No | Declared | — | User context for user-scoped commands (id or 'me') | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Show request details | — |
| --year | — | <YEAR> | No | No | Declared | — | Filter by production year | YEAR · required · arity 1 |
| --yes | — | flag | No | No | Declared | — | Skip confirmation prompts | — |

## Examples

- `items remote-search example`
