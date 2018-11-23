using System;
using NUnit.Framework;
using Simulator.Data.Helpers;
using Simulator.Data.TestUtil;

namespace Simulator.Data.Test.Helpers
{
    [TestFixture]
    public class AerodynamicsHelperTest
    {
        private const double airDensity = 1.225; //kg/m3
        private const double tolerance = 10e-3;

        [Test]
        public static void CalculateDragWithoutEngineFailure_AerodynamicDataNull_ThrowsArgumentNullException()
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
            Assert.AreEqual("aerodynamicData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicDataTestCases))]
        public static void CalculateDragWithoutEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicData, liftCoefficient, airDensity, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicData, liftCoefficient, airDensity, velocity, false);
            Assert.AreEqual(expectedDrag, drag, tolerance);
        }

        [Test]
        public static void CalculateDragWithEngineFailure_AerodynamicDataNull_ThrowsArgumentNullException()
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
            Assert.AreEqual("aerodynamicData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicDataTestCases))]
        public static void CalculateDragWithEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicData, liftCoefficient, airDensity, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicData, liftCoefficient, airDensity, velocity, true);
            Assert.AreEqual(expectedDrag, drag, tolerance);
        }

        [Test]
        public static void CalculateStallSpeed_AerodynamicDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateStallSpeed(null,
                                                                             random.NextDouble(),
                                                                             random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicDataTestCases))]
        public static void CalculateStallSpeed_WithValidParametersWithinLimits_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            const double weight = 500e3; // N

            // Call 
            double stallSpeed = AerodynamicsHelper.CalculateStallSpeed(aerodynamicData, weight, airDensity);

            // Assert
            double expectedStallSpeed = Math.Sqrt(2 * weight / (aerodynamicData.MaximumLiftCoefficient * airDensity * aerodynamicData.WingArea));
            Assert.AreEqual(expectedStallSpeed, stallSpeed, tolerance);
        }

        [Test]
        public static void CalculateLift_AerodynamicDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicsHelper.CalculateLift(null,
                                                                       random.NextDouble(),
                                                                       random.NextDouble(),
                                                                       random.NextDouble());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aerodynamicData", exception.ParamName);
        }

        [Test]
        [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicDataTestCases))]
        public static void CalculateLift_WithValidParametersAndWithinLimits_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            const double angleOfAttack = 3.0; // degrees
            const int velocity = 10; // m/s

            // Call 
            double lift = AerodynamicsHelper.CalculateLift(aerodynamicData,
                                                           angleOfAttack,
                                                           airDensity,
                                                           velocity);
            // Assert
            Assert.AreEqual(CalculateExpectedLift(aerodynamicData, angleOfAttack, airDensity, velocity), lift);
        }

        private static double CalculateExpectedLift(AerodynamicData aerodynamicData,
                                                    double angleOfAttack,
                                                    double density,
                                                    double velocity)
        {
            double liftCoefficient = aerodynamicData.LiftCoefficientGradient *
                                     DegreesToRadians(angleOfAttack - aerodynamicData.ZeroLiftAngleOfAttack);
            return liftCoefficient * CalculateDynamicPressure(velocity, density, aerodynamicData.WingArea);
        }

        private static double CalculateExpectedDrag(AerodynamicData aerodynamicData,
                                                    double liftCoefficient,
                                                    double density,
                                                    double velocity,
                                                    bool hasEngineFailed)
        {
            double staticDragCoefficient = hasEngineFailed
                                               ? aerodynamicData.RestDragCoefficientWithEngineFailure
                                               : aerodynamicData.RestDragCoefficientWithoutEngineFailure;

            double inducedDragCoefficient = Math.Pow(liftCoefficient, 2) / (Math.PI * aerodynamicData.AspectRatio * aerodynamicData.OswaldFactor);

            double totalDragCoefficient = staticDragCoefficient + inducedDragCoefficient;
            return totalDragCoefficient * CalculateDynamicPressure(velocity, density, aerodynamicData.WingArea);
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