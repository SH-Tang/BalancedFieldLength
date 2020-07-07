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
using Simulator.Calculator;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Components.Factories
{
    /// <summary>
    /// Factory to create configured instance of <see cref="DistanceCalculator"/>.
    /// </summary>
    public class DistanceCalculatorFactory : IDistanceCalculatorFactory
    {
        private readonly ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory;

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculator"/>.
        /// </summary>
        /// <param name="takeOffDynamicsCalculatorFactory">The factory to create instances of take off dynamics calculators.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="takeOffDynamicsCalculatorFactory"/> is <c>null</c>.</exception>
        public DistanceCalculatorFactory(ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory)
        {
            if (takeOffDynamicsCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(takeOffDynamicsCalculatorFactory));
            }

            this.takeOffDynamicsCalculatorFactory = takeOffDynamicsCalculatorFactory;
        }

        public IDistanceCalculator CreateContinuedTakeOffDistanceCalculator(AircraftData data,
                                                                            IIntegrator integrator,
                                                                            int nrOfFailedEngines,
                                                                            double density,
                                                                            double gravitationalAcceleration,
                                                                            CalculationSettings calculationSettings)
        {
            INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data, density, gravitationalAcceleration);
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateContinuedTakeOffDynamicsCalculator(data, nrOfFailedEngines, density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, calculationSettings);
        }

        public IDistanceCalculator CreateAbortedTakeOffDistanceCalculator(AircraftData data,
                                                                          IIntegrator integrator,
                                                                          double density,
                                                                          double gravitationalAcceleration,
                                                                          CalculationSettings calculationSettings)
        {
            INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data, density, gravitationalAcceleration);
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateAbortedTakeOffDynamics(data, density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, calculationSettings);
        }
    }
}