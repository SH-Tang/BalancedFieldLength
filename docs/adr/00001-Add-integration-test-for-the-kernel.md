# Add integration test for the kernel

## Context and Problem Statement

Integration tests should be added to verify the consistency of the results when calculations are performed by the kernel. Where should these tests be located?

## Considered Options

* Place integration tests in the same Kernel.Test project
* Place integration tests in a separate class library

## Decision Outcome

Chosen option: "Separate Class library", because 
* Separating the integration tests from the unit tests creates coherence
* Separation prevents polution and dependencies to other (unnecessary) references that are not required by the unit tests of the kernel. 