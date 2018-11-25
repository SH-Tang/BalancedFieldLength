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
        public static void Constructor_WithoutArguments_ExpectedValues()
        {
            // Call 
            var angle = new Angle();

            // Assert
            Assert.Zero(angle.Degrees);
            Assert.Zero(angle.Radians);
        }

        [Test]
        [TestCase(0 - tolerance)]
        [TestCase(360 + tolerance)]
        public static void FromDegrees_InvalidAngle_ThrowsArgumentOutOfRangeException(double degrees)
        {
            // Call 
            TestDelegate call = () => Angle.FromDegrees(degrees);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(call);
            const string expectedMessage = "Invalid angle, angle must be in the range of [0, 360] degrees";
            string message = exception.Message.Split(new[]
                                                     {
                                                         Environment.NewLine
                                                     }, StringSplitOptions.None).First();
            Assert.AreEqual(expectedMessage, message);
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
        [TestCase(0 - tolerance)]
        [TestCase(2 * Math.PI + tolerance)]
        public static void FromRadians_InvalidAngle_ThrowsArgumentOutOfRangeException(double radians)
        {
            // Call 
            TestDelegate call = () => Angle.FromRadians(radians);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(call);
            const string expectedMessage = "Invalid angle, angle must be in the range of [0, 2 PI] radians";
            string message = exception.Message.Split(new[]
                                                     {
                                                         Environment.NewLine
                                                     }, StringSplitOptions.None).First();
            Assert.AreEqual(expectedMessage, message);
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