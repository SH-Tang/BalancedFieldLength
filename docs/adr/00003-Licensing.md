# License should be applied to the application

## Context and Problem Statement

It should be clear that this software application does *not* have any liability or warranty when it is being used. Additionally, it is possible that for future purposes, parts of other open software projects will be included and the license should not conflict. 

To prevent copyright claims and clearly communicate what users can and cannot do with (parts) of the source code, a license should be incorporated.

## Considered Options

* MIT
* GPLv3
* LGPLv3

  [Source 1](https://choosealicense.com/licenses/)
  [Source 2](https://opensource.guide/legal/)

## Decision Outcome

Chosen option: "GPLv3", because 
* It forces every reuse, modifications and usage of the sourcecode of this application is made publicly available. 
* It forces the copyright and license mentioning when parts are incorporated in other applications. 
* GPLv3 and LGPLv3 are compatible with eachother. LGPLv3 will be used when there are dependencies to third (closed source) party components. Only the interface that communicates with the third party (closed source) component needs to be publicly available and licensed under the LGPLv3. Other parts of the application can be licensed under GPLv3 otherwise.