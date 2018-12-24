using System;
using System.Collections.Generic;
using Core.Common.Data;
using Core.Common.TestUtil;
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
                                                       random.NextDouble(), random.NextAngle(),
                                                       random.NextAngle(), random.NextDouble(),
                                                       random.NextDouble(), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidValuesForPropertiesLessThanZero))]
        public void Constructor_InvalidValuesForPropertiesLargerThanZero_ThrowsArgumentOutOfRangeException(Action constructorAction, string propertyName)
        {
            // Call
            TestDelegate call = () => constructorAction();

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidValuesForPropertiesLargerOrEqualToZero))]
        public void Constructor_InvalidValuesForPropertiesLargerOrEqualToZero_ThrowsArgumentOutOfRangeException(Action constructorAction,
            string propertyName)
        {
            // Call
            TestDelegate call = () => constructorAction();

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger or equal to 0.");
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            int nrOfEngines = random.Next();
            double maximumThrustPerEngine = random.NextDouble();
            double takeOffWeight = random.NextDouble();
            Angle pitchAngleGradient = random.NextAngle();
            Angle maximumPitchAngle = random.NextAngle();
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

        private static IEnumerable<TestCaseData> GetInvalidValuesForPropertiesLessThanZero()
        {
            var random = new Random(21);
            int nrOfEngines = random.Next();
            double maximumThrustPerEngine = random.NextDouble();
            double takeOffWeight = random.NextDouble();
            Angle pitchAngleGradient = random.NextAngle();
            Angle maximumPitchAngle = random.NextAngle();
            double rollingResistanceCoefficient = random.NextDouble();
            double brakingResistanceCoefficient = random.NextDouble();
            var aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();

            const string nrOfenginesPropertyName = "nrOfEngines";
            yield return new TestCaseData(new Action(() => new AircraftData(-1, maximumThrustPerEngine, takeOffWeight, 
                pitchAngleGradient, maximumPitchAngle, 
                rollingResistanceCoefficient, brakingResistanceCoefficient, 
                aerodynamicsData)), nrOfenginesPropertyName)
                .SetName($"{nrOfenginesPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(0, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), nrOfenginesPropertyName)
                .SetName($"{nrOfenginesPropertyName} Zero");

            const string maximumThrustPerEnginePropertyName = "maximumThrustPerEngine";
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, -1e-1, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), maximumThrustPerEnginePropertyName)
                .SetName($"{maximumThrustPerEnginePropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, double.NegativeInfinity, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), maximumThrustPerEnginePropertyName)
                .SetName($"{maximumThrustPerEnginePropertyName} NegativeInfinity");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, 0, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), maximumThrustPerEnginePropertyName)
                .SetName($"{maximumThrustPerEnginePropertyName} Zero");

            const string takeOffWeightPropertyName = "takeOffWeight";
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, -1e-1,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), takeOffWeightPropertyName)
                .SetName($"{takeOffWeightPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, double.NegativeInfinity,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), takeOffWeightPropertyName)
                .SetName($"{takeOffWeightPropertyName} NegativeInfinity");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, 0,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), takeOffWeightPropertyName)
                .SetName($"{takeOffWeightPropertyName} Zero");

            const string pitchAngleGradientPropertyName = "pitchAngleGradient";
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    Angle.FromRadians(-1e-1), maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), pitchAngleGradientPropertyName)
                .SetName($"{pitchAngleGradientPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    Angle.FromRadians(double.NegativeInfinity), maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), pitchAngleGradientPropertyName)
                .SetName($"{pitchAngleGradientPropertyName} NegativeInfinity");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    Angle.FromRadians(0), maximumPitchAngle,
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), pitchAngleGradientPropertyName)
                .SetName($"{pitchAngleGradientPropertyName} Zero");

            const string maximumPitchAnglePropertyName = "maximumPitchAngle";
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, Angle.FromRadians(-1e-1), 
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), maximumPitchAnglePropertyName)
                .SetName($"{maximumPitchAnglePropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, Angle.FromRadians(double.NegativeInfinity), 
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), maximumPitchAnglePropertyName)
                .SetName($"{maximumPitchAnglePropertyName} NegativeInfinity");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, Angle.FromRadians(0), 
                    rollingResistanceCoefficient, brakingResistanceCoefficient,
                    aerodynamicsData)), maximumPitchAnglePropertyName)
                .SetName($"{maximumPitchAnglePropertyName} Zero");
        }

        private static IEnumerable<TestCaseData> GetInvalidValuesForPropertiesLargerOrEqualToZero()
        {
            var random = new Random(21);
            int nrOfEngines = random.Next();
            double maximumThrustPerEngine = random.NextDouble();
            double takeOffWeight = random.NextDouble();
            Angle pitchAngleGradient = random.NextAngle();
            Angle maximumPitchAngle = random.NextAngle();
            double rollingResistanceCoefficient = random.NextDouble();
            double brakingResistanceCoefficient = random.NextDouble();
            var aerodynamicsData = AerodynamicsDataTestFactory.CreateAerodynamicsData();

            const string rollingResistanceCoefficientPropertyName = "rollingResistanceCoefficient";
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                pitchAngleGradient, maximumPitchAngle,
                -1e-1, brakingResistanceCoefficient,
                aerodynamicsData)), rollingResistanceCoefficientPropertyName)
                .SetName($"{rollingResistanceCoefficientPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    double.NegativeInfinity, brakingResistanceCoefficient,
                    aerodynamicsData)), rollingResistanceCoefficientPropertyName)
                .SetName($"{rollingResistanceCoefficientPropertyName} NegativeInfinity");

            const string brakingResistanceCoefficientPropertyName = "brakingResistanceCoefficient";
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, -1e-1,
                    aerodynamicsData)), brakingResistanceCoefficientPropertyName)
                .SetName($"{brakingResistanceCoefficientPropertyName} Negative");
            yield return new TestCaseData(new Action(() => new AircraftData(nrOfEngines, maximumThrustPerEngine, takeOffWeight,
                    pitchAngleGradient, maximumPitchAngle,
                    rollingResistanceCoefficient, double.NegativeInfinity,
                    aerodynamicsData)), brakingResistanceCoefficientPropertyName)
                .SetName($"{brakingResistanceCoefficientPropertyName} NegativeInfinity");
        }
    }
}