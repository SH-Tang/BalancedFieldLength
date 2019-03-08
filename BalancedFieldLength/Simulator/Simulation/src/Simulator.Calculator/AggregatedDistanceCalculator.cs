using System;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Data;
using Simulator.Data.Exceptions;

namespace Simulator.Calculator
{
    /// <summary>
    /// Calculator to calculate the aggregated output for a given failure velocity.
    /// </summary>
    public class AggregatedDistanceCalculator
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

        /// <summary>
        /// Calculates the required distance for a continued and a rejected take off for a given failure speed.
        /// </summary>
        /// <param name="aircraftData">The <see cref="AircraftData"/> to create a <see cref="DistanceCalculator"/> for.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to solve the dynamic system.</param>
        /// <param name="nrOfFailedEngines">The number of failed engines.</param>
        /// <param name="density">The air density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="calculationSettings">The <see cref="CalculationSettings"/> to configure the calculator.</param>
        /// <returns>A <see cref="AggregatedDistanceOutput"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>, <paramref name="integrator"/>
        /// or <see cref="CalculationSettings"/> is <c>null</c>.</exception>
        /// <exception cref="CalculatorException">Thrown when the <see cref="AggregatedDistanceOutput"/>
        /// could not be calculated.</exception>
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