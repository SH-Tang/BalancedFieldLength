using System;
using System.Collections.Generic;
using Calculator.Data.Helpers;
using NUnit.Framework;

namespace Simulator.Data.Test.Helpers
{
    [TestFixture]
    public class AerodynamicsHelperTest
    {
        private const double density = 1.225; //kg/m3
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
        [TestCaseSource(nameof(GetTestCases))]
        public static void CalculateDragWithoutEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithoutEngineFailure(aerodynamicData, liftCoefficient, density, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicData, liftCoefficient, density, velocity, false);
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
        [TestCaseSource(nameof(GetTestCases))]
        public static void CalculateDragWithEngineFailure_WithValidParameters_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            var random = new Random(21);
            double liftCoefficient = random.NextDouble();
            double velocity = random.NextDouble();

            // Call 
            double drag = AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicData, liftCoefficient, density, velocity);

            // Assert
            double expectedDrag = CalculateExpectedDrag(aerodynamicData, liftCoefficient, density, velocity, true);
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
        [TestCaseSource(nameof(GetTestCases))]
        public static void CalculateStallSpeed_WithValidParametersWithinLimits_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            const double weight = 500e3; // N

            // Call 
            double stallSpeed = AerodynamicsHelper.CalculateStallSpeed(aerodynamicData, weight, density);

            // Assert
            double expectedStallSpeed = Math.Sqrt(2 * weight / (aerodynamicData.MaximumLiftCoefficient * density * aerodynamicData.WingArea));
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
        [TestCaseSource(nameof(GetTestCases))]
        public static void CalculateLift_WithValidParametersAndWithinLimits_ReturnsExpectedValues(AerodynamicData aerodynamicData)
        {
            // Setup
            const double angleOfAttack = 3.0; // degrees
            const int velocity = 10; // m/s

            // Call 
            double lift = AerodynamicsHelper.CalculateLift(aerodynamicData,
                                                               angleOfAttack,
                                                               density,
                                                               velocity);
            // Assert
            Assert.AreEqual(CalculateExpectedLift(aerodynamicData, angleOfAttack, density, velocity), lift);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new AerodynamicData(15, 100, -3, 4.85, 1.60, 0.021, 0.026, 0.85));
            yield return new TestCaseData(new AerodynamicData(14, 200, -4, 4.32, 1.45, 0.024, 0.028, 0.80));
            yield return new TestCaseData(new AerodynamicData(12, 500, -5, 3.95, 1.40, 0.026, 0.029, 0.82));
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