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
using NUnit.Framework;

namespace Simulator.Data.TestUtil.Test
{
    [TestFixture]
    public class AerodynamicsDataTestFactoryTest
    {
        [Test]
        public static void CreateAerodynamicsData_Always_ReturnsAerodynamicsData()
        {
            // Call 
            AerodynamicsData aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();

            // Assert
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.AspectRatio));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.WingArea));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.ZeroLiftAngleOfAttack.Degrees));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.LiftCoefficientGradient));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.MaximumLiftCoefficient));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.RestDragCoefficientWithoutEngineFailure));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.RestDragCoefficientWithEngineFailure));
            Assert.IsTrue(IsConcreteNonZeroNumber(aerodynamicsData.OswaldFactor));
        }

        private static bool IsConcreteNonZeroNumber(double number)
        {
            return !double.IsInfinity(number)
                   && !double.IsNaN(number)
                   && Math.Abs(number) > double.Epsilon;
        }
    }
}