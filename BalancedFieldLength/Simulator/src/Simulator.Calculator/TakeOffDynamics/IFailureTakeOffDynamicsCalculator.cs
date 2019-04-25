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