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

namespace Application.BalancedFieldLength.Data.Test
{
    [TestFixture]
    public class AircraftDataTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var aircraftData = new AircraftData();

            // Assert
            Assert.That(aircraftData.TakeOffWeight, Is.NaN);
            Assert.That(aircraftData.PitchGradient.Degrees, Is.NaN);
            Assert.That(aircraftData.MaximumPitchAngle.Degrees, Is.NaN);

            Assert.That(aircraftData.WingSurfaceArea, Is.NaN);
            Assert.That(aircraftData.AspectRatio, Is.NaN);
            Assert.That(aircraftData.OswaldFactor, Is.NaN);

            Assert.That(aircraftData.MaximumLiftCoefficient, Is.NaN);
            Assert.That(aircraftData.LiftCoefficientGradient, Is.NaN);
            Assert.That(aircraftData.ZeroLiftAngleOfAttack.Degrees, Is.NaN);

            Assert.That(aircraftData.RestDragCoefficientWithEngineFailure, Is.NaN);
            Assert.That(aircraftData.RestDragCoefficient, Is.NaN);

            Assert.That(aircraftData.RollResistanceCoefficient, Is.NaN);
            Assert.That(aircraftData.RollResistanceWithBrakesCoefficient, Is.NaN);
        }
    }
}