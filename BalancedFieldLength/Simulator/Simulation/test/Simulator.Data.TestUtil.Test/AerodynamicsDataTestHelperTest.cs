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
using System.Collections.Generic;
using Core.Common.Data;
using NUnit.Framework;

namespace Simulator.Data.TestUtil.Test
{
    [TestFixture]
    public class AerodynamicsDataTestHelperTest
    {
        [Test]
        public void GetValidAngleOfAttack_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => AerodynamicsDataTestHelper.GetValidAngleOfAttack(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("data", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetAerodynamicsConfigurations))]
        public void GetValidAngleOfAttack_VariousAerodynamicsData_ReturnsExpectedValue(AerodynamicsData data,
                                                                                       Angle expectedAngle)
        {
            // Call
            Angle angleOfAttack = AerodynamicsDataTestHelper.GetValidAngleOfAttack(data);

            // Assert
            Assert.AreEqual(expectedAngle, angleOfAttack);
        }

        private static IEnumerable<TestCaseData> GetAerodynamicsConfigurations()
        {
            var random = new Random(21);

            yield return new TestCaseData(
                new AerodynamicsData(random.NextDouble(), random.NextDouble(),
                                     new Angle(), 1, 1,
                                     random.NextDouble(), random.NextDouble(), random.NextDouble()),
                Angle.FromRadians(0.5)).SetName("Positive gradient, no offset");
            yield return new TestCaseData(
                new AerodynamicsData(random.NextDouble(), random.NextDouble(),
                                     Angle.FromRadians(-10), 1, 1,
                                     random.NextDouble(), random.NextDouble(), random.NextDouble()),
                Angle.FromRadians(-9.5)).SetName("Positive gradient, negative offset");
            yield return new TestCaseData(
                new AerodynamicsData(random.NextDouble(), random.NextDouble(),
                                     Angle.FromRadians(10), 1, 1,
                                     random.NextDouble(), random.NextDouble(), random.NextDouble()),
                Angle.FromRadians(10.5)).SetName("Positive gradient, positive offset");
        }
    }
}