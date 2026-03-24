# jdr

- Version: `0.0.0+fbf16bf8f56da0a1132d6148c07252ea9cbc938a`
- OpenCLI: `0.1-draft`

Command-line reference for `jdr`. Available command areas include authentication, system, accounts, grabber, downloads, advanced, and more.

## Table of Contents

- [Overview](#overview)
- [Commands](#commands)
  - [accounts](#command-accounts)
    - [accounts add](#command-accounts-add)
    - [accounts basic-auth](#command-accounts-basic-auth)
      - [accounts basic-auth add](#command-accounts-basic-auth-add)
      - [accounts basic-auth list](#command-accounts-basic-auth-list)
      - [accounts basic-auth remove](#command-accounts-basic-auth-remove)
      - [accounts basic-auth update](#command-accounts-basic-auth-update)
    - [accounts disable](#command-accounts-disable)
    - [accounts enable](#command-accounts-enable)
    - [accounts get](#command-accounts-get)
    - [accounts hosters](#command-accounts-hosters)
      - [accounts hosters list](#command-accounts-hosters-list)
      - [accounts hosters urls](#command-accounts-hosters-urls)
    - [accounts list](#command-accounts-list)
    - [accounts refresh](#command-accounts-refresh)
    - [accounts remove](#command-accounts-remove)
    - [accounts update](#command-accounts-update)
  - [advanced](#command-advanced)
    - [advanced content](#command-advanced-content)
      - [advanced content describe](#command-advanced-content-describe)
      - [advanced content favicon](#command-advanced-content-favicon)
      - [advanced content file-icon](#command-advanced-content-file-icon)
      - [advanced content icon](#command-advanced-content-icon)
    - [advanced dialogs](#command-advanced-dialogs)
      - [advanced dialogs answer](#command-advanced-dialogs-answer)
      - [advanced dialogs get](#command-advanced-dialogs-get)
      - [advanced dialogs list](#command-advanced-dialogs-list)
      - [advanced dialogs type-info](#command-advanced-dialogs-type-info)
    - [advanced ingest](#command-advanced-ingest)
      - [advanced ingest cnl](#command-advanced-ingest-cnl)
    - [advanced raw](#command-advanced-raw)
      - [advanced raw request](#command-advanced-raw-request)
  - [auth](#command-auth)
    - [auth login](#command-auth-login)
    - [auth logout](#command-auth-logout)
    - [auth profiles](#command-auth-profiles)
      - [auth profiles add](#command-auth-profiles-add)
      - [auth profiles get](#command-auth-profiles-get)
      - [auth profiles list](#command-auth-profiles-list)
      - [auth profiles remove](#command-auth-profiles-remove)
      - [auth profiles rename](#command-auth-profiles-rename)
      - [auth profiles use](#command-auth-profiles-use)
    - [auth status](#command-auth-status)
    - [auth whoami](#command-auth-whoami)
  - [captcha](#command-captcha)
    - [captcha forward](#command-captcha-forward)
      - [captcha forward create-job](#command-captcha-forward-create-job)
      - [captcha forward get-result](#command-captcha-forward-get-result)
    - [captcha get](#command-captcha-get)
    - [captcha job](#command-captcha-job)
    - [captcha list](#command-captcha-list)
    - [captcha skip](#command-captcha-skip)
    - [captcha solve](#command-captcha-solve)
  - [device](#command-device)
    - [device direct-info](#command-device-direct-info)
    - [device get](#command-device-get)
    - [device list](#command-device-list)
    - [device ping](#command-device-ping)
    - [device use](#command-device-use)
  - [doctor](#command-doctor)
  - [downloads](#command-downloads)
    - [downloads links](#command-downloads-links)
      - [downloads links list](#command-downloads-links-list)
      - [downloads links remove](#command-downloads-links-remove)
    - [downloads packages](#command-downloads-packages)
      - [downloads packages list](#command-downloads-packages-list)
      - [downloads packages remove](#command-downloads-packages-remove)
    - [downloads pause](#command-downloads-pause)
    - [downloads speed](#command-downloads-speed)
    - [downloads start](#command-downloads-start)
    - [downloads status](#command-downloads-status)
    - [downloads stop](#command-downloads-stop)
    - [downloads stopmark](#command-downloads-stopmark)
      - [downloads stopmark clear](#command-downloads-stopmark-clear)
      - [downloads stopmark get](#command-downloads-stopmark-get)
      - [downloads stopmark set](#command-downloads-stopmark-set)
  - [events](#command-events)
    - [events listen](#command-events-listen)
    - [events poll](#command-events-poll)
    - [events publishers](#command-events-publishers)
    - [events remove](#command-events-remove)
    - [events set](#command-events-set)
    - [events status](#command-events-status)
    - [events subscribe](#command-events-subscribe)
  - [extraction](#command-extraction)
    - [extraction add-password](#command-extraction-add-password)
    - [extraction cancel](#command-extraction-cancel)
    - [extraction info](#command-extraction-info)
    - [extraction queue](#command-extraction-queue)
    - [extraction settings](#command-extraction-settings)
      - [extraction settings get](#command-extraction-settings-get)
      - [extraction settings set](#command-extraction-settings-set)
    - [extraction start](#command-extraction-start)
  - [grabber](#command-grabber)
    - [grabber add](#command-grabber-add)
    - [grabber add-container](#command-grabber-add-container)
    - [grabber clear](#command-grabber-clear)
    - [grabber jobs](#command-grabber-jobs)
      - [grabber jobs get](#command-grabber-jobs-get)
      - [grabber jobs list](#command-grabber-jobs-list)
    - [grabber links](#command-grabber-links)
      - [grabber links list](#command-grabber-links-list)
      - [grabber links remove](#command-grabber-links-remove)
    - [grabber move-to-downloads](#command-grabber-move-to-downloads)
    - [grabber packages](#command-grabber-packages)
      - [grabber packages list](#command-grabber-packages-list)
      - [grabber packages remove](#command-grabber-packages-remove)
    - [grabber variants](#command-grabber-variants)
      - [grabber variants list](#command-grabber-variants-list)
      - [grabber variants set](#command-grabber-variants-set)
  - [settings](#command-settings)
    - [settings config](#command-settings-config)
      - [settings config get](#command-settings-config-get)
      - [settings config list](#command-settings-config-list)
      - [settings config reset](#command-settings-config-reset)
      - [settings config set](#command-settings-config-set)
    - [settings extensions](#command-settings-extensions)
      - [settings extensions disable](#command-settings-extensions-disable)
      - [settings extensions enable](#command-settings-extensions-enable)
      - [settings extensions get](#command-settings-extensions-get)
      - [settings extensions install](#command-settings-extensions-install)
      - [settings extensions list](#command-settings-extensions-list)
    - [settings plugins](#command-settings-plugins)
      - [settings plugins get](#command-settings-plugins-get)
      - [settings plugins list](#command-settings-plugins-list)
  - [system](#command-system)
    - [system info](#command-system-info)
    - [system jd](#command-system-jd)
      - [system jd exit](#command-system-jd-exit)
      - [system jd refresh-plugins](#command-system-jd-refresh-plugins)
      - [system jd restart](#command-system-jd-restart)
      - [system jd revision](#command-system-jd-revision)
      - [system jd uptime](#command-system-jd-uptime)
      - [system jd version](#command-system-jd-version)
    - [system os](#command-system-os)
      - [system os hibernate](#command-system-os-hibernate)
      - [system os shutdown](#command-system-os-shutdown)
      - [system os standby](#command-system-os-standby)
    - [system reconnect](#command-system-reconnect)
    - [system storage](#command-system-storage)
    - [system toggle](#command-system-toggle)
    - [system update](#command-system-update)
      - [system update check](#command-system-update-check)
      - [system update restart](#command-system-update-restart)
      - [system update run](#command-system-update-run)

<a id="overview"></a>
## Overview

### CLI Scope

- Top-level command groups: `12`
- Documented commands: `145`
- Leaf commands: `112`

### Available Commands

- [accounts](#command-accounts) — Manage premium accounts and basic-auth entries.
- [advanced](#command-advanced) — Expert-only escape hatches and raw access.
- [auth](#command-auth) — Authentication, identity, and saved profiles.
- [captcha](#command-captcha) — Inspect and answer captcha jobs.
- [device](#command-device) — Resolve, inspect, and select JDownloader devices.
- [doctor](#command-doctor) — Inspect config paths, resolution, and stored auth state.
- [downloads](#command-downloads) — Inspect and control active downloads.
- [events](#command-events) — Inspect and manage event subscriptions.
- [extraction](#command-extraction) — Inspect and control archive extraction.
- [grabber](#command-grabber) — Manage linkgrabber ingestion and staging.
- [settings](#command-settings) — Inspect config, plugins, and extensions.
- [system](#command-system) — JDownloader, OS, and update operations.


<a id="commands"></a>
## Commands

<a id="command-accounts"></a>
## `accounts`

Manage premium accounts and basic-auth entries.

### Subcommands

- `add`
- `basic-auth`
- `disable`
- `enable`
- `get`
- `hosters`
- `list`
- `refresh`
- `remove`
- `update`

<a id="command-accounts-add"></a>
### `accounts add`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --hoster | — | <NAME> | No | No | Declared | — | Premium hoster name, for example ddownload.com. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password | — | <PASSWORD> | No | No | Declared | — | Account password. | PASSWORD · required · arity 1 |
| --password-stdin | — | flag | No | No | Declared | — | Read the account password from stdin. | — |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --username | — | <NAME> | No | No | Declared | — | Account username or email. | NAME · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-basic-auth"></a>
### `accounts basic-auth`

#### Subcommands

- `add`
- `list`
- `remove`
- `update`

<a id="command-accounts-basic-auth-add"></a>
#### `accounts basic-auth add`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --hostmask | — | <MASK> | No | No | Declared | — | Hostmask for the basic auth entry. | MASK · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password | — | <PASSWORD> | No | No | Declared | — | Basic auth password. | PASSWORD · required · arity 1 |
| --password-stdin | — | flag | No | No | Declared | — | Read the basic auth password from stdin. | — |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --type | — | <TYPE> | No | No | Declared | — | Basic auth type: http or ftp. | TYPE · required · arity 1 |
| --username | — | <NAME> | No | No | Declared | — | Basic auth username. | NAME · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-basic-auth-list"></a>
#### `accounts basic-auth list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-basic-auth-remove"></a>
#### `accounts basic-auth remove`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --basic-auth-id | — | <ID> | No | No | Declared | — | Repeatable basic auth identifier to remove. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-basic-auth-update"></a>
#### `accounts basic-auth update`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --basic-auth-id | — | <ID> | No | No | Declared | — | Basic auth entry id to update. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --hostmask | — | <MASK> | No | No | Declared | — | Hostmask for the basic auth entry. | MASK · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password | — | <PASSWORD> | No | No | Declared | — | Basic auth password. | PASSWORD · required · arity 1 |
| --password-stdin | — | flag | No | No | Declared | — | Read the basic auth password from stdin. | — |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --type | — | <TYPE> | No | No | Declared | — | Basic auth type: http or ftp. | TYPE · required · arity 1 |
| --username | — | <NAME> | No | No | Declared | — | Basic auth username. | NAME · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-disable"></a>
### `accounts disable`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --account-id | — | <ID> | No | No | Declared | — | Repeatable account identifier to disable. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-enable"></a>
### `accounts enable`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --account-id | — | <ID> | No | No | Declared | — | Repeatable account identifier to enable. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-get"></a>
### `accounts get`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --hoster | — | <NAME> | No | No | Declared | — | Premium hoster name to resolve to its account  URL. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-hosters"></a>
### `accounts hosters`

#### Subcommands

- `list`
- `urls`

<a id="command-accounts-hosters-list"></a>
#### `accounts hosters list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-hosters-urls"></a>
#### `accounts hosters urls`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-list"></a>
### `accounts list`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for query-style  endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-refresh"></a>
### `accounts refresh`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --account-id | — | <ID> | No | No | Declared | — | Repeatable account identifier to refresh. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-remove"></a>
### `accounts remove`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --account-id | — | <ID> | No | No | Declared | — | Repeatable account identifier to remove. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-accounts-update"></a>
### `accounts update`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --account-id | — | <ID> | No | No | Declared | — | Account identifier to update. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password | — | <PASSWORD> | No | No | Declared | — | Updated account password. | PASSWORD · required · arity 1 |
| --password-stdin | — | flag | No | No | Declared | — | Read the updated password from stdin. | — |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --username | — | <NAME> | No | No | Declared | — | Updated account username or email. | NAME · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced"></a>
## `advanced`

Expert-only escape hatches and raw access.

### Subcommands

- `content`
- `dialogs`
- `ingest`
- `raw`

<a id="command-advanced-content"></a>
### `advanced content`

#### Subcommands

- `describe`
- `favicon`
- `file-icon`
- `icon`

<a id="command-advanced-content-describe"></a>
#### `advanced content describe`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --key | — | <KEY> | No | No | Declared | — | Icon key to describe. | KEY · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-content-favicon"></a>
#### `advanced content favicon`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --hoster | — | <NAME> | No | No | Declared | — | Hoster name to fetch the favicon for. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --output-file | — | <PATH> | No | No | Declared | — | Destination file for the binary response. | PATH · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-content-file-icon"></a>
#### `advanced content file-icon`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --filename | — | <NAME> | No | No | Declared | — | Filename to fetch an icon for. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --output-file | — | <PATH> | No | No | Declared | — | Destination file for the binary response. | PATH · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-content-icon"></a>
#### `advanced content icon`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --key | — | <KEY> | No | No | Declared | — | Icon key to fetch. | KEY · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --output-file | — | <PATH> | No | No | Declared | — | Destination file for the binary response. | PATH · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --size | — | <PX> | No | No | Declared | — | Icon size in pixels. | PX · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-dialogs"></a>
### `advanced dialogs`

#### Subcommands

- `answer`
- `get`
- `list`
- `type-info`

<a id="command-advanced-dialogs-answer"></a>
#### `advanced dialogs answer`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --data-json | — | <JSON> | No | No | Declared | — | Answer payload as JSON object or @file. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Dialog id to answer. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-dialogs-get"></a>
#### `advanced dialogs get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --icon | — | flag | No | No | Declared | — | Include dialog icon data where available. | — |
| --id | — | <ID> | No | No | Declared | — | Dialog id to fetch. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --properties | — | flag | No | No | Declared | — | Include dialog properties where available. | — |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-dialogs-list"></a>
#### `advanced dialogs list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-dialogs-type-info"></a>
#### `advanced dialogs type-info`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dialog-type | — | <TYPE> | No | No | Declared | — | Dialog type to describe. | TYPE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-ingest"></a>
### `advanced ingest`

#### Subcommands

- `cnl`

<a id="command-advanced-ingest-cnl"></a>
#### `advanced ingest cnl`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password | — | <PASSWORD> | No | No | Declared | — | Optional password passed to the ingest  endpoint. | PASSWORD · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --source | — | <NAME> | No | No | Declared | — | Source label sent to the ingest endpoint. | NAME · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --url | — | <URL> | No | No | Declared | — | URL to add to Linkgrabber via the  Flash/Toolbar ingest endpoint. | URL · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-advanced-raw"></a>
### `advanced raw`

#### Subcommands

- `request`

<a id="command-advanced-raw-request"></a>
#### `advanced raw request`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| PATH | Yes | 1 | — | — | — |

##### Options

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

<a id="command-auth"></a>
## `auth`

Authentication, identity, and saved profiles.

### Subcommands

- `login` — Store encrypted auth material for a profile.
- `logout` — Remove stored auth material for the resolved  profile.
- `profiles` — Manage saved CLI profiles.
- `status` — Show stored auth state for the resolved profile.
- `whoami` — Show the resolved profile and stored account.

<a id="command-auth-login"></a>
### `auth login`

Store encrypted auth material for a profile.

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --email | — | <EMAIL> | No | No | Declared | — | My.JDownloader account email. | EMAIL · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password-stdin | — | flag | No | No | Declared | — | Read the password from stdin without echo. | — |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-logout"></a>
### `auth logout`

Remove stored auth material for the resolved  profile.

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-profiles"></a>
### `auth profiles`

Manage saved CLI profiles.

#### Subcommands

- `add`
- `get`
- `list`
- `remove`
- `rename`
- `use`

<a id="command-auth-profiles-add"></a>
#### `auth profiles add`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| NAME | Yes | 1 | — | — | — |

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-profiles-get"></a>
#### `auth profiles get`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| NAME | Yes | 1 | — | — | — |

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-profiles-list"></a>
#### `auth profiles list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-profiles-remove"></a>
#### `auth profiles remove`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| NAME | Yes | 1 | — | — | — |

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-profiles-rename"></a>
#### `auth profiles rename`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| OLD_NAME | Yes | 1 | — | — | — |
| NEW_NAME | Yes | 1 | — | — | — |

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-profiles-use"></a>
#### `auth profiles use`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| NAME | Yes | 1 | — | — | — |

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-status"></a>
### `auth status`

Show stored auth state for the resolved profile.

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-auth-whoami"></a>
### `auth whoami`

Show the resolved profile and stored account.

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha"></a>
## `captcha`

Inspect and answer captcha jobs.

### Subcommands

- `forward`
- `get`
- `job`
- `list`
- `skip`
- `solve`

<a id="command-captcha-forward"></a>
### `captcha forward`

#### Subcommands

- `create-job`
- `get-result`

<a id="command-captcha-forward-create-job"></a>
#### `captcha forward create-job`

##### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| ARG1 | Yes | 1 | — | — | — |
| ARG2 | Yes | 1 | — | — | — |
| ARG3 | Yes | 1 | — | — | — |
| ARG4 | Yes | 1 | — | — | — |

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha-forward-get-result"></a>
#### `captcha forward get-result`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --job-id | — | <ID> | No | No | Declared | — | Captcha forward job id to retrieve the result  for. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha-get"></a>
### `captcha get`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --format | — | <FORMAT> | No | No | Declared | — | Optional format override. | FORMAT · required · arity 1 |
| --id | — | <ID> | No | No | Declared | — | Captcha identifier to fetch. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha-job"></a>
### `captcha job`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Captcha identifier to fetch as a job object. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha-list"></a>
### `captcha list`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha-skip"></a>
### `captcha skip`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Captcha identifier to skip. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --legacy | — | flag | No | No | Declared | — | Use the deprecated id-only skip overload (no  --type). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --type | — | <TYPE> | No | No | Declared | — | Skip request type (required unless --legacy). | TYPE · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-captcha-solve"></a>
### `captcha solve`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Captcha identifier to solve. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --result | — | <TEXT> | No | No | Declared | — | Captcha solution/result. | TEXT · required · arity 1 |
| --result-format | — | <FORMAT> | No | No | Declared | — | Optional result format. | FORMAT · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-device"></a>
## `device`

Resolve, inspect, and select JDownloader devices.

### Subcommands

- `direct-info`
- `get`
- `list`
- `ping`
- `use`

<a id="command-device-direct-info"></a>
### `device direct-info`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-device-get"></a>
### `device get`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-device-list"></a>
### `device list`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-device-ping"></a>
### `device ping`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-device-use"></a>
### `device use`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --device-name | — | <NAME> | No | No | Declared | — | Optional friendly name when adding a new local  device record. | NAME · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-doctor"></a>
## `doctor`

Inspect config paths, resolution, and stored auth state.

### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and output  settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads"></a>
## `downloads`

Inspect and control active downloads.

### Subcommands

- `links`
- `packages`
- `pause`
- `speed`
- `start`
- `status`
- `stop`
- `stopmark`

<a id="command-downloads-links"></a>
### `downloads links`

#### Subcommands

- `list`
- `remove`

<a id="command-downloads-links-list"></a>
#### `downloads links list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for  query-style endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-links-remove"></a>
#### `downloads links remove`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable download link identifier to  remove. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier whose links  should be removed. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-packages"></a>
### `downloads packages`

#### Subcommands

- `list`
- `remove`

<a id="command-downloads-packages-list"></a>
#### `downloads packages list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for  query-style endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-packages-remove"></a>
#### `downloads packages remove`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable download package identifier to  remove. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-pause"></a>
### `downloads pause`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --resume | — | flag | No | No | Declared | — | Resume downloads instead of pausing them. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-speed"></a>
### `downloads speed`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-start"></a>
### `downloads start`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-status"></a>
### `downloads status`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-stop"></a>
### `downloads stop`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-stopmark"></a>
### `downloads stopmark`

#### Subcommands

- `clear`
- `get`
- `set`

<a id="command-downloads-stopmark-clear"></a>
#### `downloads stopmark clear`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-stopmark-get"></a>
#### `downloads stopmark get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-downloads-stopmark-set"></a>
#### `downloads stopmark set`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Download link id to stop at. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Download package id to stop at. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events"></a>
## `events`

Inspect and manage event subscriptions.

### Subcommands

- `listen`
- `poll`
- `publishers`
- `remove`
- `set`
- `status`
- `subscribe`

<a id="command-events-listen"></a>
### `events listen`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --subscription-id | — | <ID> | No | No | Declared | — | Subscription id to listen on. | ID · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events-poll"></a>
### `events poll`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --subscription-id | — | <ID> | No | No | Declared | — | Subscription id to poll. | ID · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events-publishers"></a>
### `events publishers`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events-remove"></a>
### `events remove`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --exclude | — | <NAME> | No | No | Declared | — | Repeatable exclusion pattern/name to remove. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --subscription | — | <NAME> | No | No | Declared | — | Repeatable publisher subscription name to  remove. | NAME · required · arity 1 |
| --subscription-id | — | <ID> | No | No | Declared | — | Subscription id to update. | ID · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events-set"></a>
### `events set`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --exclude | — | <NAME> | No | No | Declared | — | Repeatable exclusion pattern/name. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --subscription | — | <NAME> | No | No | Declared | — | Repeatable publisher subscription name. | NAME · required · arity 1 |
| --subscription-id | — | <ID> | No | No | Declared | — | Subscription id to update. | ID · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events-status"></a>
### `events status`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --subscription-id | — | <ID> | No | No | Declared | — | Subscription id to inspect. | ID · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-events-subscribe"></a>
### `events subscribe`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --exclude | — | <NAME> | No | No | Declared | — | Repeatable exclusion pattern/name. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --subscription | — | <NAME> | No | No | Declared | — | Repeatable publisher subscription name. | NAME · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction"></a>
## `extraction`

Inspect and control archive extraction.

### Subcommands

- `add-password`
- `cancel`
- `info`
- `queue`
- `settings`
- `start`

<a id="command-extraction-add-password"></a>
### `extraction add-password`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --password | — | <PASSWORD> | No | No | Declared | — | Archive password to add. | PASSWORD · required · arity 1 |
| --password-stdin | — | flag | No | No | Declared | — | Read the archive password from stdin. | — |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction-cancel"></a>
### `extraction cancel`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --controller-id | — | <ID> | No | No | Declared | — | Extraction controller id to cancel. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction-info"></a>
### `extraction info`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable download link identifier to inspect  extraction info for. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier to inspect  extraction info for. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction-queue"></a>
### `extraction queue`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction-settings"></a>
### `extraction settings`

#### Subcommands

- `get`
- `set`

<a id="command-extraction-settings-get"></a>
#### `extraction settings get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --archive-id | — | <ID> | No | No | Declared | — | Repeatable archive identifier to fetch  settings for. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction-settings-set"></a>
#### `extraction settings set`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --archive-id | — | <ID> | No | No | Declared | — | Archive identifier to update. | ID · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --settings-json | — | <JSON> | No | No | Declared | — | Archive settings JSON object or @file. | JSON · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-extraction-start"></a>
### `extraction start`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier to start extraction  for. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier to start extraction  for. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber"></a>
## `grabber`

Manage linkgrabber ingestion and staging.

### Subcommands

- `add`
- `add-container`
- `clear`
- `jobs`
- `links`
- `move-to-downloads`
- `packages`
- `variants`

<a id="command-grabber-add"></a>
### `grabber add`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --assign-job-id | — | flag | No | No | Declared | — | Set AddLinksQuery.assignJobID=true. | — |
| --auto-extract | — | flag | No | No | Declared | — | Set AddLinksQuery.autoExtract=true. | — |
| --autostart | — | flag | No | No | Declared | — | Set AddLinksQuery.autostart=true. | — |
| --deep-decrypt | — | flag | No | No | Declared | — | Set AddLinksQuery.deepDecrypt=true. | — |
| --destination-folder | — | <PATH> | No | No | Declared | — | Optional destination folder override. | PATH · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --links | — | <TEXT> | No | No | Declared | — | Raw newline-separated link text to add. | TEXT · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --overwrite-packagizer-rules | — | flag | No | No | Declared | — | Set AddLinksQuery.overwritePackagizerRules=true. | — |
| --package-name | — | <NAME> | No | No | Declared | — | Optional package name override. | NAME · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw AddLinksQuery JSON object or @file override.  Do not combine with other flags. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --source-url | — | <URL> | No | No | Declared | — | Optional source URL for provenance. | URL · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --url | — | <URL> | No | No | Declared | — | Repeatable URL to add to the linkgrabber. | URL · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-add-container"></a>
### `grabber add-container`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --content | — | <CONTENT> | No | No | Declared | — | Container content (payload string). | CONTENT · required · arity 1 |
| --content-file | — | <PATH> | No | No | Declared | — | Read container content from a local file. | PATH · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --type | — | <TYPE> | No | No | Declared | — | Container type (e.g., DLC, CCF, RSDF). | TYPE · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-clear"></a>
### `grabber clear`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-jobs"></a>
### `grabber jobs`

#### Subcommands

- `get`
- `list`

<a id="command-grabber-jobs-get"></a>
#### `grabber jobs get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --job-id | — | <ID> | No | No | Declared | — | Repeatable crawler job id to fetch. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-jobs-list"></a>
#### `grabber jobs list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --job-id | — | <ID> | No | No | Declared | — | Repeatable crawler job id filter. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-links"></a>
### `grabber links`

#### Subcommands

- `list`
- `remove`

<a id="command-grabber-links-list"></a>
#### `grabber links list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for  query-style endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-links-remove"></a>
#### `grabber links remove`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable linkgrabber link identifier to  remove. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier whose links  should be removed. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-move-to-downloads"></a>
### `grabber move-to-downloads`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable linkgrabber link identifier to move. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable linkgrabber package identifier to  move. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-packages"></a>
### `grabber packages`

#### Subcommands

- `list`
- `remove`

<a id="command-grabber-packages-list"></a>
#### `grabber packages list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for  query-style endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-packages-remove"></a>
#### `grabber packages remove`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable linkgrabber package identifier to  remove. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-variants"></a>
### `grabber variants`

#### Subcommands

- `list`
- `set`

<a id="command-grabber-variants-list"></a>
#### `grabber variants list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Linkgrabber link id to inspect variants for. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-grabber-variants-set"></a>
#### `grabber variants set`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --link-id | — | <ID> | No | No | Declared | — | Linkgrabber link id to update. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --variant-id | — | <ID> | No | No | Declared | — | Variant id to assign. | ID · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings"></a>
## `settings`

Inspect config, plugins, and extensions.

### Subcommands

- `config`
- `extensions`
- `plugins`

<a id="command-settings-config"></a>
### `settings config`

#### Subcommands

- `get`
- `list`
- `reset`
- `set`

<a id="command-settings-config-get"></a>
#### `settings config get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --interface-name | — | <NAME> | No | No | Declared | — | Config interface name. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --key | — | <KEY> | No | No | Declared | — | Config key. | KEY · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --storage | — | <NAME> | No | No | Declared | — | Config storage name. Omit for entries without  a dedicated storage. | NAME · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-config-list"></a>
#### `settings config list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --pattern | — | <TEXT> | No | No | Declared | — | Optional pattern filter. | TEXT · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --return-default-values | — | flag | No | No | Declared | — | Include default values. | — |
| --return-description | — | flag | No | No | Declared | — | Include docs/description fields. | — |
| --return-enum-info | — | flag | No | No | Declared | — | Include enum metadata. | — |
| --return-values | — | flag | No | No | Declared | — | Include current values. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-config-reset"></a>
#### `settings config reset`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --interface-name | — | <NAME> | No | No | Declared | — | Config interface name. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --key | — | <KEY> | No | No | Declared | — | Config key to reset to its default value. | KEY · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --storage | — | <NAME> | No | No | Declared | — | Config storage name. Omit for entries without  a dedicated storage. | NAME · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-config-set"></a>
#### `settings config set`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --interface-name | — | <NAME> | No | No | Declared | — | Config interface name. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --key | — | <KEY> | No | No | Declared | — | Config key to set. | KEY · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --storage | — | <NAME> | No | No | Declared | — | Config storage name. Omit for entries without  a dedicated storage. | NAME · required · arity 1 |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --value | — | <VALUE> | No | No | Declared | — | String value to set. | VALUE · required · arity 1 |
| --value-json | — | <JSON> | No | No | Declared | — | Raw JSON value or @file for non-string  values. | JSON · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-extensions"></a>
### `settings extensions`

#### Subcommands

- `disable`
- `enable`
- `get`
- `install`
- `list`

<a id="command-settings-extensions-disable"></a>
#### `settings extensions disable`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --classname | — | <NAME> | No | No | Declared | — | Extension classname/config interface to  disable (alternative to --id). | NAME · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Extension id to disable. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-extensions-enable"></a>
#### `settings extensions enable`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --classname | — | <NAME> | No | No | Declared | — | Extension classname/config interface to enable (alternative to --id). | NAME · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Extension id to enable. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-extensions-get"></a>
#### `settings extensions get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Exact extension identifier to resolve. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --name | — | <NAME> | No | No | Declared | — | Exact extension name to resolve. | NAME · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-extensions-install"></a>
#### `settings extensions install`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --id | — | <ID> | No | No | Declared | — | Extension id to install. | ID · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-extensions-list"></a>
#### `settings extensions list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for  query-style endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-plugins"></a>
### `settings plugins`

#### Subcommands

- `get`
- `list`

<a id="command-settings-plugins-get"></a>
#### `settings plugins get`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --display-name | — | <NAME> | No | No | Declared | — | Plugin display name. | NAME · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --interface-name | — | <NAME> | No | No | Declared | — | Plugin config interface name. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --key | — | <KEY> | No | No | Declared | — | Plugin config key. | KEY · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-settings-plugins-list"></a>
#### `settings plugins list`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --body-json | — | <JSON> | No | No | Declared | — | Raw body object JSON or @file override. | JSON · required · arity 1 |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --fields | — | <CSV> | No | No | Declared | — | Comma-separated field projection for  query-style endpoints. | CSV · required · arity 1 |
| --hoster | — | <NAME> | No | No | Declared | — | Repeatable hoster selector. | NAME · required · arity 1 |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --limit | — | <NUMBER> | No | No | Declared | — | Maximum number of results. | NUMBER · required · arity 1 |
| --link-id | — | <ID> | No | No | Declared | — | Repeatable link identifier filter. | ID · required · arity 1 |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --offset | — | <NUMBER> | No | No | Declared | — | Result offset. | NUMBER · required · arity 1 |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --package-id | — | <ID> | No | No | Declared | — | Repeatable package identifier filter. | ID · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --query-json | — | <JSON> | No | No | Declared | — | Raw query object JSON or @file override. | JSON · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system"></a>
## `system`

JDownloader, OS, and update operations.

### Subcommands

- `info`
- `jd`
- `os`
- `reconnect`
- `storage`
- `toggle`
- `update`

<a id="command-system-info"></a>
### `system info`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-jd"></a>
### `system jd`

#### Subcommands

- `exit`
- `refresh-plugins`
- `restart`
- `revision`
- `uptime`
- `version`

<a id="command-system-jd-exit"></a>
#### `system jd exit`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-jd-refresh-plugins"></a>
#### `system jd refresh-plugins`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-jd-restart"></a>
#### `system jd restart`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-jd-revision"></a>
#### `system jd revision`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-jd-uptime"></a>
#### `system jd uptime`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-jd-version"></a>
#### `system jd version`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-os"></a>
### `system os`

#### Subcommands

- `hibernate`
- `shutdown`
- `standby`

<a id="command-system-os-hibernate"></a>
#### `system os hibernate`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-os-shutdown"></a>
#### `system os shutdown`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --force | — | flag | No | No | Declared | — | Force OS shutdown (matches  /system/shutdownOS?force parameter). | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-os-standby"></a>
#### `system os standby`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-reconnect"></a>
### `system reconnect`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-storage"></a>
### `system storage`

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --path | — | <PATH> | No | No | Declared | — | Filesystem path to inspect on the remote  JDownloader host. | PATH · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-toggle"></a>
### `system toggle`

#### Arguments

| Name | Required | Arity | Accepted Values | Group | Description |
| --- | --- | --- | --- | --- | --- |
| NAME | Yes | 1 | — | — | Toggle name (e.g. pause-downloads, speed-limit,  premium, clipboard-monitoring, automatic-reconnect, stop-after-current). |

#### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit without  mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract  (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-update"></a>
### `system update`

#### Subcommands

- `check`
- `restart`
- `run`

<a id="command-system-update-check"></a>
#### `system update check`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-update-restart"></a>
#### `system update restart`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |

<a id="command-system-update-run"></a>
#### `system update run`

##### Options

| Name | Aliases | Value | Required | Recursive | Scope | Group | Description | Arguments |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| --device | — | <VALUE> | No | No | Declared | — | Device id or exact device name override. | VALUE · required · arity 1 |
| --dry-run | — | flag | No | No | Declared | — | Print the resolved request plan and exit  without mutating. | — |
| --json | — | flag | No | No | Declared | — | Emit the default stable JSON envelope contract (v1). | — |
| --no-color | — | flag | No | No | Declared | — | Disable ANSI color output. | — |
| --output | — | <MODE> | No | No | Declared | — | Output mode override: human or json. | MODE · required · arity 1 |
| --profile | — | <NAME> | No | No | Declared | — | Saved profile to use for auth, defaults, and  output settings. | NAME · required · arity 1 |
| --quiet | — | flag | No | No | Declared | — | Suppress prompts and non-essential stderr  chatter. | — |
| --timeout | — | <SECONDS> | No | No | Declared | — | Timeout override in seconds. | SECONDS · required · arity 1 |
| --verbose | — | flag | No | No | Declared | — | Increase diagnostic detail on stderr. | — |
| --yes | -y | flag | No | No | Declared | — | Skip confirmation prompts for destructive  operations. | — |
