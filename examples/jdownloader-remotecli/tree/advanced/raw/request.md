# `advanced raw request`

- Root: [index](../../index.md)
- Parent: [advanced raw](index.md)

## Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| PATH | Yes | 1 | — | — | — |

## Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body JSON or @file. | JSON · required · arity 1 |
| --destructive | — | flag | No | No | Declared | — | Mark this call as destructive and require  confirmation (unless -y/--yes). | — |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --method | — | <METHOD> | No | No | Declared | — | My.JDownloader relay calls are always POST.  Only POST is accepted. | METHOD · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --output-file | — | <PATH> | No | No | Declared | — | Destination for binary response modes. | PATH · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query JSON or @file. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |
