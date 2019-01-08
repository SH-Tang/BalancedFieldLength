using NUnit.Framework;

namespace Simulator.Data.TestUtil.Test
{
    [TestFixture]
    public class CalculationSettingsTestFactoryTest
    {
        [Test]
        public void CreateDistanceCalculatorSettings_Always_ReturnsExpectedValues()
        {
            // Call
            CalculationSettings settings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            // Assert
            Assert.IsNotNull(settings);
            Assert.Greater(settings.FailureSpeed, 0);
            Assert.Greater(settings.MaximumNrOfTimeSteps, 0);
            Assert.IsTrue(IsConcreteNumber(settings.TimeStep));
        }

        private static bool IsConcreteNumber(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }
    }
}