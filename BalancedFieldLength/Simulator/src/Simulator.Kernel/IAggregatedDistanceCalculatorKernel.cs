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
        /// <returns>The <see cref="KernelValidationError"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        KernelValidationError Validate(AircraftData aircraftData,
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