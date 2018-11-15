using System;
using Calculator.Data.TestUtil;
using NUnit.Framework;

namespace Calculator.Data.Test
{
    [TestFixture]
    public class AircraftDataTest
    {
        [Test]
        public static void Constructor_AerodynamicDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => new AircraftData(random.Next(), random.NextDouble(),
                                                       random.NextDouble(), random.NextDouble(), 
                                                       random.NextDouble(), random.NextDouble(), 
                                                       random.NextDouble(), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicData", exception.ParamName);
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            int nrOfEngines = random.Next();
            double maximumThrustPerEngine = random.NextDouble();
            double takeOffWeight = random.NextDouble();
            double pitchAngleGradient = random.NextDouble();
            double maximumPitchAngle = random.NextDouble();
            double rollingResistanceCoefficient = random.NextDouble();
            double brakingResistanceCoefficient = random.NextDouble();

            AerodynamicData aerodynamicData = AerodynamicDataTestFactory.CreateAerodynamicData();

            // Call
            var aircraftData = new AircraftData(nrOfEngines, maximumThrustPerEngine,
                                                takeOffWeight,
                                                pitchAngleGradient, maximumPitchAngle,
                                                rollingResistanceCoefficient, brakingResistanceCoefficient,
                                                aerodynamicData);

            // Assert
            Assert.AreEqual(nrOfEngines, aircraftData.NrOfEngines);
            Assert.AreEqual(maximumThrustPerEngine, aircraftData.MaximumThrustPerEngine);
            Assert.AreEqual(takeOffWeight, aircraftData.TakeOffWeight);
            Assert.AreEqual(pitchAngleGradient, aircraftData.PitchAngleGradient);
            Assert.AreEqual(maximumPitchAngle, aircraftData.MaximumPitchAngle);
            Assert.AreEqual(rollingResistanceCoefficient, aircraftData.RollingResistanceCoefficient);
            Assert.AreEqual(brakingResistanceCoefficient, aircraftData.BrakingResistanceCoefficient);
            Assert.AreSame(aerodynamicData, aircraftData.AerodynamicData);
        }
    }
}