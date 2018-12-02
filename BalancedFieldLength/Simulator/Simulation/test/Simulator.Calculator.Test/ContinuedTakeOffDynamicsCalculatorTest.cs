using System;
using Core.Common.Data;
using Core.Common.TestUtil;
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
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            // Call
            var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData,
                                                                    random.Next(),
                                                                    random.NextDouble(),
                                                                    random.NextDouble());

            // Assert
            Assert.IsInstanceOf<TakeoffDynamicsCalculatorBase>(calculator);
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
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
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
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
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
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
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
                double pitchAngle = aircraftData.MaximumPitchAngle.Degrees - random.NextDouble();

                var aircraftState = new AircraftState(Angle.FromDegrees(pitchAngle),
                                                      random.NextAngle(),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Angle expectedPitchRate = aircraftData.PitchAngleGradient;
                Assert.AreEqual(expectedPitchRate, accelerations.PitchRate);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WitAircraftStateSpeedLowerThanRotationSpeed_ReturnsExpectedZeroPitchRate(AircraftData aircraftData)
            {
                // Setup
                double rotationSpeed = GetRotationSpeed(aircraftData);

                var random = new Random(21);
                var aircraftState = new AircraftState(random.NextAngle(),
                                                      random.NextAngle(),
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
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.PitchRate.Degrees);
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