# Refactor the MVVM components within the Application.BalancedFieldLength assembly
**Note:** This entry only considers the refactoring the MVVM components within the namespace. For an entry about the refactoring and managing the generic components, please refer to the follow-up entry [ADR-00008](00008-Refactor-Components.md).

## Context and Problem Statement
An intuitive way needs to be found in order to manage the View and the ViewModel hierarchy. Additionally, one should also keep in mind that there are two additional assemblies on which the ViewModels might depend on, namely:
* A Data assembly which serves as a data transfer assembly to transfer data from the GUI to the wrapper around the simulation kernel
* The wrapper around the simulation kernel

Therefore, the following questions need to be answered:
* How to group the View and the ViewModels within the `Application.BalancedFieldLength` in a coherent way? 
* Considering the outcome of [ADR-00008](00008-Refactor-Components.md), how to group the View and the ViewModels in a coherent way, if different from the approach in the first bullet?

## Considered options
On the internet, there are multiple approaches, namely:
* Make separate folders for the View and the ViewModels 
* Group the View and ViewModels together as a single use case 
* Drop the View and ViewModels folder

[Source1](https://stackoverflow.com/questions/18825888/project-structure-for-mvvm-in-wpf)
[Source2](https://www.reddit.com/r/csharp/comments/3rmhqz/wpf_mvvm_good_structure_for_views_and_view_models/)

### How should the View and ViewModels be managed in the Application.BalancedFieldLength specific components?
In the current situation, there are two types of views defined, namely:
* Views and ViewModels that are not part of elements from the WPF common components. An example of this, is at this moment the output view. This view is an application specific view which is an UserControl by itself 
* Views and ViewModels that are part of a generic GUI component, such as the TabViews. These views are defined for the TabControlViewModel. 

Based on these two criteria, there should be at least one level of solution folder indicating for which component (if they're part of a generic component) the items belong. One proposed structure could be: 
```
| Application.BalancedFieldLength
| -- Tabs
| ---- Tab1ViewModel
| ---- Tab1View
| ---- Tab2ViewModel
| ---- Tab2View
| -- Output
| ---- OutputView
| ---- OutputViewModel
```
However, a potential problem would be in case components of the `Tabs` and `Outputs` smake use of shared components that are only relevant within the Application.BalancedFieldLength assembly. A solution could be by introducing an additional layer on top of both, resulting in: 

```
| Application.BalancedFieldLength
| -- WPFComponents
| ---- [Some shared components]
| ---- Tabs
| ------ Tab1ViewModel
| ------ Tab1View
| ------ Tab2ViewModel
| ------ Tab2View
| ---- Output
| ------ OutputView
| ------ OutputViewModel
```
In this situation, it would not make sense anymore to create an additional View solution folder within the components as that introduces another layer. Considering that the Views and  the ViewModels are tightly coupled, it doesn't make sense to separate these from each other. 

It should be noted that regardless of which approach is chosen, the components are located within the integration platform project assembly. Therefore, there is no problem when the `Application.BalancedFieldLength` assembly refers to the `Data` assembly and the wrapper assembly. As long as the latter two assemblies do not have a reference towards the `Application.BalancedFieldLength` assembly, independence between the modules can be guaranteed. 

### How should the View and ViewModels be managed in the generic components?
For the generic components, it makes sense to go for the second approach. As the components are generally independent from other (GUI) related components, it makes sense to group everything together. Because the  View, ViewModel and potentially the model itself are all tightly related to each other, the proposed grouping will have the following structure: 
```
| JustAComponent
| - JustAComponentModel.cs
| - JustAComponentViewModel.cs 
| - JustAComponentView.cs
```

This has the following advantages:
* It makes the reuse of the component easier in different solutions. When this component needs to be reused, the contents of the solution folder can simply be copied over 
* In the foreseen circumstance that it is preferred to separate components to their own project assembly, this structure facillitates this proces: simply move all the contents of the solution folder to the new assembly. 

In case there are usages of subcomponents, such as commands, within the generic component, one could consider introducing additional solution folders. 

## Decision Outcome

### How should the View and ViewModels be managed in the Application.BalancedFieldLength specific components?
Considering the possibility that there might be shared components between the various View and ViewModel definitions, the following structure is adopted: 
```
| Application.BalancedFieldLength
| -- WPFComponents
| ---- [Some shared components]
| ---- Tabs
| ------ Tab1ViewModel
| ------ Tab1View
| ------ Tab2ViewModel
| ------ Tab2View
| ---- Output
| ------ OutputView
| ------ OutputViewModel
```

This structure promotes:
* The coherence of the various component and what they are meant for. Each solution folder represents one component or for which component they are intended for. 
* In case the components should be moved to their own project assembly, the contents of a single solution folder can simply be copied over. 

### How should the View and ViewModels be managed in the generic components?
Similar to the specific components, the View and ViewModels are grouped in a solution folder based on their functionality. This means that the proposed solution should look like: 
```
| JustAComponent
| - JustAComponentModel.cs
| - JustAComponentViewModel.cs 
| - JustAComponentView.cs
```

The advantages of this approach are similar to the advantages of grouping specific components in the `Application.BalancedFieldLength` assembly.