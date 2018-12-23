using System;
using Simulator.Data;

namespace Simulator.Calculator.Integrators
{
    /// <summary>
    /// Interface describing the integration of the following first order dynamic system:
    /// <code>
    /// Height(n+1) = Height(n) + dHeight * dt 
    /// Distance(n+1) = Distance(n) + Velocity * dt
    /// Velocity(n+1) = Velocity(n) + dVelocity * dt
    /// Flight Path Angle(n+1) = Flight Path Angle(n) + dFlight Path Angle * dt
    /// Pitch Angle(n+1) = Pitch Angle(n) + dPitch Angle* dt
    /// </code>
    /// </summary>
    public interface IIntegrator
    {
        /// <summary>
        /// Integrates the dynamic system based on its input arguments.
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/> to integrate from.</param>
        /// <param name="accelerations">The <see cref="AircraftAccelerations"/> to integrate with.</param>
        /// <param name="timeStep">The stepping time to integrate.</param>
        /// <returns>The integrated <see cref="AircraftState"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="state"/>
        /// or <paramref name="accelerations"/> is <c>null</c>.</exception>
        AircraftState Integrate(AircraftState state, AircraftAccelerations accelerations, double timeStep);
    }
}