using System;
using Calculator.Data;
using NUnit.Framework;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test
{
    [TestFixture]
    public class ContinuedTakeOffDynamicsCalculatorTest
    {
        private const double airDensity = 1.225; // kg/m3;
        private const double tolerance = 10e-6;

        [Test]
        public void Constructor_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call
            TestDelegate call = () => new ContinuedTakeOffDynamicsCalculator(null, random.Next(), random.NextDouble());

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
                                                random.NextDouble(), AerodynamicDataTestFactory.CreateAerodynamicData());

            var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble());

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
            var aircraftData = new AircraftData(random.Next(), random.NextDouble(),
                                                random.NextDouble(), random.NextDouble(),
                                                random.NextDouble(), random.NextDouble(),
                                                random.NextDouble(), AerodynamicDataTestFactory.CreateAerodynamicData());

            var aircraftState = new AircraftState(random.NextDouble(),
                                                  random.NextDouble(),
                                                  random.NextDouble(),
                                                  random.NextDouble());

            var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble());

            // Call 
            AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

            // Assert
            double expectedClimbRate = aircraftState.TrueAirspeed * Math.Sin(DegToRadians(aircraftState.FlightPathAngle));
            Assert.AreEqual(expectedClimbRate, accelerations.ClimbRate, tolerance);
        }

        private static double DegToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        [TestFixture]
        public class CalculatePitchRate
        {
            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public void Calculate_WithAircraftStateAndSpeedLargerThanRotationSpeedAndPitch_ReturnsExpectedPitchRate(AircraftData aircraftData)
            {
                // Setup
                var random = new Random(21);
                double rotationSpeed = GetRotationSpeed(aircraftData);
                double pitchAngle = aircraftData.MaximumPitchAngle - random.NextDouble();

                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double expectedPitchRate = aircraftData.PitchAngleGradient;
                Assert.AreEqual(expectedPitchRate, accelerations.PitchRate);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WitAircraftStateSpeedLowerThanRotationSpeed_ReturnsExpectedZeroPitchRate(AircraftData aircraftData)
            {
                // Setup
                double rotationSpeed = GetRotationSpeed(aircraftData);

                var random = new Random(21);
                var aircraftState = new AircraftState(random.NextDouble(),
                                                      random.NextDouble(),
                                                      rotationSpeed - random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.PitchRate);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAircraftStateSpeedHigherThanRotationSpeedAndPitchAngleAtMaximumPitch_ReturnsExpectedZeroPitchRate(AircraftData aircraftData)
            {
                // Setup
                double rotationSpeed = GetRotationSpeed(aircraftData);

                var random = new Random(21);
                var aircraftState = new AircraftState(aircraftData.MaximumPitchAngle,
                                                      random.NextDouble(),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.PitchRate);
            }
        }

        private static double GetRotationSpeed(AircraftData aircraftData)
        {
            double stallSpeed = AerodynamicsHelper.CalculateStallSpeed(aircraftData.AerodynamicData,
                                                                       aircraftData.TakeOffWeight * 1000,
                                                                       airDensity);
            double rotationSpeed = stallSpeed * 1.2;
            return rotationSpeed;
        }
    }

    /// <summary>
    /// Class which contains the time derivatives of the aircraft states.
    /// </summary>
    public class AircraftAccelerations
    {
        public double PitchRate { get; set; }
        public double ClimbRate { get; set; }
    }

    /// <summary>
    /// Class which contains the aircraft states.
    /// </summary>
    public class AircraftState
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftState"/>.
        /// </summary>
        /// <param name="pitchAngle">The pitch angle. [deg]</param>
        /// <param name="flightPathAngle">The flight path angle. [deg]</param>
        /// <param name="trueAirspeed">The true airspeed. [m/s]</param>
        /// <param name="height">The height. [m]</param>
        public AircraftState(double pitchAngle, double flightPathAngle, double trueAirspeed, double height)
        {
            PitchAngle = pitchAngle;
            FlightPathAngle = flightPathAngle;
            TrueAirspeed = trueAirspeed;
            Height = height;
        }

        /// <summary>
        /// Gets the pitch angle. [deg]
        /// </summary>
        /// <remarks>Also denoted as theta.</remarks>
        public double PitchAngle { get; }

        /// <summary>
        /// Gets the flight path angle. [deg]
        /// </summary>
        /// <remarks>Also denoted as gamma.</remarks>
        public double FlightPathAngle { get; }

        /// <summary>
        /// Gets the true airspeed. [m/s]
        /// </summary>
        /// <remarks>Also denoted as Vtas.</remarks>
        public double TrueAirspeed { get; }

        /// <summary>
        /// Gets the height. [m]
        /// </summary>
        /// <remarks>Also denoted as h.</remarks>
        public double Height { get; }
    }

    /// <summary>
    /// Class which describes the calculation of the aircraft dynamics
    /// when the take off is continued after engine failure.
    /// </summary>
    public class ContinuedTakeOffDynamicsCalculator
    {
        private readonly AircraftData aircraftData;
        private readonly int numberOfFailedEngines;
        private readonly double density;

        /// <summary>
        /// Creates a new instance of <see cref="ContinuedTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="aircraftData">THe <see cref="AircraftData"/> which holds
        /// all the information of the aircraft to simulate.</param>
        /// <param name="numberOfFailedEngines">The number of engines which failed during takeoff.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        public ContinuedTakeOffDynamicsCalculator(AircraftData aircraftData, int numberOfFailedEngines, double density)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            this.aircraftData = aircraftData;
            this.numberOfFailedEngines = numberOfFailedEngines;
            this.density = density;
        }

        /// <summary>
        /// Calculates the accelerations acting on the aircraft based
        /// on <see cref="AircraftState"/>.
        /// </summary>
        /// <param name="aircraftState">The <see cref="AircraftState"/>
        /// the aircraft is currently in.</param>
        /// <returns>The <see cref="AircraftAccelerations"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftState"/>
        /// is <c>null</c>.</exception>
        public AircraftAccelerations Calculate(AircraftState aircraftState)
        {
            if (aircraftState == null)
            {
                throw new ArgumentNullException(nameof(aircraftState));
            }

            double rotationSpeed = 1.2 * AerodynamicsHelper.CalculateStallSpeed(aircraftData.AerodynamicData,
                                                                                aircraftData.TakeOffWeight * 1000,
                                                                                density);

            return new AircraftAccelerations
                   {
                       PitchRate = ShouldRotate(rotationSpeed, aircraftState) ? aircraftData.PitchAngleGradient : 0.0,
                       ClimbRate = aircraftState.TrueAirspeed * Math.Sin(DegreesToRadians(aircraftState.FlightPathAngle))
                   };
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private bool ShouldRotate(double rotationSpeed, AircraftState aircraftState)
        {
            return aircraftState.TrueAirspeed >= rotationSpeed
                   && aircraftState.PitchAngle < aircraftData.MaximumPitchAngle;
        }
    }
}