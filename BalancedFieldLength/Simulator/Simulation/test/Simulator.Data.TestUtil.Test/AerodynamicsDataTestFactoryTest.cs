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
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.AspectRatio));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.WingArea));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.ZeroLiftAngleOfAttack));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.LiftCoefficientGradient));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.MaximumLiftCoefficient));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.RestDragCoefficientWithoutEngineFailure));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.RestDragCoefficientWithEngineFailure));
            Assert.IsTrue(IsConcreteNumber(aerodynamicsData.OswaldFactor));
        }

        private static bool IsConcreteNumber(double number)
        {
            return !double.IsInfinity(number) && !double.IsNaN(number);
        }
    }
}