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
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Data.Test
{
    [TestFixture]
    public class AngleExtensionsTest
    {
        [Test]
        public void IsConcreteAngle_AngleWithConcreteNumbers_ReturnsTrue()
        {
            // Setup
            var random = new Random(21);
            Angle angle = random.NextAngle();

            // Call 
            bool result = angle.IsConcreteAngle();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void IsConcreteAngle_AngleWithNonConcreteNumbers_ReturnsFalse(double nonConcreteNumbers)
        {
            // Setup
            Angle angle = Angle.FromDegrees(nonConcreteNumbers);

            // Call 
            bool result = angle.IsConcreteAngle();

            // Assert
            Assert.That(result, Is.False);
        }
    }
}