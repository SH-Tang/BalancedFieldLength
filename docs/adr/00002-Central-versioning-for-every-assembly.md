# Versioning should be centrally administered in the application

## Context and Problem Statement

Each .csproj (CS project file) currently generates its own assembly info with versioning information. To reduce the administrative burden for each potential release, the versioning should be centrally managed within the project solution. 

## Considered Options

* CTRL+F, CTRL+H across the whole solution to adjust the version number for each assembly info file.
* Make use of a `GlobalAssemblyInfo.cs` which is added as a link to the project that needs to be versioned. See: 
  [Source 1]( http://www.technologytoolbox.com/blog/jjameson/archive/2009/04/03/shared-assembly-info-in-visual-studio-projects.aspx)
  [Source 2](https://stackoverflow.com/questions/3768261/best-practices-guidance-for-maintaining-assembly-version-numbers)
* Same as previous option, but make a separate assembly which contains the `GlobalAssemblyInfo.cs` file and add the containing project as a dependency

## Decision Outcome

Chosen option: "`GlobalAssemblyInfo.cs` as a linked file", because 
* The third option is a more complicated option. It is useful when a prebuild step needs to be configured (to set for example the githash for the versioning). However, the versioning project needs to be build first and be included. A brief investigation shows that this approach does not immediately work out of the box and needs to be further investigated before it can be implemented. 
* Proof of concept shows that the chosen option is easily implemented: 
  - Other projects can simply drag and drop already existing links to the `GlobalAssemblyInfo.cs` after it was added 
  - Changes in the `GlobalAssemblyInfo.cs` file are reflected in all assemblies
* Solution suffices for the goal it is needed for: by including the `GlobalAssemblyInfo.cs` file in every solution, the release version can be administered from a central location. 

## Implementation Details
The `GlobalAssemblyInfo.cs` file should preferably be visible from within the application: it is therefore advisable to create a separate project which contains this file. No dependencies to this project should exist, as the dependent projects do not use any class or method from this. 
Additionally, a separate project should be made to unit test the properties that are set by the `GlobalAssemblyInfo.cs` file.  
As a final consideration, the `GlobalAssemblyInfo.cs` file should contain **all** assembly information that are shared by all the other projects, for example the copyright and company information.