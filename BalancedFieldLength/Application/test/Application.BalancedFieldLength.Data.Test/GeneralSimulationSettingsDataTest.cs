using NUnit.Framework;

namespace Application.BalancedFieldLength.Data.Test
{
    [TestFixture]
    public class GeneralSimulationSettingsDataTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var generalSimulationSettings = new GeneralSimulationSettingsData();

            // Assert
            Assert.That(generalSimulationSettings.Density, Is.EqualTo(1.225));
            Assert.That(generalSimulationSettings.GravitationalAcceleration, Is.EqualTo(9.81));
            Assert.That(generalSimulationSettings.EndFailureVelocity, Is.Zero);

            Assert.That(generalSimulationSettings.MaximumNrOfIterations, Is.Zero);
            Assert.That(generalSimulationSettings.TimeStep, Is.NaN);
        }
    }
}