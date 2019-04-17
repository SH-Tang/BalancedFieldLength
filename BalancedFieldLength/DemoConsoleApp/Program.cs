using System;
using System.Collections.Generic;
using System.IO;
using Core.Common.Data;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.BalancedFieldLengthCalculator;
using Simulator.Components.Integrators;
using Simulator.Data;
using Simulator.Kernel;

namespace DemoConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Calculation settings 
            const double gravitationalAcceleration = 9.81;
            const double density = 1.225;

            const int maximumTimeSteps = 10000;
            const double timeStep = 0.1;

            var aerodynamicsData = new AerodynamicsData(15, 100, Angle.FromDegrees(-3), 4.85, 1.60, 0.021, 0.026, 0.85);
            var aircraftData = new AircraftData(2, 75, 500, Angle.FromDegrees(6), Angle.FromDegrees(16), 0.02, 0.2, aerodynamicsData);

            var integrator = new EulerIntegrator();

            var calculationKernel = new AggregatedDistanceCalculatorKernel();

            // Call the calculation
            var results = new List<AggregatedDistanceOutput>();
            for (var i = 0; i < 90; i++)
            {
                Console.WriteLine($"Calculating for speed {i}");
                var calculationSettings = new CalculationSettings(i, maximumTimeSteps, timeStep);
                AggregatedDistanceOutput result = calculationKernel.Calculate(aircraftData,
                                                                              integrator,
                                                                              1,
                                                                              density,
                                                                              gravitationalAcceleration,
                                                                              calculationSettings);
                results.Add(result);
            }

            // Calculate the BFL
            AggregatedDistanceOutput balancedFieldLength = AggregatedDistanceCalculatorHelper.DetermineCrossing(results);
            Console.WriteLine($"Balanced Field length is: {balancedFieldLength.FailureSpeed} [m/s] at {balancedFieldLength.ContinuedTakeOffDistance} [m]");

            // Write result
            Console.WriteLine("Write Result");
            using (var writer = new StreamWriter("Results.csv"))
            {
                writer.WriteLine("FailureSpeed ; Continued Take Off Distance ; Aborted Take Off Distance");
                foreach (AggregatedDistanceOutput result in results)
                {
                    writer.WriteLine($"{result.FailureSpeed} ; {result.ContinuedTakeOffDistance} ; {result.AbortedTakeOffDistance}");
                }
            }

            Console.WriteLine("Write Result complete");
            Console.ReadLine();
        }
    }
}