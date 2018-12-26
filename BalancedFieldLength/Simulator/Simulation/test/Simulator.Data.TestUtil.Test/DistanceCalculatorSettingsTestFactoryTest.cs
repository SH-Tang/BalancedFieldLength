using NUnit.Framework;

namespace Simulator.Data.TestUtil.Test
{
    [TestFixture]
    public class DistanceCalculatorSettingsTestFactoryTest
    {
        private static bool IsConcreteNumber(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }

        [Test]
        public void CreateDistanceCalculatorSettings_Always_ReturnsExpectedValues()
        {
            // Call
            var settings = DistanceCalculatorSettingsTestFactory.CreateDistanceCalculatorSettings();

            // Assert
            Assert.IsNotNull(settings);
            Assert.Greater(settings.FailureSpeed, 0);
            Assert.Greater(settings.MaximumNrOfTimeSteps, 0);
            Assert.IsTrue(IsConcreteNumber(settings.TimeStep));
        }
    }
}