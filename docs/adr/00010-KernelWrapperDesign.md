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

* Extend the simplementation with functionality to calculate multiple distances 
* Create a class to wrap around the current implementation and execute the calculations for the multiple distances. 

Both options are possible, as the whole code base is available and under our control. However, the first solution would shift the responsibility to the `Simulator.Kernel` implementation which could reduce the flexibility. If this implementation were to be chosen, the consumers will not be able to control the individual calculations anymore. The major drawback is that the consumers cannot generate log messages or other information for each individual calculation. In short, `Simulator.Kernel` would dictate the functionality on HOW the calculation should be performed. However, the consumer of the component is still able to generate their own calculation, as the implementation for individual failure velocities is still defined in the public API.

The second implementation would shift the calculation procedure to the consumer side, allowing more flexibility on how the consumer should handle each individual calculation. The consumer of the component would be able to configure the handling of each individual failure velocity, potentially able to provide more information when needed. This option will mean that the consumer is fully responsible in configuring and using the components of the `Simulator.Kernel` accordingly. 

The current conclusion is to go for the second option. While the first option would extend the functionality of the `Simulator.Kernel` and _potential_ consumers, there is no added benefit. This option would result in duplicate code, as the `Simulator.Kernel` would be able to calculate and in the wrapper if it is necessary to define additional behaviour for each individual failure velocity calculation. Do keep in mind that this code duplication is legimate, due to the fact that the responsibility is different between the wrapper assembly and the `Simulator.Kernel` assembly. Besides code duplication, it is uncertain whether this additional functionality is needed at all for the `Simulator.Kernel`. Therefore, the second option has the least drawback, as:

* No code duplication, therefore less code maintenance
* More flexibility for the component consumers as they can define their own behaviour on what should happen during the calculation of each failure velocities
* No bloating of the public API of the `Simulator.Kernel` assembly as it is uncertain whether the functionality of calculating multiple failure velocities at once is ever going to be needed. (Adhering to the *YAGNI* principle)

