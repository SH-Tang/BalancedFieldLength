using System;
using Core.Common.Data;
using NUnit.Framework;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Data.Test.Helpers
{
    [TestFixture]
    public class AerodynamicsHelperTest
    {
        private const double airDensity = SimulationConstants.Density;
        private const double tolerance = SimulationConstants.Tolerance;

        [Test]
        public static void CalculateDragWithoutEngineFailure_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateDragWithoutEngineFailure(null,
                                                                                           random.NextDouble(),
                                                                                           random.NextDouble(),
                                                                                           random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateDragWithoutEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicsData, liftCoefficient, airDensity, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicsData, liftCoefficient, airDensity, velocity, false);
            Assert.AreEqual(expectedDrag, drag, tolerance);
        }

        [Test]
        public static void CalculateDragWithEngineFailure_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateDragWithEngineFailure(null,
                                                                                        random.NextDouble(),
                                                                                        random.NextDouble(),
                                                                                        random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateDragWithEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData, liftCoefficient, airDensity, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicsData, liftCoefficient, airDensity, velocity, true);
            Assert.AreEqual(expectedDrag, drag, tolerance);
        }

        [Test]
        public static void CalculateStallSpeed_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateStallSpeed(null,
                                                                             random.NextDouble(),
                                                                             random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateStallSpeed_WithValidParametersWithinLimits_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            const double weight = 500e3; // N

            // Call 
            double stallSpeed = AerodynamicsHelper.CalculateStallSpeed(aerodynamicsData, weight, airDensity);

            // Assert
            double expectedStallSpeed = Math.Sqrt(2 * weight / (aerodynamicsData.MaximumLiftCoefficient * airDensity * aerodynamicsData.WingArea));
            Assert.AreEqual(expectedStallSpeed, stallSpeed, tolerance);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateLiftCoefficient_WithAerodynamicsData_ReturnsExpectedLiftCoefficient(AerodynamicsData aerodynamicsData)
        {
            // Setup
            var random = new Random(21);
            Angle angleOfAttack = Angle.FromDegrees(random.NextDouble()); 

            // Call 
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aerodynamicsData,
                                                                                 angleOfAttack);

            // Assert
            double expectedLiftCoefficient = CalculateExpectedLiftCoefficient(aerodynamicsData, angleOfAttack);
            Assert.AreEqual(expectedLiftCoefficient, liftCoefficient, tolerance);
        }

        [Test]
        public static void CalculateLift_AerodynamicsDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateLift(null,
                                                                       new Angle(),
                                                                       random.NextDouble(),
                                                                       random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicsData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicsDataTestCases))]
        public static void CalculateLift_WithValidParametersAndWithinLimits_ReturnsExpectedValues(AerodynamicsData aerodynamicsData)
        {
            // Setup
            Angle angleOfAttack = Angle.FromDegrees(3.0);
            const int velocity = 10; // m/s

            // Call 
            double lift = AerodynamicsHelper.CalculateLift(aerodynamicsData,
                                                           angleOfAttack,
                                                           airDensity,
                                                           velocity);
            // Assert
            Assert.AreEqual(CalculateExpectedLift(aerodynamicsData, angleOfAttack, airDensity, velocity), lift);
        }

        private static double CalculateExpectedLift(AerodynamicsData aerodynamicsData,
                                                    Angle angleOfAttack,
                                                    double density,
                                                    double velocity)
        {
            double liftCoefficient = CalculateExpectedLiftCoefficient(aerodynamicsData, angleOfAttack);
            return liftCoefficient * CalculateDynamicPressure(velocity, density, aerodynamicsData.WingArea);
        }

        private static double CalculateExpectedLiftCoefficient(AerodynamicsData aerodynamicsData, Angle angleOfAttack)
        {
            return aerodynamicsData.LiftCoefficientGradient *
                   DegreesToRadians(angleOfAttack.Degrees - aerodynamicsData.ZeroLiftAngleOfAttack);
        }

        private static double CalculateExpectedDrag(AerodynamicsData aerodynamicsData,
                                                    double liftCoefficient,
                                                    double density,
                                                    double velocity,
                                                    bool hasEngineFailed)
        {
            double staticDragCoefficient = hasEngineFailed
                                               ? aerodynamicsData.RestDragCoefficientWithEngineFailure
                                               : aerodynamicsData.RestDragCoefficientWithoutEngineFailure;

            double inducedDragCoefficient = Math.Pow(liftCoefficient, 2) / (Math.PI * aerodynamicsData.AspectRatio * aerodynamicsData.OswaldFactor);

            double totalDragCoefficient = staticDragCoefficient + inducedDragCoefficient;
            return totalDragCoefficient * CalculateDynamicPressure(velocity, density, aerodynamicsData.WingArea);
        }

        private static double CalculateDynamicPressure(double velocity, double density, double wingArea)
        {
            return 0.5 * density * Math.Pow(velocity, 2) * wingArea;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}