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