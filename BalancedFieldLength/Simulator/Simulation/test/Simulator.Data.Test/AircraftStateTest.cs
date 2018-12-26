using NUnit.Framework;

namespace Simulator.Data.Test
{
    [TestFixture]
    public class AircraftStateTest
    {
        [Test]
        public void DefaultConstructor_ExpectedValues()
        {
            // Call
            var state = new AircraftState();

            // Assert
            Assert.Zero(state.FlightPathAngle.Radians);
            Assert.Zero(state.PitchAngle.Radians);
            Assert.Zero(state.TrueAirspeed);
            Assert.Zero(state.Height);
            Assert.Zero(state.Distance);
        }
    }
}