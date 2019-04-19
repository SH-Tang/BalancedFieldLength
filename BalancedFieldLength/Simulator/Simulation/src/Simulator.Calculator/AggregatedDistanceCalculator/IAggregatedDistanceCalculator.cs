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
using Simulator.Calculator.Integrators;
using Simulator.Data;
using Simulator.Data.Exceptions;

namespace Simulator.Calculator.AggregatedDistanceCalculator
{
    /// <summary>
    /// Interface which describes the calculation of the rejected and continued take off
    /// distances for a given failure speed.
    /// </summary>
    public interface IAggregatedDistanceCalculator
    {
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
        AggregatedDistanceOutput Calculate(AircraftData aircraftData,
                                           IIntegrator integrator,
                                           int nrOfFailedEngines,
                                           double density,
                                           double gravitationalAcceleration,
                                           CalculationSettings calculationSettings);
    }
}