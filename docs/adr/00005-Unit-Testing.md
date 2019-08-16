# Devise a testing strategy for the GUI related components

## Context and Problem Statement

For the window (GUI) based application, a new C# project needs to be set up. As was decided to go for a WPF application with an MVVM approach, an unit testing strategy can be devised. In short:
* What are we going to unit test? 
* With what are we going to unit test? 

## Considered Options
For the "With what" question:
* Use NUnit in the classical form 
* Use NUnit with the constraint type

The following mocking frameworks are available:
* RhinoMocks
* NSubstitute
* Moq

For the "What" question:
Based on [ADR-00004](00004-GUI.md), there are basically 3 components involved with a MVVM approach, namely:
* The view itself
* The viewmodel wich contains the logic of the view 
* The data 

## Decision Outcome
Based on the foregoing information, the following was decided: 

### Testing framework:
For the new project, it is advisable to use the "Constraint" model assertions by NUnit. According to the [NUnit documentation](https://github.com/nunit/docs/wiki/Assertions):
> In NUnit 3.0, assertions are written primarily using the Assert.That method, which takes constraint objects as an argument. We call this the Constraint Model of assertions.
> 
> In earlier versions of NUnit, a separate method of the Assert class was used for each different assertion. This Classic Model is still supported but since no new features have been added to it for some time, the constraint-based model must be used in order to have full access to NUnit's capabilities.

Hence, it is decided that when tests are written for the *new* projects, these will be written according to the "Constraint Model." Do note that the current code makes use of the "Classic Model." No effort will be spend to convert the current code to the new standard and will be written in the "Classic Model" in order to maintain consistency.

### Mocking framework
RhinoMocks is not maintained and current code makes use of NSubstitute. There are no strong arguments to deviate from the currently used mocking framework. Therefore, NSubstitute will be used. 

### What should be tested?
The viewmodel and the data objects that are being used by viewmodel should be fully testable. The GUI logic (how items are displayed) is part of the view and is not fully testable. Therefore, it is desired to fully test: 
* The ViewModel
* The data 

Additionally, there will probably be an integration platform present which configures all the various components together. To prevent large dependencies in the unit tests, the following guideline is followed: 
* Individual units will be fully tested 
* When a component makes use of an atomic unit: a subset of the testcases of the atomic unit will be used to indicate that it's being used. The subset should consist of at least one "successfull" and one "failing" behaviour, if present. 
* The integration platform does not need to be fully covered by unit tests. As the individual unit tests prove the stability of the individual components, and there's full control in how to configure the integration platform, there's no necessity to fully test this platform. 
