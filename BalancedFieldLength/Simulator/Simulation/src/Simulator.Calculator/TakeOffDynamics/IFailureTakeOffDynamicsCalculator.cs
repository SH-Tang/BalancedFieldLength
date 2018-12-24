using System;
using Simulator.Data;
using Simulator.Data.Exceptions;

namespace Simulator.Calculator.TakeOffDynamics
{
    /// <summary>
    /// Interface for describing the calculations for the aborted takeoff dynamics.
    /// </summary>
    public interface IFailureTakeOffDynamicsCalculator
    {
        /// <summary>
        /// Calculates the <see cref="AircraftAccelerations"/> based on the <paramref name="state"/>.
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/>.</param>
        /// <returns>The <see cref="AircraftAccelerations"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="CalculatorException">Thrown when the <paramref name="state"/>
        /// results in a state where the calculator cannot continue the calculation.</exception>
        AircraftAccelerations Calculate(AircraftState state);
    }
}