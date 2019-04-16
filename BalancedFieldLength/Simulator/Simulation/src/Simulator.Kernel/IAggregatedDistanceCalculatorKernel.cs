using System;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Kernel
{
    /// <summary>
    /// Interface describing a calculator kernel for calculating aggregated distance outputs.
    /// </summary>
    public interface IAggregatedDistanceCalculatorKernel
    {
        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <param name="aircraftData">The <see cref="Simulator.Data.AircraftData"/> to validate.</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="nrOfFailedEngines">The number of failed engines.</param>
        /// <returns>The <see cref="KernelValidationResult"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        KernelValidationResult Validate(AircraftData aircraftData,
                                        double density,
                                        double gravitationalAcceleration,
                                        int nrOfFailedEngines);

        /// <summary>
        /// Calculates the <see cref="AggregatedDistanceOutput"/> based on the input.
        /// </summary>
        /// <param name="aircraftData">The <see cref="AircraftData"/>.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/>.</param>
        /// <param name="nrOfFailedEngines">The number of failed engines.</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="calculationSettings">The <see cref="CalculationSettings"/> to configure the kernel.</param>
        /// <returns>A <see cref="AggregatedDistanceOutput"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>, <paramref name="integrator"/>
        /// or <paramref name="calculationSettings"/> is <c>null</c>.</exception>
        /// <exception cref="Simulator.Data.Exceptions.CalculatorException">Thrown when the <see cref="AggregatedDistanceOutput"/>
        /// could not be calculated.</exception>
        AggregatedDistanceOutput Calculate(AircraftData aircraftData,
                                           IIntegrator integrator,
                                           int nrOfFailedEngines,
                                           double density,
                                           double gravitationalAcceleration,
                                           CalculationSettings calculationSettings);
    }
}