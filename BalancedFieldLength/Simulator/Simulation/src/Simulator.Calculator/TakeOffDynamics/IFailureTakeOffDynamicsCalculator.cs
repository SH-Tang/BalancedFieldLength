﻿using System;
using Simulator.Data;

namespace Simulator.Calculator.Dynamics
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
        AircraftAccelerations Calculate(AircraftState state);
    }
}