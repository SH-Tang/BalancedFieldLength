using System;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator.Factories
{
    /// <summary>
    /// Interface describing a factory for creating configured instances of <see cref="DistanceCalculator"/>.
    /// </summary>
    public interface IDistanceCalculatorFactory
    {
        /// <summary>
        /// Creates a configured instance of <see cref="DistanceCalculator"/> that calculates the
        /// distance after continuing the take-off after failure.
        /// </summary>
        /// <param name="takeOffDynamicsCalculatorFactory">The <see cref="ITakeOffDynamicsCalculatorFactory"/>
        /// to create the aircraft dynamics from.</param>
        /// <param name="data">The <see cref="AircraftData"/> to create a <see cref="DistanceCalculator"/>
        /// for.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to solve the dynamic system.</param>
        /// <param name="nrOfFailedEngines">The number of failed engines.</param>
        /// <param name="density">The air density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="calculatorSettings">The <see cref="DistanceCalculatorSettings"/> to
        /// configure the calculator.</param>
        /// <returns>A configured <see cref="DistanceCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="takeOffDynamicsCalculatorFactory"/>,
        /// <paramref name="data"/> or <paramref name="integrator"/> is <c>null</c>.</exception>
        DistanceCalculator CreateContinuedTakeOffDistanceCalculator(
            ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory,
            AircraftData data,
            IIntegrator integrator,
            int nrOfFailedEngines,
            double density,
            double gravitationalAcceleration, DistanceCalculatorSettings calculatorSettings);

        /// <summary>
        /// Creates a configured instance of <see cref="DistanceCalculator"/> that calculates the
        /// distance after aborting the take-off after failure.
        /// </summary>
        /// <param name="takeOffDynamicsCalculatorFactory">The <see cref="ITakeOffDynamicsCalculatorFactory"/>
        /// to create the aircraft dynamics from.</param>
        /// <param name="data">The <see cref="AircraftData"/> to create a <see cref="DistanceCalculator"/>
        /// for.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to solve the dynamic system.</param>
        /// <param name="density">The air density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="calculatorSettings">The <see cref="DistanceCalculatorSettings"/> to
        /// configure the calculator.</param>
        /// <returns>A configured <see cref="DistanceCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="takeOffDynamicsCalculatorFactory"/>,
        /// <paramref name="data"/> or <paramref name="integrator"/> is <c>null</c>.</exception>
        DistanceCalculator CreateAbortedTakeOffDistanceCalculator(
            ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory,
            AircraftData data,
            IIntegrator integrator,
            double density,
            double gravitationalAcceleration,
            DistanceCalculatorSettings calculatorSettings);
    }
}