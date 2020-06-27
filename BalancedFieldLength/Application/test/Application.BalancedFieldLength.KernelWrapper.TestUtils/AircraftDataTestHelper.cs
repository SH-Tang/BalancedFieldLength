using Application.BalancedFieldLength.Data;
using NUnit.Framework;
using KernelAircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils
{
    /// <summary>
    /// Helper class which contains method which can be used for testing <see cref="KernelAircraftData"/>.
    /// </summary>
    public static class AircraftDataTestHelper
    {
        /// <summary>
        /// Asserts whether the <paramref name="kernelAircraftData"/> contain the correct information based on
        /// <paramref name="aircraftData"/> and <paramref name="engineData"/>.
        /// </summary>
        /// <param name="aircraftData">The <see cref="AircraftData"/> to use as a reference.</param>
        /// <param name="engineData">The <see cref="EngineData"/> to use as a reference.</param>
        /// <param name="kernelAircraftData">The <see cref="KernelAircraftData"/> to assert.</param>
        public static void AssertAircraftData(AircraftData aircraftData,
                                              EngineData engineData,
                                              KernelAircraftData kernelAircraftData)
        {
            Assert.That(kernelAircraftData.MaximumPitchAngle, Is.EqualTo(aircraftData.MaximumPitchAngle));
            Assert.That(kernelAircraftData.PitchAngleGradient, Is.EqualTo(aircraftData.PitchGradient));
            Assert.That(kernelAircraftData.RollingResistanceCoefficient, Is.EqualTo(aircraftData.RollResistanceCoefficient));
            Assert.That(kernelAircraftData.BrakingResistanceCoefficient, Is.EqualTo(aircraftData.RollResistanceWithBrakesCoefficient));

            Assert.That(kernelAircraftData.NrOfEngines, Is.EqualTo(engineData.NrOfEngines));
            Assert.That(kernelAircraftData.MaximumThrustPerEngine, Is.EqualTo(engineData.ThrustPerEngine));
            Assert.That(kernelAircraftData.TakeOffWeight, Is.EqualTo(aircraftData.TakeOffWeight));

            AerodynamicsDataTestHelper.AssertAerodynamicsData(aircraftData, kernelAircraftData.AerodynamicsData);
        }
    }
}