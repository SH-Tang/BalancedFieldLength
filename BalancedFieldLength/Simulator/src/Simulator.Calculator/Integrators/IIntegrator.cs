// Copyright (C) 2018 Dennis Tang. All rights reserved.
//
// This file is part of Balanced Field Length.
//
// Balanced Field Length is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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