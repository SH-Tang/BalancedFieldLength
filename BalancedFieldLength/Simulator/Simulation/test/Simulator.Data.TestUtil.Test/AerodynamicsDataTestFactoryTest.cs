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