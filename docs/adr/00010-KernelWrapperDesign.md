# Kernel wrapper design

## Context and Problem Statement
The `Application` namespace should be decoupled from the simulation `Simulator.Kernel` implementation and vice versa. A wrapper assembly should be introduced for this purpose, but for unit testing purposes it is also desirable to prevent the kernel from running. An additional problem is that the `Simulator.Kernel` implementation currently calculates one distance each time it is executed.

In short, the problem consists of:

* A wrapper assembly should be created, but its implementation should be flexible enough to mock the calculation instead of running them.
* The wrapper should also provide the ability to calculate multiple failure velocities at once to determine the balanced field length.

## Considered options
The problem is split into two separate sections for discusion. The first section tackles the problem on how to perform the calculation for multiple failure velocities. The wrapper design is discussed in the second section and depends on the outcome of how the calculation is to be performed for multiple failure velocities at once.

### Calculation of multiple distances
There are multiple solutions possible, namely:

* Extend the simplementation with functionality to calculate multiple distances.
* Create a class to wrap around the current implementation and execute the calculations for the multiple distances.

Both options are possible, as the whole code base is available and under our control. However, the first solution would shift the responsibility to the `Simulator.Kernel` implementation which could reduce the flexibility. If this implementation were to be chosen, the consumers will not be able to control the individual calculations anymore. The major drawback is that the consumers cannot generate log messages or other information for each individual calculation. In short, `Simulator.Kernel` would dictate the functionality on HOW the calculation should be performed. However, the consumer of the component is still able to generate their own calculation, as the implementation for individual failure velocities is still defined in the public API.

The second implementation would shift the calculation procedure to the consumer side, allowing more flexibility on how the consumer should handle each individual calculation. The consumer of the component would be able to configure the handling of each individual failure velocity, potentially able to provide more information when needed. This option will mean that the consumer is fully responsible in configuring and using the components of the `Simulator.Kernel` accordingly.

The current conclusion is to go for the second option. While the first option would extend the functionality of the `Simulator.Kernel` and _potential_ consumers, there is no added benefit. This option would result in duplicate code, as the `Simulator.Kernel` would be able to calculate and in the wrapper if it is necessary to define additional behaviour for each individual failure velocity calculation. Do keep in mind that this code duplication is legimate, due to the fact that the responsibility is different between the wrapper assembly and the `Simulator.Kernel` assembly. Besides code duplication, it is uncertain whether this additional functionality is needed at all for the `Simulator.Kernel`. Therefore, the second option has the least drawback, as:

* No code duplication, therefore less code maintenance.
* More flexibility for the component consumers as they can define their own behaviour on what should happen during the calculation of each failure velocities.
* No bloating of the public API of the `Simulator.Kernel` assembly as it is uncertain whether the functionality of calculating multiple failure velocities at once is ever going to be needed. (Adhering to the *YAGNI* principle).

### Defining the wrapper
The wrapper has the important functionality to separate the application domain from the simulation (kernel) domain. Additionally, the `Simulator.Kernel` assembly exposes the interface `IAggregatedDistanceCalculator` which allows mocking the behaviour of the kernel without actually performing the calculation when necessary. One option is to use the  self-shunt pattern as described by *"Agile Principles, Patterns, and Practices in C#"* by Robert C. Martin and Micah Martin. This pattern makes use of interfaces to mock or retrieve additional information during a test. The mocked interface implementation can then be either injected directly in the composite (aggregate) object or used as a test class within a test. As the `IAggregatedDistanceCalculator` is exposed, it is possible to create a mocked alternative which allows the consumer to verify its behaviour based on:

* Inputs that are passed as function arguments
* Exceptions that are thrown by the interface implementation and how the consumer handles them
* Outputs and how the consumer deal with these

If this were to be used in the proposed `CalculationModule` by the previous section, the module would look like this:

```
public CalculationModule(IEnumerable<IAggregatedDistanceCalculator> calculators){} // Where calculators contain the set of failure velocities it needs to compute
```

This means that in the integration platform, there will be a dependency to the `Simulator.Kernel`. This is a situation that should be preferably prevented as the purpose of the wrapper is to eliminate the `Simulator.Kernel` from crossing the simulation domain to the application domain. As such, the set of calculators should not be injected in the form of dependency injection. 

An alternative would be to use a composite class: 

```
private readonly IEnumerable<IAggregatedDistanceCalculator> calculators;
public CalculationModule()
{
    calculators = CreateCalculators(); // Where CreateCalculators returns a collection of AggregatedDistanceCalculators defined in the Simulator.Kernel
}
```

The main drawback of this composite construction is that it is impossible to test the calculator behaviours, as there is no entry point to stub the `IAggregatedDistanceCalculator` in this construction. A way to circumvent this limitation is to define the factory for the `IAggregatedDistanceCalculator` instances as a singleton and ONLY in specific situations let the instance be replaced by a test implementation of the factory which returns the mocked implementations of `IAggregatedDistanceCalculator`. Another possibility is the use of a Dependency Injection Framework (DI) to inject items in a private field. However, this would require a dependency of a framework which is capable to do such and is generally a bad practice due to:

* It breaks encapsulation, especially when Reflection is used to set the private field
* If private fields are not an option, the DI should happen by means of a constructor argument injection or a setter injection. The latter would result in a `CalculationModule` as the previous situation. However, it is perhaps possible to use the DI framework to move the dependency from the application domain to the DI framework.

As there is not enough knowledge available regarding the use of DI frameworks, it is not the recommended solution. Instead, the only option for now is the singleton factory, which will look like:

```
interface IAggregatedDistanceCalculatorFactory
{
    IAggregatedDistanceCalculator CreateCalculator();
}

AggregatedDistanceCalculatorFactory : IAggregatedDistanceCalculatorFactory
{
    private readonly AggregatedDistanceCalculatorFactory instance;
    
    private AggregatedDistanceCalculatorFactory(){}
    
    IAggregatedDistanceCalculator Instance{
        get
        {
            return instance ?? instance = new AggregatedDistanceCalculatorFactory(); 
        }
        internal set
        {
            instance = value;
        }
    }
    
    IAggregatedDistanceCalculator CreateCalculator () 
    {
        // Create the kernel specific calculator
    }
}

TestAggregatedDistanceCalculatorFactory : IAggregatedDistanceCalculatorFactory
{
    // Additional information can be stubbed here

    IAggregatedDistanceCalculator CreateCalculator () 
    {
        // Create the a stubbed calculator
    }
}
```

With this construction, it becomes clear that there needs to be a class which is able to access the internal setter `AggregatedDistanceCalculatorFactory.Instance` to stub the `TestAggregatedDistanceCalculatorFactory` with it. A similar structure can be implemented for the calculation module if its behaviour needs to be stubbed away for testing purposes. However, this does increase the complexity of the software solution. Also, due to the mocking (shunting), the black box unit test is shifting towards a greybox test as it can be tested which functions are called with specific arguments. Finally, there is also an additional overhead, because 3 additional files are needed for each calculator factory implementation:

* A factory to create the calculator 
* A test factory to create a test calculator 
* A separate class which substitutes the factory instance with a test factory (and restores it after testing)

The advantages of the chosen approach are: 

* Familiarity reasons, as the same structure was used in an earlier project with the same issues.
* Dependency injection would create a dependency between the application domain and the simulation domain, a situation that should be prevented as the wrapper is the one responsible for communicating between these two layers
* DI frameworks could be used, but due to limited experience (and the fact that probably a constructor or setter injection needs to be implemented) this solution is discarded.

## Decision outcome

Based on the foregoing discussion, the following design decisions were made:

* The `CalculationModule` will be implemented in the application domain and will calculate for multiple failure velocities. No similar functionality is implementated in the simulation domain, as it is unclear whether this functionality is going to be needed at all.
* The (calculator)wrapper will use a factory with a singleton pattern with its instance property or field exposed to set a mock implementation that returns stubbed instances of the calculator.

## Resources

* [Self Shunt paper](http://community.wvu.edu/~hhammar/rts/adv%20rts/design%20patterns%20case%20studies/SelfShunPtrn%20patterns%20examples%20unit%20testing.pdf)
* *"Agile Principles, Patterns, and Practices in C#"* by Robert C. Martin and Micah Martin
* [Self Shunt drawbacks](https://8thlight.com/blog/paul-pagel/2006/09/11/self-shunt.html)
