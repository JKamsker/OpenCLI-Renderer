param(
  [string]$Ref = 'HEAD',
  [string[]]$SourcePaths = @(
    'src/InSpectra.Discovery.Tool',
    'src/InSpectra.Lib',
    'src/InSpectra.Gen.StartupHook',
    'README.md'
  ),
  [string]$BranchName = '',
  [string]$VersionPrefix = '0.1.0-ci'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$commitSha = (git log -1 --format=%H $Ref -- @SourcePaths).Trim()
if ([string]::IsNullOrWhiteSpace($commitSha)) {
  throw "Unable to resolve a commit that touched the discovery tool package inputs from ref '$Ref'."
}

$commitTimestamp = (git log -1 --format=%cd --date=format:'%Y%m%d%H%M%S' $commitSha).Trim()
$shortSha = $commitSha.Substring(0, [Math]::Min(12, $commitSha.Length)).ToLowerInvariant()

if ([string]::IsNullOrWhiteSpace($BranchName)) {
  return "$VersionPrefix.$commitTimestamp.$shortSha"
}

$branchLabel = $BranchName.Trim().ToLowerInvariant()
$branchLabel = [System.Text.RegularExpressions.Regex]::Replace($branchLabel, '[^a-z0-9-]+', '-')
$branchLabel = [System.Text.RegularExpressions.Regex]::Replace($branchLabel, '-{2,}', '-').Trim('-')

if ([string]::IsNullOrWhiteSpace($branchLabel)) {
  $branchLabel = 'branch'
}
elseif ($branchLabel.Length -gt 24) {
  $branchLabel = $branchLabel.Substring(0, 24).TrimEnd('-')
}

return "$VersionPrefix-$branchLabel.$commitTimestamp.$shortSha"
