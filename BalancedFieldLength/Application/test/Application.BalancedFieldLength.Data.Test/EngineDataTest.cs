using NUnit.Framework;

namespace Application.BalancedFieldLength.Data.Test
{
    [TestFixture]
    public class EngineDataTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var engineData = new EngineData();

            // Assert
            Assert.That(engineData.NrOfEngines, Is.Zero);
            Assert.That(engineData.NrOfFailedEngines, Is.Zero);
            Assert.That(engineData.ThrustPerEngine, Is.NaN);
        }
    }
}