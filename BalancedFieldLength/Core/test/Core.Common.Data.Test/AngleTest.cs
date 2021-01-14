// Copyright (C) 2018 Dennis Tang. All rights reserved.
//
// This file is part of Balanced Field Length.
//
// Balanced Field Length is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
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
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
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
        public void Equals_Null_ReturnsFalse()
        {
            // Setup
            var random = new Random(21);
            Angle angle = Angle.FromDegrees(random.NextDouble());

            // Call 
            bool result = angle.Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_DifferentObject_ReturnsFalse()
        {
            // Setup
            var random = new Random(21);
            Angle angle = Angle.FromDegrees(random.NextDouble());

            // Call 
            bool result = angle.Equals(new object());

            // Assert
            Assert.IsFalse(result);
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
        public static void GetHashCode_EqualAngles_ReturnsSameHashCode()
        {
            // Setup
            var random = new Random(21);
            double angle = random.NextDouble();
            Angle angle1 = Angle.FromDegrees(angle);
            Angle angle2 = Angle.FromDegrees(angle);

            // Precondition
            Assert.IsTrue(angle1.Equals(angle2));

            // Call 
            int hashCodeAngle1 = angle1.GetHashCode();
            int hashCodeAngle2 = angle2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodeAngle1, hashCodeAngle2);
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