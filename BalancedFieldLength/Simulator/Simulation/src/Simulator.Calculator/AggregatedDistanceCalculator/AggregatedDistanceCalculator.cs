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
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator.AggregatedDistanceCalculator
{
    /// <summary>
    /// Calculator to calculate the aggregated output for a given failure velocity.
    /// </summary>
    public class AggregatedDistanceCalculator : IAggregatedDistanceCalculator
    {
        private readonly IDistanceCalculatorFactory distanceCalculatorFactory;

        /// <summary>
        /// Creates a new instance of <see cref="AggregatedDistanceCalculator"/>.
        /// </summary>
        /// <param name="distanceCalculatorFactory">Factory for creating instances of <see cref="DistanceCalculator"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when any arguments are <c>null</c>.</exception>
        public AggregatedDistanceCalculator(IDistanceCalculatorFactory distanceCalculatorFactory)
        {
            if (distanceCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(distanceCalculatorFactory));
            }

            this.distanceCalculatorFactory = distanceCalculatorFactory;
        }

        public AggregatedDistanceOutput Calculate(AircraftData aircraftData,
                                                  IIntegrator integrator,
                                                  int nrOfFailedEngines,
                                                  double density,
                                                  double gravitationalAcceleration,
                                                  CalculationSettings calculationSettings)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            if (integrator == null)
            {
                throw new ArgumentNullException(nameof(integrator));
            }

            if (calculationSettings == null)
            {
                throw new ArgumentNullException(nameof(calculationSettings));
            }

            IDistanceCalculator abortedTakeOffCalculator = distanceCalculatorFactory.CreateAbortedTakeOffDistanceCalculator(aircraftData,
                                                                                                                            integrator,
                                                                                                                            density,
                                                                                                                            gravitationalAcceleration,
                                                                                                                            calculationSettings);

            IDistanceCalculator continuedTakeOffCalculator = distanceCalculatorFactory.CreateContinuedTakeOffDistanceCalculator(aircraftData,
                                                                                                                                integrator,
                                                                                                                                nrOfFailedEngines,
                                                                                                                                density,
                                                                                                                                gravitationalAcceleration,
                                                                                                                                calculationSettings);

            DistanceCalculatorOutput abortedTakeOffOutput = abortedTakeOffCalculator.Calculate();
            DistanceCalculatorOutput continuedTakeOffOutput = continuedTakeOffCalculator.Calculate();

            return new AggregatedDistanceOutput(calculationSettings.FailureSpeed,
                                                abortedTakeOffOutput.Distance,
                                                continuedTakeOffOutput.Distance);
        }
    }
}