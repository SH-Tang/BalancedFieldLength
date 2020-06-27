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
    public class BalancedFieldLengthCalculationTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var calculation = new BalancedFieldLengthCalculation();

            // Assert
            Assert.That(calculation.AircraftData, Is.Not.Null);
            Assert.That(calculation.EngineData, Is.Not.Null);
            Assert.That(calculation.SimulationSettings, Is.Not.Null);
            Assert.That(calculation.Output, Is.Null);
        }
    }
}