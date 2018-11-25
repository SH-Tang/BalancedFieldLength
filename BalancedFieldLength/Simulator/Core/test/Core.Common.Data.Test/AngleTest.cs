using System;
using NUnit.Framework;

namespace Core.Common.Data.Test
{
    [TestFixture]
    public class AngleTest
    {
        private const double tolerance = 10e-6;

        [Test]
        public static void Constructor_WithoutArguments_ExpectedValues()
        {
            // Call 
            var angle = new Angle();

            // Assert
            Assert.Zero(angle.Degrees);
            Assert.Zero(angle.Radians);
        }

        [Test]
        [TestCase(0)]
        [TestCase(180)]
        [TestCase(360)]
        [TestCase(double.NaN)]
        public static void FromDegrees_WithValidValues_ReturnsExpectedAngle(double degrees)
        {
            // Call
            Angle angle = Angle.FromDegrees(degrees);

            // Then
            Assert.AreEqual(degrees, angle.Degrees);
            Assert.AreEqual(DegreesToRadians(degrees), angle.Radians);
        }

        [Test]
        [TestCase(0)]
        [TestCase(Math.PI)]
        [TestCase(2 * Math.PI)]
        [TestCase(double.NaN)]
        public static void FromRadians_WithValidValues_ThenReturnsExpectedAngle(double radians)
        {
            // Call
            Angle angle = Angle.FromRadians(radians);

            // Assert
            Assert.AreEqual(RadiansToDegrees(radians), angle.Degrees, tolerance);
            Assert.AreEqual(radians, angle.Radians);
        }

        [Test]
        public static void AdditionOperator_WhenAddingAngles_ReturnExpectedAngle()
        {
            // Setup
            var random = new Random(21);
            Angle angle1 = Angle.FromDegrees(random.NextDouble());
            Angle angle2 = Angle.FromDegrees(random.NextDouble());

            // Call 
            Angle result = angle1 + angle2;

            // Assert
            Assert.AreEqual(angle1.Degrees + angle2.Degrees, result.Degrees, tolerance);
            Assert.AreEqual(angle1.Radians + angle2.Radians, result.Radians);
        }

        [Test]
        public static void SubtractionOperator_WhenSubtractingAngles_ReturnExpectedAngle()
        {
            // Setup
            var random = new Random(21);
            Angle angle1 = Angle.FromDegrees(random.NextDouble());
            Angle angle2 = Angle.FromDegrees(random.NextDouble());

            // Call 
            Angle result = angle1 - angle2;

            // Assert
            Assert.AreEqual(angle1.Degrees - angle2.Degrees, result.Degrees, tolerance);
            Assert.AreEqual(angle1.Radians - angle2.Radians, result.Radians);
        }

        [Test]
        [TestCase(10, 20, false)]
        [TestCase(20, 20, false)]
        [TestCase(20, 10, true)]
        public static void GreaterThanOperator_WhenComparingAngles_ReturnsExpectedResult(double leftAngle,
                                                                                         double rightAngle,
                                                                                         bool expectedResult)
        {
            // Setup
            Angle angle1 = Angle.FromDegrees(leftAngle);
            Angle angle2 = Angle.FromDegrees(rightAngle);

            // Call 
            bool result = angle1 > angle2;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(10, 20, false)]
        [TestCase(20, 20, true)]
        [TestCase(20, 10, true)]
        public static void GreaterThanOrEqualToOperator_WhenComparingAngles_ReturnsExpectedResult(double leftAngle,
                                                                                                  double rightAngle,
                                                                                                  bool expectedResult)
        {
            // Setup
            Angle angle1 = Angle.FromDegrees(leftAngle);
            Angle angle2 = Angle.FromDegrees(rightAngle);

            // Call 
            bool result = angle1 >= angle2;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(10, 20, true)]
        [TestCase(20, 20, false)]
        [TestCase(20, 10, false)]
        public static void SmallerThanOperator_WhenComparingAngles_ReturnsExpectedResult(double leftAngle,
                                                                                         double rightAngle,
                                                                                         bool expectedResult)
        {
            // Setup
            Angle angle1 = Angle.FromDegrees(leftAngle);
            Angle angle2 = Angle.FromDegrees(rightAngle);

            // Call 
            bool result = angle1 < angle2;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(10, 20, true)]
        [TestCase(20, 20, true)]
        [TestCase(20, 10, false)]
        public static void SmallerThanOrEqualToOperator_WhenComparingAngles_ReturnsExpectedResult(double leftAngle,
                                                                                                  double rightAngle,
                                                                                                  bool expectedResult)
        {
            // Setup
            Angle angle1 = Angle.FromDegrees(leftAngle);
            Angle angle2 = Angle.FromDegrees(rightAngle);

            // Call 
            bool result = angle1 <= angle2;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(10, 20, false)]
        [TestCase(20, 20, true)]
        [TestCase(20, 10, false)]
        public static void EqualsOperator_WhenComparingAngles_ReturnsExpectedResult(double leftAngle,
                                                                                    double rightAngle,
                                                                                    bool expectedResult)
        {
            // Setup
            Angle angle1 = Angle.FromDegrees(leftAngle);
            Angle angle2 = Angle.FromDegrees(rightAngle);

            // Call 
            bool result = angle1 == angle2;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(10, 20, true)]
        [TestCase(20, 20, false)]
        [TestCase(20, 10, true)]
        public static void UnequalOperator_WhenComparingAngles_ReturnsExpectedResult(double leftAngle,
                                                                                     double rightAngle,
                                                                                     bool expectedResult)
        {
            // Setup
            Angle angle1 = Angle.FromDegrees(leftAngle);
            Angle angle2 = Angle.FromDegrees(rightAngle);

            // Call 
            bool result = angle1 != angle2;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public static void Equals_HavingSameReference_ReturnsTrue()
        {
            // Setup
            var random = new Random(21);
            Angle angle1 = Angle.FromDegrees(random.NextDouble());
            Angle angle2 = angle1;

            // Call 
            bool result12 = angle1.Equals(angle2);
            bool result21 = angle2.Equals(angle1);

            // Assert
            Assert.IsTrue(result12);
            Assert.IsTrue(result21);
        }

        [Test]
        public static void Equals_HavingSameValues_ReturnsTrue()
        {
            // Setup
            var random = new Random(21);
            double angle = random.NextDouble();
            Angle angle1 = Angle.FromDegrees(angle);
            Angle angle2 = Angle.FromDegrees(angle);

            // Call 
            bool result12 = angle1.Equals(angle2);
            bool result21 = angle2.Equals(angle1);

            // Assert
            Assert.IsTrue(result12);
            Assert.IsTrue(result21);
        }

        [Test]
        public static void Equals_HavingDifferentValues_ReturnsFalse()
        {
            // Setup
            var random = new Random(21);
            Angle angle1 = Angle.FromDegrees(random.NextDouble());
            Angle angle2 = Angle.FromDegrees(random.NextDouble());

            // Call 
            bool result12 = angle1.Equals(angle2);
            bool result21 = angle2.Equals(angle1);

            // Assert
            Assert.IsFalse(result12);
            Assert.IsFalse(result21);
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private static double RadiansToDegrees(double radians)
        {
            return (radians * 180) / Math.PI;
        }
    }
}