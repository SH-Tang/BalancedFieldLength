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

using Application.BalancedFieldLength.Data;
using NUnit.Framework;
using Simulator.Data;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils
{
    /// <summary>
    /// Helper class which contains method which can be used for testing <see cref="CalculationSettings"/>.
    /// </summary>
    public class CalculationSettingsTestHelper
    {
        /// <summary>
        /// Asserts whether the <paramref name="calculationSettings"/> contain the correct information based on
        /// <paramref name="simulationSettings"/>.
        /// </summary>
        /// <param name="simulationSettings">The <see cref="GeneralSimulationSettingsData"/> to use as a reference.</param>
        /// <param name="failureSpeed">The failure speed to use as reference.</param>
        /// <param name="calculationSettings">The <see cref="CalculationSettings"/> to assert.</param>
        public static void AssertCalculationSettings(GeneralSimulationSettingsData simulationSettings,
                                                     int failureSpeed,
                                                     CalculationSettings calculationSettings)
        {
            Assert.That(calculationSettings.TimeStep, Is.EqualTo(simulationSettings.TimeStep));
            Assert.That(calculationSettings.MaximumNrOfTimeSteps, Is.EqualTo(simulationSettings.MaximumNrOfIterations));
            Assert.That(calculationSettings.FailureSpeed, Is.EqualTo(failureSpeed));
        }
    }
}