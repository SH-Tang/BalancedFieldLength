using System;
using NUnit.Framework;

namespace Simulator.Data.TestUtil.Test
{
    [TestFixture]
    public class AircraftDataTestFactoryTest
    {
        [Test]
        public static void CreateAircraftData_Always_ReturnsAircraftData()
        {
            // Call 
            AircraftData data = AircraftDataTestFactory.CreateRandomAircraftData();

            // Assert
            Assert.IsTrue(IsConcreteNonZeroNumber(data.NrOfEngines));
            Assert.IsTrue(IsConcreteNonZeroNumber(data.MaximumThrustPerEngine));
            Assert.IsTrue(IsConcreteNonZeroNumber(data.TakeOffWeight));
            Assert.IsTrue(IsConcreteNonZeroNumber(data.PitchAngleGradient.Degrees));
            Assert.IsTrue(IsConcreteNonZeroNumber(data.MaximumPitchAngle.Degrees));
            Assert.IsTrue(IsConcreteNonZeroNumber(data.RollingResistanceCoefficient));
            Assert.IsTrue(IsConcreteNonZeroNumber(data.BrakingResistanceCoefficient));

            AerodynamicsData aerodynamicsData = data.AerodynamicsData;
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