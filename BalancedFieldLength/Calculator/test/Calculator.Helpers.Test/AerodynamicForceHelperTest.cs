using System;
using System.Collections.Generic;
using Calculator.Data;
using NUnit.Framework;

namespace Calculator.Helpers.Test
{
    [TestFixture]
    public class AerodynamicForceHelperTest
    {
        [Test]
        public static void CalculateLift_AerodynamicDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call 
            TestDelegate call = () => AerodynamicForceHelper.CalculateLift(null, random.NextDouble(), 
                                                                           random.NextDouble(), random.NextDouble());

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
            const double density = 1.225; //kg/m3
            const int velocity = 10; // m/s

            // Call 
            var lift = AerodynamicForceHelper.CalculateLift(aerodynamicData,
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
            return 0.5 * liftCoefficient * density * Math.Pow(velocity, 2) * aerodynamicData.WingArea;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}