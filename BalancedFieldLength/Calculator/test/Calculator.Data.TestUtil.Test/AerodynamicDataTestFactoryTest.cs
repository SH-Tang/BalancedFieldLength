using NUnit.Framework;

namespace Calculator.Data.TestUtil.Test
{
    [TestFixture]
    public class AerodynamicDataTestFactoryTest
    {
        [Test]
        public static void CreateAerodynamicData_Always_ReturnsAerodynamicData()
        {
            // Call 
            AerodynamicData aerodynamicData = AerodynamicDataTestFactory.CreateAerodynamicData();

            // Assert
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.AspectRatio));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.WingArea));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.ZeroLiftAngleOfAttack));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.LiftCoefficientGradient));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.MaximumLiftCoefficient));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.RestDragCoefficientWithoutEngineFailure));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.RestDragCoefficientWithEngineFailure));
            Assert.IsTrue(IsConcreteNumber(aerodynamicData.OswaldFactor));
        }

        private static bool IsConcreteNumber(double number)
        {
            return !double.IsInfinity(number) && !double.IsNaN(number);
        }
    }
}