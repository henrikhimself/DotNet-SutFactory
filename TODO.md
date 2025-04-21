# ToDo.
* Change vscode "view test report" task to use a script (eg. View-TestReport.ps1).
    * Should extract directory path code from Update-TestReport.ps1 into shared functions.
    * Ensure script works cross-platform.
* Add Invoke-Benchmark.ps1 for consistent execution.
    * Consider CSV with some sort of merge script and save into a "history" directory.
* Fix cobertura exclusion incorrectly including the TestCase project.
* Fix error "Unable to find a datacollector with friendly name 'XPlat Code Coverage'" when running test.
* Consider reorg "test" directory to avoid duplicate package references in UnitTest and Example projects.
