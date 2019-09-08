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
using Simulator.Components.Factories;
using Simulator.Data;

namespace Simulator.Kernel
{
    /// <summary>
    /// Calculator kernel which is configured to calculate the aggregated distance output.
    /// </summary>
    public class AggregatedDistanceCalculatorKernel : IAggregatedDistanceCalculatorKernel
    {
        private readonly IAggregatedDistanceCalculator aggregatedDistanceCalculator;

        /// <summary>
        /// Creates a new instance of <see cref="AggregatedDistanceCalculatorKernel"/>.
        /// </summary>
        public AggregatedDistanceCalculatorKernel()
        {
            var distanceCalculatorFactory = new DistanceCalculatorFactory(new TakeOffDynamicsCalculatorFactory());
            aggregatedDistanceCalculator = new AggregatedDistanceCalculator(distanceCalculatorFactory);
        }

        public KernelValidationError Validate(AircraftData aircraftData,
                                              double density,
                                              double gravitationalAcceleration,
                                              int nrOfFailedEngines)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            var validationError = KernelValidationError.None;

            if (density <= 0)
            {
                validationError |= KernelValidationError.InvalidDensity;
            }

            if (gravitationalAcceleration <= 0)
            {
                validationError |= KernelValidationError.InvalidGravitationalAcceleration;
            }

            if (nrOfFailedEngines >= aircraftData.NrOfEngines)
            {
                validationError |= KernelValidationError.InvalidNrOfFailedEngines;
            }

            return validationError;
        }

        public AggregatedDistanceOutput Calculate(AircraftData aircraftData,
                                                  IIntegrator integrator,
                                                  int nrOfFailedEngines,
                                                  double density,
                                                  double gravitationalAcceleration,
                                                  CalculationSettings calculationSettings)
        {
            return aggregatedDistanceCalculator.Calculate(aircraftData, integrator, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);
        }
    }
}