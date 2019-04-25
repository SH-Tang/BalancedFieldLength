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
        /// <param name="data">The <see cref="AircraftData"/> to create a <see cref="DistanceCalculator"/> for.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to solve the dynamic system.</param>
        /// <param name="nrOfFailedEngines">The number of failed engines.</param>
        /// <param name="density">The air density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="calculationSettings">The <see cref="CalculationSettings"/> to configure the calculator.</param>
        /// <returns>A configured <see cref="DistanceCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/>,
        /// <paramref name="integrator"/> or <paramref name="calculationSettings"/> is <c>null</c>.</exception>
        IDistanceCalculator CreateContinuedTakeOffDistanceCalculator(AircraftData data,
                                                                     IIntegrator integrator,
                                                                     int nrOfFailedEngines,
                                                                     double density,
                                                                     double gravitationalAcceleration,
                                                                     CalculationSettings calculationSettings);

        /// <summary>
        /// Creates a configured instance of <see cref="DistanceCalculator"/> that calculates the
        /// distance after aborting the take-off after failure.
        /// </summary>
        /// <param name="data">The <see cref="AircraftData"/> to create a <see cref="DistanceCalculator"/> for.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to solve the dynamic system.</param>
        /// <param name="density">The air density. [kg/m^3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration. [m/s^2]</param>
        /// <param name="calculationSettings">The <see cref="CalculationSettings"/> to configure the calculator.</param>
        /// <returns>A configured <see cref="DistanceCalculator"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/>,
        /// <paramref name="integrator"/> or <paramref name="calculationSettings"/> is <c>null</c>.</exception>
        IDistanceCalculator CreateAbortedTakeOffDistanceCalculator(AircraftData data,
                                                                   IIntegrator integrator,
                                                                   double density,
                                                                   double gravitationalAcceleration,
                                                                   CalculationSettings calculationSettings);
    }
}