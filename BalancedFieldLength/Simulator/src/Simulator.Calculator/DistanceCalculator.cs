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
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.Exceptions;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class for calculating the traversed distance until the simulation reached the stopping criteria.
    /// </summary>
    public class DistanceCalculator : IDistanceCalculator
    {
        private const double screenHeight = 10.7;
        private readonly INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator;
        private readonly IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator;
        private readonly IIntegrator integrator;
        private readonly CalculationSettings calculationSettings;

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculator"/>.
        /// </summary>
        /// <param name="normalTakeOffDynamicsCalculator">The <see cref="INormalTakeOffDynamicsCalculator"/>
        /// to calculate the aircraft dynamics without failure.</param>
        /// <param name="failureTakeOffDynamicsCalculator">The <see cref="IFailureTakeOffDynamicsCalculator"/>
        /// to calculate the aircraft dynamics after failure.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to integrate the first order
        /// dynamic system.</param>
        /// <param name="calculationSettings">The <see cref="CalculationSettings"/>
        /// to configure the calculator.</param>
        /// <exception cref="ArgumentNullException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="normalTakeOffDynamicsCalculator"/></item>
        /// <item><paramref name="failureTakeOffDynamicsCalculator"/></item>
        /// <item><paramref name="integrator"/></item>
        /// <item><paramref name="calculationSettings"/></item>
        /// </list>
        /// is <c>null</c>.</exception>
        public DistanceCalculator(INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator,
                                  IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator,
                                  IIntegrator integrator,
                                  CalculationSettings calculationSettings)
        {
            if (normalTakeOffDynamicsCalculator == null)
            {
                throw new ArgumentNullException(nameof(normalTakeOffDynamicsCalculator));
            }

            if (failureTakeOffDynamicsCalculator == null)
            {
                throw new ArgumentNullException(nameof(failureTakeOffDynamicsCalculator));
            }

            if (integrator == null)
            {
                throw new ArgumentNullException(nameof(integrator));
            }

            if (calculationSettings == null)
            {
                throw new ArgumentNullException(nameof(calculationSettings));
            }

            this.normalTakeOffDynamicsCalculator = normalTakeOffDynamicsCalculator;
            this.failureTakeOffDynamicsCalculator = failureTakeOffDynamicsCalculator;
            this.integrator = integrator;
            this.calculationSettings = calculationSettings;
        }

        public DistanceCalculatorOutput Calculate()
        {
            var state = new AircraftState();
            bool hasFailureOccurred = false;
            int failureSpeed = calculationSettings.FailureSpeed;

            for (int i = 0; i < calculationSettings.MaximumNrOfTimeSteps; i++)
            {
                AircraftAccelerations accelerations = hasFailureOccurred
                                                          ? failureTakeOffDynamicsCalculator.Calculate(state)
                                                          : normalTakeOffDynamicsCalculator.Calculate(state);

                state = integrator.Integrate(state, accelerations, calculationSettings.TimeStep);

                if (state.TrueAirspeed > failureSpeed)
                {
                    hasFailureOccurred = true;
                }

                if (state.Height >= screenHeight || state.TrueAirspeed <= 0)
                {
                    if (!hasFailureOccurred)
                    {
                        throw new CalculatorException("Calculation converged before failure occurred.");
                    }

                    return new DistanceCalculatorOutput(failureSpeed, state.Distance);
                }
            }

            throw new CalculatorException("Calculation did not converge.");
        }
    }
}