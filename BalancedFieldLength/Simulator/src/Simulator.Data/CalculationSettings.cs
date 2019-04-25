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
using Core.Common.Utils;

namespace Simulator.Data
{
    /// <summary>
    /// Class that holds all the numeric information to configure a calculation.
    /// </summary>
    public class CalculationSettings
    {
        /// <summary>
        /// Creates a new instance of <see cref="CalculationSettings"/>.
        /// </summary>
        /// <param name="failureSpeed">The speed at which a failure occurs. [m/s]</param>
        /// <param name="maximumNrOfTimeSteps">The maximum number of time steps before the
        /// calculator times out.</param>
        /// <param name="timeStep">The amount of seconds each time step represents. [s]</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="timeStep"/>
        /// is <see cref="double.NaN"/> or <see cref="double.PositiveInfinity"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureSpeed"/> &lt; 0</item>item>
        /// <item><paramref name="maximumNrOfTimeSteps"/> &lt;= 0</item>
        /// <item><paramref name="timeStep"/> &lt;= 0</item>
        /// </list></exception>
        public CalculationSettings(int failureSpeed, int maximumNrOfTimeSteps, double timeStep)
        {
            ValidateInput(failureSpeed, maximumNrOfTimeSteps, timeStep);

            FailureSpeed = failureSpeed;
            MaximumNrOfTimeSteps = maximumNrOfTimeSteps;
            TimeStep = timeStep;
        }

        /// <summary>
        /// Gets the failure speed. [m/s]
        /// </summary>
        public int FailureSpeed { get; }

        /// <summary>
        /// Gets the maximum number of time steps before the calculator times out.
        /// </summary>
        public int MaximumNrOfTimeSteps { get; }

        /// <summary>
        /// Gets the amount of seconds a time step represents. [s]
        /// </summary>
        public double TimeStep { get; }

        /// <summary>
        /// Validates the numeric input.
        /// </summary>
        /// <param name="failureSpeed">The speed at which a failure occurs. [m/s]</param>
        /// <param name="maximumNrOfTimeSteps">The maximum number of time steps before the
        /// calculator times out.</param>
        /// <param name="timeStep">The amount of seconds each time step represents. [s]</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="timeStep"/>
        /// is <see cref="double.NaN"/> or <see cref="double.PositiveInfinity"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when:
        /// <list type="bullet">
        /// <paramref name="failureSpeed"/> &lt; 0
        /// <paramref name="maximumNrOfTimeSteps"/> &lt;= 0
        /// <paramref name="timeStep"/> &lt;= 0
        /// </list></exception>
        private static void ValidateInput(int failureSpeed, int maximumNrOfTimeSteps, double timeStep)
        {
            failureSpeed.ArgumentIsLargerOrEqualToZero(nameof(failureSpeed));
            maximumNrOfTimeSteps.ArgumentIsLargerThanZero(nameof(maximumNrOfTimeSteps));

            timeStep.ArgumentIsLargerThanZero(nameof(timeStep));
            timeStep.ArgumentIsConcreteNumber(nameof(timeStep));
        }
    }
}