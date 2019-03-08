using System;
using Simulator.Calculator;
using Simulator.Calculator.Integrators;
using Simulator.Data;
using Simulator.Integration.Factories;

namespace Simulator.Integration.AggregatedDistanceCalculator
{
    /// <summary>
    /// Calculator to calculate the aggregated output for a given failure velocity.
    /// </summary>
    public class AggregatedDistanceCalculator : IAggregatedDistanceCalculator
    {
        private readonly IDistanceCalculatorFactory distanceCalculatorFactory;

        /// <summary>
        /// Creates a new instance of <see cref="AggregatedDistanceCalculator"/>.
        /// </summary>
        /// <param name="distanceCalculatorFactory">Factory for creating instances of <see cref="DistanceCalculator"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when any arguments are <c>null</c>.</exception>
        public AggregatedDistanceCalculator(IDistanceCalculatorFactory distanceCalculatorFactory)
        {
            if (distanceCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(distanceCalculatorFactory));
            }

            this.distanceCalculatorFactory = distanceCalculatorFactory;
        }

        public AggregatedDistanceOutput Calculate(AircraftData aircraftData,
                                                  IIntegrator integrator,
                                                  int nrOfFailedEngines,
                                                  double density,
                                                  double gravitationalAcceleration,
                                                  CalculationSettings calculationSettings)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            if (integrator == null)
            {
                throw new ArgumentNullException(nameof(integrator));
            }

            if (calculationSettings == null)
            {
                throw new ArgumentNullException(nameof(calculationSettings));
            }

            IDistanceCalculator abortedTakeOffCalculator = distanceCalculatorFactory.CreateAbortedTakeOffDistanceCalculator(aircraftData,
                                                                                                                            integrator,
                                                                                                                            density,
                                                                                                                            gravitationalAcceleration,
                                                                                                                            calculationSettings);

            IDistanceCalculator continuedTakeOffCalculator = distanceCalculatorFactory.CreateContinuedTakeOffDistanceCalculator(aircraftData,
                                                                                                                                integrator,
                                                                                                                                nrOfFailedEngines,
                                                                                                                                density,
                                                                                                                                gravitationalAcceleration,
                                                                                                                                calculationSettings);

            DistanceCalculatorOutput abortedTakeOffOutput = abortedTakeOffCalculator.Calculate();
            DistanceCalculatorOutput continuedTakeOffOutput = continuedTakeOffCalculator.Calculate();

            return new AggregatedDistanceOutput(calculationSettings.FailureSpeed,
                                                abortedTakeOffOutput.Distance,
                                                continuedTakeOffOutput.Distance);
        }
    }
}