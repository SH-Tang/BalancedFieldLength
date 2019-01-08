using System;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Calculator.Factories
{
    /// <summary>
    /// Interface for describing a factory creating calculator instances for take off dynamics.
    /// </summary>
    public interface ITakeOffDynamicsCalculatorFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="INormalTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="data">The <see cref="AircraftData"/> to create the calculator for.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <returns>An instance of <see cref="INormalTakeOffDynamicsCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/>
        /// is <c>null</c>.</exception>
        INormalTakeOffDynamicsCalculator CreateNormalTakeOffDynamics(AircraftData data,
                                                                     double density,
                                                                     double gravitationalAcceleration);

        /// <summary>
        /// Creates an instance of <see cref="IFailureTakeOffDynamicsCalculator"/> representing
        /// the aborted take off.
        /// </summary>
        /// <param name="data">The <see cref="AircraftData"/> to create the calculator for.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <returns>An instance of <see cref="INormalTakeOffDynamicsCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/>
        /// is <c>null</c>.</exception>
        IFailureTakeOffDynamicsCalculator CreateAbortedTakeOffDynamics(AircraftData data,
                                                                       double density,
                                                                       double gravitationalAcceleration);

        /// <summary>
        /// Creates an instance of <see cref="IFailureTakeOffDynamicsCalculator"/> representing
        /// the continued take off.
        /// </summary>
        /// <param name="data">The <see cref="AircraftData"/> to create the calculator for.</param>
        /// <param name="nrOfFailedEngines">The number of failed engines.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <returns>An instance of <see cref="INormalTakeOffDynamicsCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/>
        /// is <c>null</c>.</exception>
        IFailureTakeOffDynamicsCalculator CreateContinuedTakeOffDynamicsCalculator(AircraftData data,
                                                                                   int nrOfFailedEngines,
                                                                                   double density,
                                                                                   double gravitationalAcceleration);
    }
}