using System;
using Core.Common.Data;
using NUnit.Framework;
using Simulator.Data;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test
{
    [TestFixture]
    public class ContinuedTakeOffDynamicsCalculatorTest
    {
        private const double gravitationalAcceleration = SimulationConstants.GravitationalAcceleration;
        private const double airDensity = SimulationConstants.Density;
        private const double tolerance = SimulationConstants.Tolerance;

        [Test]
        public void Constructor_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call
            TestDelegate call = () => new ContinuedTakeOffDynamicsCalculator(null, random.Next(), random.NextDouble(), random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aircraftData", exception.ParamName);
        }

        [Test]
        public void Calculate_AircraftStateNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            var aircraftData = new AircraftData(random.Next(), random.NextDouble(),
                                                random.NextDouble(), random.NextDouble(),
                                                random.NextDouble(), random.NextDouble(),
                                                random.NextDouble(), AerodynamicsDataTestFactory.CreateAerodynamicsData());

            var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble(), random.NextDouble());

            // Call
            TestDelegate call = () => calculator.Calculate(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aircraftState", exception.ParamName);
        }

        [Test]
        public static void Calculate_WithAircraftStateAlways_ReturnsExpectedClimbRate()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var aircraftState = new AircraftState(Angle.FromDegrees(random.NextDouble()),
                                                  Angle.FromDegrees(random.NextDouble()),
                                                  random.NextDouble(),
                                                  random.NextDouble());

            var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble(), random.NextDouble());

            // Call 
            AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

            // Assert
            double expectedClimbRate = aircraftState.TrueAirspeed * Math.Sin(aircraftState.FlightPathAngle.Radians);
            Assert.AreEqual(expectedClimbRate, accelerations.ClimbRate, tolerance);
        }

        [TestFixture]
        public class CalculateTrueAirspeedRate
        {
            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightEqualToThresholdAndLiftLowerThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
                const int nrOfFailedEngines = 1;
                const double airspeed = 10.0;
                const double threshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(aircraftData.MaximumPitchAngle),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      airspeed,
                                                      threshold);

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicsData, angleOfAttack);
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicsData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed);

                double thrustForce = (aircraftData.NrOfEngines - nrOfFailedEngines) * aircraftData.MaximumThrustPerEngine * 1000;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrustForce - dragForce - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightLowerThanThresholdAndLiftHigherThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
                const int nrOfFailedEngines = 1;
                const double airspeed = 100.0;
                const double threshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(aircraftData.MaximumPitchAngle),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      airspeed,
                                                      threshold - random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift > takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicsData, angleOfAttack);
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicsData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed);

                double thrustForce = (aircraftData.NrOfEngines - nrOfFailedEngines) * aircraftData.MaximumThrustPerEngine * 1000;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrustForce - dragForce - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightSmallerThanThresholdAndLiftSmallerThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
                const int nrOfFailedEngines = 1;
                const double airspeed = 10.0;
                const double threshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(aircraftData.MaximumPitchAngle),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      airspeed,
                                                      threshold - random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicsData, angleOfAttack);
                double normalForce = takeOffWeightNewton - lift;
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicsData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed) + normalForce * aircraftData.RollingResistanceCoefficient;

                double thrustForce = (aircraftData.NrOfEngines - nrOfFailedEngines) * aircraftData.MaximumThrustPerEngine * 1000;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrustForce - dragForce - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }
        }

        [TestFixture]
        public class CalculatePitchRate
        {
            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public void Calculate_WithAircraftStateAndSpeedHigherThanRotationSpeedAndPitch_ReturnsExpectedPitchRate(AircraftData aircraftData)
            {
                // Setup
                var random = new Random(21);
                double rotationSpeed = GetRotationSpeed(aircraftData);
                double pitchAngle = aircraftData.MaximumPitchAngle - random.NextDouble();

                var aircraftState = new AircraftState(Angle.FromDegrees(pitchAngle),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double expectedPitchRate = aircraftData.PitchAngleGradient;
                Assert.AreEqual(expectedPitchRate, accelerations.PitchRate.Degrees);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WitAircraftStateSpeedLowerThanRotationSpeed_ReturnsExpectedZeroPitchRate(AircraftData aircraftData)
            {
                // Setup
                double rotationSpeed = GetRotationSpeed(aircraftData);

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(random.NextDouble()),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      rotationSpeed - random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.PitchRate.Degrees);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateSpeedHigherThanRotationSpeedAndPitchAngleAtMaximumPitch_ReturnsExpectedZeroPitchRate(AircraftData aircraftData)
            {
                // Setup
                double rotationSpeed = GetRotationSpeed(aircraftData);

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(aircraftData.MaximumPitchAngle + 0.01),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.PitchRate.Degrees);
            }
        }

        [TestFixture]
        public class CalculateFlightPathRate
        {
            [Test]
            public static void Calculate_WithAirspeedLowerThanThreshold_ReturnsExpectedZeroRate()
            {
                // Setup
                var random = new Random(21);
                AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

                var aircraftState = new AircraftState(Angle.FromDegrees(random.NextDouble()),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      random.NextDouble(),
                                                      random.NextDouble());

                // Precondition
                Assert.IsTrue(aircraftState.TrueAirspeed < 1);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble(), random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.FlightPathRate.Degrees);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAirspeedEqualToThresholdAndNormalForcePresent_ReturnsExpectedFlightPathAngleRate(AircraftData aircraftData)
            {
                // Setup
                const double velocityThreshold = 1.0;
                const double heightThreshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(aircraftData.MaximumPitchAngle),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      velocityThreshold,
                                                      heightThreshold - random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               aircraftState.TrueAirspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.FlightPathRate.Degrees);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAirspeedEqualToThresholdAndNoNormalForcePresent_ReturnsExpectedFlightPathAngleRate(AircraftData aircraftData)
            {
                // Setup
                const double heightThreshold = 0.01;

                var random = new Random(21);
                var aircraftState = new AircraftState(Angle.FromDegrees(aircraftData.MaximumPitchAngle),
                                                      Angle.FromDegrees(random.NextDouble()),
                                                      100,
                                                      heightThreshold - random.NextDouble());

                Angle angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle;

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               aircraftState.TrueAirspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift > takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double expectedRate = (gravitationalAcceleration * (lift - takeOffWeightNewton))
                                      / (takeOffWeightNewton * aircraftState.TrueAirspeed);
                Assert.AreEqual(expectedRate, accelerations.FlightPathRate.Radians, tolerance);
            }
        }

        private static double GetRotationSpeed(AircraftData aircraftData)
        {
            double stallSpeed = AerodynamicsHelper.CalculateStallSpeed(aircraftData.AerodynamicsData,
                                                                       aircraftData.TakeOffWeight * 1000,
                                                                       airDensity);
            double rotationSpeed = stallSpeed * 1.2;
            return rotationSpeed;
        }
    }
}