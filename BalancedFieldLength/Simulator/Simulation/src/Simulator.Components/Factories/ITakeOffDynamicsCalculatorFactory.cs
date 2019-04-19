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
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Components.Factories
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