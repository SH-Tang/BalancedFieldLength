# Balanced Field Length

# License
This project was released under the GPLv3 license. A copy of this license can be found [here](/BalancedFieldLength/licenses).

# Background
The major concern in the society when it comes to aviation is safety, which is the main reason of setting up flight regulations. The so-called “balanced field length” is one of those regulations in operating airplanes. This application provides an interface to calculate the balanced field length, which is the length of the runway required for a jet powered airplane to operate safely with a failure of engines expected at any point during the take-off phase.

When the balanced field length and the corresponding speed V1 (also known as the decision speed) are known, the pilot can make a decision to abort the flight or continue take-off according to the values found. When the failure speed is reached before decision speed, it is recommended to abort the take-off and if the failure speed occurs after the decision speed, it is recommended to continue to take off. 
![Alt text](docs/documentation/Example.png?raw=true "Example of a balanced field length calculation for a 2 jet engine powered airplane. The balanced field length of 2343m occurs at V1 = 72.03m/s.")

# Goals
The main goal of this application is: 

> To demonstrate that it is possible to write a robust, numerical simulation in C#, while adhering to software engineering standards. 

Additionally, the following requirements were set: 

* The simulator / simulation related libraries should be flexible enough to allow various configurations. 
* The simulation related libraries should be decoupled from the application and/or visualisation logic. In short, it should be possible to use the business rules (simulation module) in other applications without considerable effort.
* Apply software engineering principles to ensure a qualitatively, robust application.
* The application will be written according to the principles of 'Test Driven Development' to verify its behaviour. 

# Assumptions
The current implementation of the flight dynamics are based on a few assumptions. These are:
* Weight remains constant.
* Reverse thrust is not applied, instead, the thrust is set to 0 immediately after failure when an aborted take-off is opted. (*Note:* Application of reverse thrust is however possible during these conditions)
* In case of continued takeoff, the maximum thrust by the remaining engines are used for the calculation.
* The screenheight is fixed at 10.7m.
* SI units are used for the simulation. 

# Code example
The following illustrates how the simulation library can be used for calculating the balanced field length:

    // Calculation settings 
    const double gravitationalAcceleration = 9.81;
    const double density = 1.225;

    const int maximumTimeSteps = 10000;
    const double timeStep = 0.1;
     
    // Set the aerodynamic properties of the aircraft
    var aerodynamicsData = new AerodynamicsData(15, 100, Angle.FromDegrees(-3), 4.85, 1.60, 0.021, 0.026, 0.85);
    
    // Set the other aircraft data
    var aircraftData = new AircraftData(2, 75, 500, Angle.FromDegrees(6), Angle.FromDegrees(16), 0.02, 0.2, aerodynamicsData);

    // Set the integration scheme that should be used for the calculations.
    var integrator = new EulerIntegrator();

    // Make the calculations. 
    var calculationKernel = new AggregatedDistanceCalculatorKernel();

    // Call the calculation. A range of velocities is used as an input
    var results = new List<AggregatedDistanceOutput>();
    for (var i = 0; i < 90; i++)
    {
        var calculationSettings = new CalculationSettings(i, maximumTimeSteps, timeStep);
        AggregatedDistanceOutput result = calculationKernel.Calculate(aircraftData,
                                                                      integrator,
                                                                      1,
                                                                      density,
                                                                      gravitationalAcceleration,
                                                                      calculationSettings);
        results.Add(result);
    }

    // Calculate the Balanced Field Length by using the obtained results
    BalancedFieldLength balancedFieldLength = BalancedFieldLengthCalculator.CalculateBalancedFieldLength(results);