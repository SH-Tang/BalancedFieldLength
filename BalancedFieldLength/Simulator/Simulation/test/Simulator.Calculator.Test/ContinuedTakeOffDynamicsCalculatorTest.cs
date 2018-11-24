using System;
using Calculator.Data;
using NUnit.Framework;
using Simulator.Data;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test
{
    [TestFixture]
    public class ContinuedTakeOffDynamicsCalculatorTest
    {
        private const double gravitationalAcceleration = 9.81; // m/s^2
        private const double airDensity = 1.225; // kg/m3
        private const double tolerance = 10e-6;

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
                                                random.NextDouble(), AerodynamicDataTestFactory.CreateAerodynamicData());

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
            AircraftData aircraftData = CreateRandomAircraftData();

            var aircraftState = new AircraftState(random.NextDouble(),
                                                  random.NextDouble(),
                                                  random.NextDouble(),
                                                  random.NextDouble());

            var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble(), random.NextDouble());

            // Call 
            AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

            // Assert
            double expectedClimbRate = aircraftState.TrueAirspeed * Math.Sin(DegToRadians(aircraftState.FlightPathAngle));
            Assert.AreEqual(expectedClimbRate, accelerations.ClimbRate, tolerance);
        }

        private static AircraftData CreateRandomAircraftData()
        {
            var random = new Random(21);
            return new AircraftData(random.Next(), random.NextDouble(),
                                    random.NextDouble(), random.NextDouble(),
                                    random.NextDouble(), random.NextDouble(),
                                    random.NextDouble(), AerodynamicDataTestFactory.CreateAerodynamicData());
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
                double pitchAngle = aircraftData.MaximumPitchAngle;

                var random = new Random(21);
                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      airspeed,
                                                      threshold);

                double angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle; // degrees

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicData, angleOfAttack);
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed);

                double thrustForce = (aircraftData.NrOfEngines - nrOfFailedEngines) * aircraftData.MaximumThrustPerEngine * 1000;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(DegToRadians(aircraftState.FlightPathAngle));
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
                double pitchAngle = aircraftData.MaximumPitchAngle;

                var random = new Random(21);
                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      airspeed,
                                                      threshold - random.NextDouble());

                double angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle; // degrees

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift > takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicData, angleOfAttack);
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed);

                double thrustForce = (aircraftData.NrOfEngines - nrOfFailedEngines) * aircraftData.MaximumThrustPerEngine * 1000;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(DegToRadians(aircraftState.FlightPathAngle));
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
                double pitchAngle = aircraftData.MaximumPitchAngle;

                var random = new Random(21);
                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      airspeed,
                                                      threshold - random.NextDouble());

                double angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle; // degrees

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               airspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aircraftData.AerodynamicData, angleOfAttack);
                double normalForce = takeOffWeightNewton - lift;
                double dragForce = AerodynamicsHelper.CalculateDragWithEngineFailure(aircraftData.AerodynamicData,
                                                                                     liftCoefficient,
                                                                                     airDensity,
                                                                                     airspeed) + normalForce * aircraftData.RollingResistanceCoefficient;

                double thrustForce = (aircraftData.NrOfEngines - nrOfFailedEngines) * aircraftData.MaximumThrustPerEngine * 1000;
                double horizontalWeightComponent = takeOffWeightNewton * Math.Sin(DegToRadians(aircraftState.FlightPathAngle));
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

                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      rotationSpeed + random.NextDouble(),
                                                      random.NextDouble());

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

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

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

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

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.PitchRate);
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
                AircraftData aircraftData = CreateRandomAircraftData();

                var aircraftState = new AircraftState(random.NextDouble(),
                                                      random.NextDouble(),
                                                      random.NextDouble(),
                                                      random.NextDouble());

                // Precondition
                Assert.IsTrue(aircraftState.TrueAirspeed < 1);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), random.NextDouble(), random.NextDouble());

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.FlightPathRate);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAirspeedEqualToThresholdAndNormalForcePresent_ReturnsExpectedFlightPathAngleRate(AircraftData aircraftData)
            {
                // Setup
                const double velocityThreshold = 1.0;
                const double heightThreshold = 0.01;

                double pitchAngle = aircraftData.MaximumPitchAngle;

                var random = new Random(21);
                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      velocityThreshold,
                                                      heightThreshold - random.NextDouble());

                double angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle; // degrees

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicData,
                                                               angleOfAttack,
                                                               airDensity,
                                                               aircraftState.TrueAirspeed);
                double takeOffWeightNewton = aircraftData.TakeOffWeight * 1000; // N
                Assert.IsTrue(lift < takeOffWeightNewton);

                var calculator = new ContinuedTakeOffDynamicsCalculator(aircraftData, random.Next(), airDensity, gravitationalAcceleration);

                // Call 
                AircraftAccelerations accelerations = calculator.Calculate(aircraftState);

                // Assert
                Assert.Zero(accelerations.FlightPathRate);
            }

            [Test]
            [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
            public static void Calculate_WithAirspeedEqualToThresholdAndNoNormalForcePresent_ReturnsExpectedFlightPathAngleRate(AircraftData aircraftData)
            {
                // Setup
                const double heightThreshold = 0.01;

                double pitchAngle = aircraftData.MaximumPitchAngle;

                var random = new Random(21);
                var aircraftState = new AircraftState(pitchAngle,
                                                      random.NextDouble(),
                                                      100,
                                                      heightThreshold - random.NextDouble());

                double angleOfAttack = aircraftState.PitchAngle - aircraftState.FlightPathAngle; // degrees

                // Precondition
                double lift = AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicData,
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
                Assert.AreEqual(expectedRate, accelerations.FlightPathRate, tolerance);
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

        private static double DegToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }

    /// <summary>
    /// Class which contains the time derivatives of the aircraft states.
    /// </summary>
    public class AircraftAccelerations
    {
        /// <summary>
        /// Gets the pitch rate. [deg/s]
        /// </summary>
        /// <remarks>Also denoted as dTheta/dt.</remarks>
        public double PitchRate { get; set; }

        /// <summary>
        /// Gets the climb rate. [m/s]
        /// </summary>
        /// <remarks>Also denoted as dh/dt.</remarks>
        public double ClimbRate { get; set; }

        /// <summary>
        /// Gets the true airspeed rate. [m/s^2]
        /// </summary>
        /// <remarks>Also denoted as dVtas/dt.</remarks>
        public double TrueAirSpeedRate { get; set; }

        /// <summary>
        /// Gets the flight path angle rate. [rad/s]
        /// </summary>
        /// <remarks>Also  denoted as dGamma/dt.</remarks>
        public double FlightPathRate { get; set; }
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
        private readonly double gravitationalAcceleration;
        private readonly AerodynamicData aerodynamicData;

        /// <summary>
        /// Creates a new instance of <see cref="ContinuedTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="aircraftData">Tee <see cref="AircraftData"/> which holds
        /// all the information of the aircraft to simulate.</param>
        /// <param name="numberOfFailedEngines">The number of engines which failed during takeoff.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration g0. [m/s^2]</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        public ContinuedTakeOffDynamicsCalculator(AircraftData aircraftData, int numberOfFailedEngines, double density, double gravitationalAcceleration)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            this.aircraftData = aircraftData;
            this.numberOfFailedEngines = numberOfFailedEngines;
            this.density = density;
            this.gravitationalAcceleration = gravitationalAcceleration;
            aerodynamicData = aircraftData.AerodynamicData;
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

            double rotationSpeed = 1.2 * AerodynamicsHelper.CalculateStallSpeed(aerodynamicData,
                                                                                GetNewton(aircraftData.TakeOffWeight),
                                                                                density);

            double trueAirSpeedRate = (gravitationalAcceleration * (CalculateThrust()
                                                                    - CalculateDragForce(aircraftState) - CalculateRollDrag(aircraftState)
                                                                    - GetNewton(aircraftData.TakeOffWeight) * Math.Sin(DegreesToRadians(aircraftState.FlightPathAngle))))
                                      / GetNewton(aircraftData.TakeOffWeight);
            return new AircraftAccelerations
                   {
                       PitchRate = ShouldRotate(rotationSpeed, aircraftState) ? aircraftData.PitchAngleGradient : 0.0,
                       ClimbRate = aircraftState.TrueAirspeed * Math.Sin(DegreesToRadians(aircraftState.FlightPathAngle)),
                       TrueAirSpeedRate = trueAirSpeedRate,
                       FlightPathRate = CalculateFlightPathAngleRate(aircraftState)
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

        private double CalculateFlightPathAngleRate(AircraftState state)
        {
            if (state.TrueAirspeed < 1)
            {
                return 0;
            }

            return (gravitationalAcceleration * (CalculateLift(state) - GetNewton(aircraftData.TakeOffWeight) + CalculateNormalForce(state)))
                   / (GetNewton(aircraftData.TakeOffWeight) * state.TrueAirspeed);
        }

        private double CalculateDragForce(AircraftState state)
        {
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aerodynamicData,
                                                                                 CalculateAngleOfAttack(state));
            return AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicData,
                                                                     liftCoefficient,
                                                                     density,
                                                                     state.TrueAirspeed);
        }

        private double CalculateRollDrag(AircraftState state)
        {
            return aircraftData.RollingResistanceCoefficient * CalculateNormalForce(state);
        }

        private double CalculateNormalForce(AircraftState state)
        {
            double normalForce = GetNewton(aircraftData.TakeOffWeight) - CalculateLift(state);
            if (state.Height >= 0.01 || normalForce < 0)
            {
                return 0;
            }

            return normalForce;
        }

        private double CalculateLift(AircraftState state)
        {
            return AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicData,
                                                    CalculateAngleOfAttack(state),
                                                    density,
                                                    state.TrueAirspeed);
        }

        private static double CalculateAngleOfAttack(AircraftState state)
        {
            return state.PitchAngle - state.FlightPathAngle;
        }

        private double CalculateThrust()
        {
            return (aircraftData.NrOfEngines - numberOfFailedEngines) * GetNewton(aircraftData.MaximumThrustPerEngine);
        }

        private static double GetNewton(double kiloNewton)
        {
            return kiloNewton * 1000;
        }
    }
}