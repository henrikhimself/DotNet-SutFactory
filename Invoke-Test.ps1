#!/usr/bin/env pwsh
#requires -Version 7
Set-StrictMode -Version Latest
$InformationPreference = 'Continue'

<# Global #>
[string]$UpdateTestReportScript = Join-Path $PSScriptRoot 'scripts' 'Update-TestReport.ps1'
[string]$SourceSolutionPath = Join-Path $PSScriptRoot 'SutFactory.sln'
[string]$TestReportPath = Join-Path $PSScriptRoot 'TestReport'

<# Main #>
& $UpdateTestReportScript -TestPath $SourceSolutionPath -ReportDirectoryPath $TestReportPath
