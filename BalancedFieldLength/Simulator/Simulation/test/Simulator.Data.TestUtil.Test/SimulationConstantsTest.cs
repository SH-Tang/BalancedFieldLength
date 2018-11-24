using NUnit.Framework;

namespace Simulator.Data.TestUtil.Test
{
    [TestFixture]
    public class SimulationConstantsTest
    {
        [Test]
        public static void SimulationConstants_Always_ReturnsExpectedValues()
        {
            // Assert
            Assert.AreEqual(10e-6, SimulationConstants.Tolerance);
            Assert.AreEqual(9.81, SimulationConstants.GravitationalAcceleration);
            Assert.AreEqual(1.225, SimulationConstants.Density);
        }
    }
}