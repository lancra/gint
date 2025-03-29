[CmdletBinding()]
param()

$rootDirectoryPath = Join-Path -Path $PSScriptRoot -ChildPath '..'

$solutionPath = Get-ChildItem -Path $rootDirectoryPath -Filter '*.slnx' |
    Select-Object -First 1
$relativeSolutionPath = [System.IO.Path]::GetRelativePath($PSScriptRoot, $solutionPath)

$projectPaths = Get-ChildItem -Path $rootDirectoryPath -Filter '*.csproj' -Exclude '*.Dev.csproj' -Recurse |
    Select-Object -ExpandProperty FullName |
    ForEach-Object {
        [System.IO.Path]::GetRelativePath($rootDirectoryPath, $_)
    } |
    Sort-Object

$artifactsDirectoryPath = Join-Path -Path $rootDirectoryPath -ChildPath 'artifacts'
$solutionFilterPath = Join-Path -Path $artifactsDirectoryPath -ChildPath 'gint.slnf'

[PSCustomObject]@{
    solution = [PSCustomObject]@{
        path = $relativeSolutionPath
        projects = $projectPaths
    }
} |
ConvertTo-Json |
Set-Content -Path $solutionFilterPath
