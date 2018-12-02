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
    public class AircraftDynamicsCalculatorBaseTest
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
            TestDelegate call = () => new TestAircraftfDynamicsCalculator(null, random.NextDouble(), random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aircraftData", exception.ParamName);
        }

        [Test]
        public void Calculate_AircraftStateNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            var calculator = new TestAircraftfDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble());

            // Call
            TestDelegate call = () => calculator.Calculate(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aircraftState", exception.ParamName);
        }

        [Test]
        public static void Calculate_Always_SendsCorrectInput()
        {
            // Setup
            var random = new Random(21);

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            var calculator = new TestAircraftfDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble());

            var state = new AircraftState(random.NextAngle(),
                                          random.NextAngle(),
                                          random.NextDouble(),
                                          random.NextDouble());

            // Call 
            calculator.Calculate(state);

            // Assert
            Assert.AreSame(state, calculator.CalculateDragInput);
            Assert.AreSame(state, calculator.CalculateRollDragInput);
            Assert.AreSame(state, calculator.CalculatePitchRateInput);
        }

        [Test]
        public static void Calculate_WithAircraftStateAlways_ReturnsExpectedClimbRate()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var aircraftState = new AircraftState(random.NextAngle(),
                                                  random.NextAngle(),
                                                  random.NextDouble(),
                                                  random.NextDouble());

            var calculator = new TestAircraftfDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble());

            // Call 
            AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

            // Assert
            double expectedClimbRate = aircraftState.TrueAirspeed * Math.Sin(aircraftState.FlightPathAngle.Radians);
            Assert.AreEqual(expectedClimbRate, accelerations.ClimbRate, tolerance);
        }

        [Test]
        public static void Calculate_WithAircraftState_ReturnsExpectedPitchRate()
        {
            // Setup

            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var aircraftState = new AircraftState(random.NextAngle(),
                                                  random.NextAngle(),
                                                  random.NextDouble(),
                                                  random.NextDouble());

            Angle pitchRate = random.NextAngle();
            var calculator = new TestAircraftfDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble())
                             {
                                 PitchRate = pitchRate
                             };

            // Call 
            AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

            // Assert
            Assert.AreEqual(pitchRate, accelerations.PitchRate);
        }

        [TestFixture]
        public class CalculateTrueAirspeedRate
        {
            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightEqualToThresholdAndLiftLowerThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
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

                double thrust = random.NextDouble() * 1000;
                double drag = random.NextDouble() * 100;
                var calculator = new TestAircraftfDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration)
                                 {
                                     Thrust = thrust,
                                     Drag = drag
                                 };

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrust - drag - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightLowerThanThresholdAndLiftHigherThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
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

                double thrust = random.NextDouble() * 1000;
                double drag = random.NextDouble() * 100;
                var calculator = new TestAircraftfDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration)
                                 {
                                     Thrust = thrust,
                                     Drag = drag
                                 };

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrust - drag - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateHeightSmallerThanThresholdAndLiftSmallerThanTakeOffWeight_ReturnsExpectedTrueAirspeedRate(AircraftData aircraftData)
            {
                // Setup
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

                double thrust = random.NextDouble() * 1000;
                double drag = random.NextDouble() * 100;
                var calculator = new TestAircraftfDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration)
                                 {
                                     Thrust = thrust,
                                     Drag = drag
                                 };

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(aircraftState.FlightPathAngle.Radians);
                double expectedAcceleration = (gravitationalAcceleration * (thrust - drag - horizontalWeightComponent))
                                              / takeOffWeightNewton;
                Assert.AreEqual(expectedAcceleration, accelerations.TrueAirSpeedRate, tolerance);
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

                var aircraftState = new AircraftState(random.NextAngle(),
                                                      random.NextAngle(),
                                                      random.NextDouble(),
                                                      random.NextDouble());

                // Precondition
                Assert.IsTrue(aircraftState.TrueAirspeed < 1);

                var calculator = new TestAircraftfDynamicsCalculator(aircraftData, random.NextDouble(), random.NextDouble());

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
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
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

                var calculator = new TestAircraftfDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration);

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
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextAngle(),
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

                var calculator = new TestAircraftfDynamicsCalculator(aircraftData, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double expectedRate = (gravitationalAcceleration * (lift - takeOffWeightNewton))
                                      / (takeOffWeightNewton * aircraftState.TrueAirspeed);
                Assert.AreEqual(expectedRate, accelerations.FlightPathRate.Radians, tolerance);
            }
        }
    }

    public class TestAircraftfDynamicsCalculator : AircraftDynamicsCalculatorBase
    {
        public TestAircraftfDynamicsCalculator(AircraftData aircraftData, double density, double gravitationalAcceleration)
            : base(aircraftData, density, gravitationalAcceleration) {}

        public double Thrust { private get; set; }

        public AircraftState CalculateRollDragInput { get; private set; }

        public double RollDrag { private get; set; }
        public double Drag { private get; set; }
        public AircraftState CalculateDragInput { get; private set; }

        public Angle PitchRate { private get; set; }

        public AircraftState CalculatePitchRateInput { get; private set; }

        protected override double CalculateRollDrag(AircraftState state)
        {
            CalculateRollDragInput = state;
            return RollDrag;
        }

        protected override double CalculateThrust()
        {
            return Thrust;
        }

        protected override double CalculateDragForce(AircraftState state)
        {
            CalculateDragInput = state;
            return Drag;
        }

        protected override Angle CalculatePitchRate(AircraftState state)
        {
            CalculatePitchRateInput = state;
            return PitchRate;
        }
    }
}