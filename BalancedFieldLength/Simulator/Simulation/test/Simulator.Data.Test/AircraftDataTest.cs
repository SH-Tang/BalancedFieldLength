using System;
using Core.Common.Data;
using NUnit.Framework;
using Simulator.Data.TestUtil;

namespace Simulator.Data.Test
{
    [TestFixture]
    public class AircraftDataTest
    {
        [Test]
        public static void Constructor_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => new AircraftData(random.Next(), random.NextDouble(),
                                                       random.NextDouble(), new Angle(), 
                                                       new Angle(), random.NextDouble(),
                                                       random.NextDouble(), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            int nrOfEngines = random.Next();
            double maximumThrustPerEngine = random.NextDouble();
            double takeOffWeight = random.NextDouble();
            Angle pitchAngleGradient = Angle.FromDegrees(random.NextDouble());
            Angle maximumPitchAngle = Angle.FromDegrees(random.NextDouble());
            double rollingResistanceCoefficient = random.NextDouble();
            double brakingResistanceCoefficient = random.NextDouble();

            AerodynamicsData aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();

            // Call
            var aircraftData = new AircraftData(nrOfEngines, maximumThrustPerEngine,
                                                takeOffWeight,
                                                pitchAngleGradient, maximumPitchAngle,
                                                rollingResistanceCoefficient, brakingResistanceCoefficient,
                                                aerodynamicsData);

            // Assert
            Assert.AreEqual(nrOfEngines, aircraftData.NrOfEngines);
            Assert.AreEqual(maximumThrustPerEngine, aircraftData.MaximumThrustPerEngine);
            Assert.AreEqual(takeOffWeight, aircraftData.TakeOffWeight);
            Assert.AreEqual(pitchAngleGradient.Degrees, aircraftData.PitchAngleGradient.Degrees);
            Assert.AreEqual(maximumPitchAngle.Degrees, aircraftData.MaximumPitchAngle.Degrees);
            Assert.AreEqual(rollingResistanceCoefficient, aircraftData.RollingResistanceCoefficient);
            Assert.AreEqual(brakingResistanceCoefficient, aircraftData.BrakingResistanceCoefficient);
            Assert.AreSame(aerodynamicsData, aircraftData.AerodynamicsData);
        }
    }
}