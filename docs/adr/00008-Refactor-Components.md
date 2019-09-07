# Refactor the Application.BalancedFieldLength assembly into reusable components
**Note:** This entry only considers the refactoring of the namespace by separating the various components. For an entry about the refactoring and managing the MVVM specific components, please refer to the follow-up entry [ADR-00009](00009-Refactor-MVVM-Components.md).

## Context and Problem Statement
The frontend of the application is currently developed as a monolith which means that:
* Reusability becomes harder, as the entire assembly needs to be imported while only a part of the functionality is requested.
* References between different folders/elements might arise, while they're not legit. (for example, a reference from the Data layer to the ViewModel, while it should be the other way around)

It is therefore desired to split up the current implementation in various components that are related to each other. The current architecture in the Application is now:

```
| Application
| -> Application.BalancedFieldLength
| ---> Controls
| ---> Converters
| ---> WPFCommon
| Core
| Demo
| Simulator
```
where:
* Core contains the shared components between the simulator implementation and the (frontend) of the Application
* Demo contains a console demo application which serves as a proof of concept
* Simulator contains the simulation implementation itself

The current situation is that there are the following components that are implemented in a reusable way, namely:
* The message window component
* The tab control
* The converters
* The entire `WPFCommon` directory as this contains the base class implementation of the `INotifyPropertyChanged` interface

The following questions need to be answered:
* Where should these components reside, while keeping in mind that these components might have a reference to the data components residing in `Core`?
* Where should the application specific components reside, while these will highly likely have a reference to components defined in the assembly of the previous bullet point?

## Considered options

### Where should the generic components reside?
The generic components could be placed in two locations, each with their advantages and disadvantages. However, do note that the WPF components cannot be used without their `Core.Common` assemblies, regardless of the solution. Bullet point 1 creates a constraint, namely that there is a potential reference between the `WPF.Components` and the `Core.Common` assemblies. This constraint implies that whenever the generic WPF components are to be reused in another application, they have to incluce their referred `Core.Common` assemblies.

#### Within the `Core` directory
The first proposal is to place the components within the `Core` directory, resulting in the following structure:
```
| Core
| -> Core.Common.Data
| -> Core.Common.Geometry
| -> Core.Common.Utils
| -> Core.Common.Version
| -> Core.WPF.Components - This contains the component elements, such as the converter, tab control etc
| -> Core.WPFCore / Core.WPF.Core - This contains the basic definitions, such as the ViewModelBase and other (base) implementations
```

##### Pros
* Every generic component is located within one solution folder, namely `Core`
* References can be managed in a relatively intuitive way. The `Core.WPF.<AssemblyName>` can have references to the `Core.Common` elements.
* Naming can be dealt with additional prefix, such as: `Core.WPF.<AssemblyName>`

Additionally,  note that the `WPF.Components` are separated from the base definitions that are contained in `WPF.Core` definitions. This approach was taken, to encourage the reusability of the WPF Components in case an assembly does not require the component definitions.
One could also opt to merge these definitions, but that means that for every new application, every component will be referred to as well. This inclusion might be unnecessary.

##### Cons
There are a few cons with this approach, one being that the naming is not concise. The 'WPFCore' will be prefixed with core and a name as 'Core.WPF.Core' does not seem descriptive enough.

The second concern is that the `Core` directory might get polluted with all types of generic components that may or may not have direct relations. For example, if a generic writer would be written, this creates the precedence that an assembly like `Core.IO.<GenericWriter>` should be placed here as well. The question is whether this is desirable, as the `Core` library now mainly contains generic datatypes which can be reused through the `Simulator` assembly and the `Application` assembly.

#### Besides the `Core` directory
The second solution is to create a solution folder besides the already existing folders. This would result in the following structure:
```
| Application
| -> Application.BalancedFieldLength
| Core
| WPF
| ---> WPF.Components - This contains the component elements, such as the converter, tab control etc
| ---> WPF.Core - This contains the basic definitions, such as the ViewModelBase and other (base) implementations
| Demo
| Simulator
```

##### Pros
* Whenever a WPF component needs to be added, this can be introduced besides the already existing assemblies
* This approach also allows further separation without deterring the coherence of the solution folder. Theoretically, each reusable component could be separated in its own assembly, for example `WPF.Component.TabControl.` In the meantime, the folder still contains only WPF components

##### Cons
There are two cons with this approach, namely that the introduction of another solution folder might bloat the solution, making it harder to understand. The second consideration is that this approach creates the precedence that each component might introduce a separate solution folder.

### Where should the specifc components reside?
It needs to be assumed that the application specific GUI components, such as the viewmodel and the corresponding views, have a relation with the WPF components in defined in the previous section. Therefore, there's a possibility that the GUI components will refer to this component assembly. 

There are in fact two approaches possible. Considering that it is highly likely that a `Data` assembly will be introduced to communicate between the application layer and the service / wrapper layer with the `Simulation` kernel, one could argue that the view specific components should get their own assembly besides the `Application.BalancedFieldLength` assembly. In this situation, the `Application.BalancedFieldLength` basically becomes the integration platform which consumes all the other assemblies to configure the application.

On the other hand, the view components can all be seen as application specific elements. The viewmodel might be potentially decoupled, but at this moment there's a tight coupling between the views and the viewmodels (and in the future between the viewmodels and the data objects). This perspective implies that the views and their viewmodels should not be separated from the `Application.BalancedFieldLength` assembly, as they are all a realisation of how the application should look like.

Regardless of which solution is chosen, the flow of outgoing and ingoing dependencies can be maintained by the separation. In both cases, the `Data` assembly should not have a reference to the introduced assembly. 

## Decision Outcome

### Where should the generic components reside?
Based on the pros and cons, it is desired to separate the WPF components in their own solution folder and the WPFCore components will be separated from the WPF Components. This approach prevents:
* That the coherence will be broken within the `Core` solution folder. However, one might argue whether in this situation the `Common` prefix is still necessary.
* That the `Core` solution folder will be bloated with other generic components, such as IO related components. It also prevents the bloating when separating the components in separate solutions (e.g `Core.WPF.Components.TabControl` / `Core.WPF.TabControl` along with the other generic components)
* Duplicate naming, such as `Core.WPFCore` or `Core.WPF.Core`. Additional prefixes is not necessary with this proposal.

Based on the findings, the structure will be:
```
| Application
| -> Application.BalancedFieldLength
| Core
| WPF
| ---> WPF.Components 
| ---> WPF.Core
| Demo
| Simulator
```

### Where should the specifc components reside?
There is currently no need to split up the application specific GUI components in a separate assembly. The in- and outgoing references can already be managed when the `Data` assembly is introduced (this assembly should be a stand alone assembly and should only have consumers).  Additionally, it is always possible to separate the components when it's necessary. 

It is therefore decided that the application specific GUI components stay within the `Application.BalancedFieldLength` assembly.
