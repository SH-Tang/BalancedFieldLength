using System;
using System.Linq;
using NUnit.Framework;

namespace Core.Common.Data.Test
{
    [TestFixture]
    public class AngleTest
    {
        private const double tolerance = 10e-6;

        [Test]
        [TestCase(0 - tolerance)]
        [TestCase(360 + tolerance)]
        public static void Constructor_InvalidAngle_ThrowsArgumentOutOfRangeException(double degrees)
        {
            // Call 
            TestDelegate call = () => new Angle(degrees);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(call);
            const string expectedMessage = "Invalid angle, angle must be in the range of [0, 360]";
            string message = exception.Message.Split(new[]
                                                     {
                                                         Environment.NewLine
                                                     }, StringSplitOptions.None).First();
            Assert.AreEqual(expectedMessage, message);
        }

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
        public static void GivenAngleWithValidValues_WhenCallingAsDegrees_ReturnsAngleInDegrees(double degrees)
        {
            // Given
            var angle = new Angle(degrees);

            // When 
            double angleInDegrees = angle.Degrees;

            // Then
            Assert.AreEqual(degrees, angleInDegrees);
        }

        [Test]
        [TestCase(0)]
        [TestCase(180)]
        [TestCase(360)]
        [TestCase(double.NaN)]
        public static void GivenAngleWithValidValues_WhenCallingAsRadians_ThenReturnsAngleInRadians(double degrees)
        {
            // Given
            var angle = new Angle(degrees);

            // When 
            double angleInRadians = angle.Radians;

            // Then
            Assert.AreEqual(DegreesToRadians(degrees), angleInRadians, tolerance);
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}