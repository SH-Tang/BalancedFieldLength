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