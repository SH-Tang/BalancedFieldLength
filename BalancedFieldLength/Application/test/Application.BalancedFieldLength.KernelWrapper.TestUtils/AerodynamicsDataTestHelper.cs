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

using NUnit.Framework;
using Simulator.Data;
using AircraftData = Application.BalancedFieldLength.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils
{
    /// <summary>
    /// Helper class which contains method which can be used for testing <see cref="AerodynamicsData"/>.
    /// </summary>
    public static class AerodynamicsDataTestHelper
    {
        /// <summary>
        /// Asserts whether the <paramref name="aerodynamicsData"/> contain the correct information based on
        /// <paramref name="aircraftData"/>.
        /// </summary>
        /// <param name="aircraftData">The <see cref="Data.AircraftData"/> to use as a reference.</param>
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/> to assert.</param>
        public static void AssertAerodynamicsData(AircraftData aircraftData, AerodynamicsData aerodynamicsData)
        {
            Assert.That(aerodynamicsData.AspectRatio, Is.EqualTo(aircraftData.AspectRatio));
            Assert.That(aerodynamicsData.WingArea, Is.EqualTo(aircraftData.WingSurfaceArea));

            Assert.That(aerodynamicsData.LiftCoefficientGradient, Is.EqualTo(aircraftData.LiftCoefficientGradient));
            Assert.That(aerodynamicsData.MaximumLiftCoefficient, Is.EqualTo(aircraftData.MaximumLiftCoefficient));
            Assert.That(aerodynamicsData.ZeroLiftAngleOfAttack, Is.EqualTo(aircraftData.ZeroLiftAngleOfAttack));
            Assert.That(aerodynamicsData.OswaldFactor, Is.EqualTo(aircraftData.OswaldFactor));

            Assert.That(aerodynamicsData.RestDragCoefficientWithEngineFailure, Is.EqualTo(aircraftData.RestDragCoefficientWithEngineFailure));
            Assert.That(aerodynamicsData.RestDragCoefficientWithoutEngineFailure, Is.EqualTo(aircraftData.RestDragCoefficient));
        }
    }
}