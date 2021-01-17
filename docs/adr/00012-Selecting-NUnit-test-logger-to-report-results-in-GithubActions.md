# Selecting NUnit test logger to report results in GithubActions

## Context and Problem Statement
After migrating the projects to the SDK format, it is possible to build, restore and run the unit tests in a CI/CD approach in combination with GithubActions. However, the failing tests on GithubActions are not easily determined without scrolling through the whole run log. 

## Considered options

There are many options available, the following were considered and a proof of concept was implemented and tested in a branch to determine the results.

### GitHubActionsTestLogger
This test logger can be easily installed by NuGet. No further changes in the code are necessary and no explicit result directory needs to be pinpointed.

**Pros**
* Easily installed with a NuGet package, no further code changes necessary besides specifying the logger when running the test.
* The results are presented in a clear way when the tests have failed. The annotations are present at the end of the run log which clearly indicate which tests failed during the run. It also provides an inline annotation in the source files to indicate which tests failed.
* Will give a warning when it is being run in a non-GithubActions system environment.

**Cons**
* Does not present a full test report of the total amount of tests. Only reports when tests are failing.

### NUnit Test Reporter
The NUnit Test reporter is an additional step in the workflow. It harvests the .XML test result files and provides inline annotation in the test files.

**Pros**
* Provides an inline annotation of failing tests.
* Presents the test results about which tests have failed at the end of a run.

**Cons**
* An additional reference is required, namely the `NunitXml.TestLogger` A drawback with this logger is that the assemblies are placed in subdirectories and that the test report names are all the same. The test result directories should be piped in a central spot post-run. They are also not cleaned up automatically.
* Only a single path can be defined where the XML files are located
* Even when there is a single XML file present, it seems that the workflow action cannot parse the XML file, as the amount of tests passed are reported to be 0. (Could be caused due to a wrong configuration)
* Not actively maintained.
* Similar to the GitHubActionsTestLogger, it does not generate a full test report at the end of a run. 


### Publish Unit Tests
This option provides the most features. It generates a report (while comparing it against previous commits), including the time of the unit tests results. 

**Pros**
* Contains the most features in terms of reporting, such as test failures, the amount of unit tests between commits and test run time

**Cons**
* Similar to the Nunit Reporter, this action requires an additional reference of the `NunitXml.TestLogger` A drawback with this logger is that the assemblies are placed in subdirectories and that the test report names are all the same. The test result directories should be piped in a central spot post-run. They are also not cleaned up automatically after a run.
* No inline annotation with failed tests
* Only works on Ubuntu environments as it requires a docker container

## Decision outcome

Based on the pros and cons, the GitHubActionsTestLogger was incorporated within the solution. Although it is not entirely excluded to combine both the GitHubActionsTestLogger in combination with the Publish Unit Test Results in the future.

## Resources

* [GitHubActionsTestLogger](https://github.com/Tyrrrz/GitHubActionsTestLogger)
* [NUnit Reporter](https://github.com/marketplace/actions/nunit-reporter)
* [Publish Unit Test Results](https://github.com/marketplace/actions/publish-unit-test-results)

## Instructions for setting up GitHubActionsTestLogger with NUnit3 and dotnet CLI
GithubActions requires the following NuGet dependencies for the unit test projects:
* NUnit3
* NUnit3TestAdapter 
This dependency allows the unit tests to run with the integrated test window within Visual Studio. As a result, it is not necessary anymore to make use of external runners such as the Resharper test runner
* Microsoft.NET.Test.Sdk
This dependency is necessary to run the unit tests within the integrated test window and with the `dotnet test` command
* GitHubActionsTestLogger

Note that the older versions of ReSharper unit test runner has issues with a different version of the NUnit3TestAdapter. Updating Resharper to version 3.2, build 2020-12-29 in combination with NUnit3TestAdapter 3.17.0 resolves this issue.
