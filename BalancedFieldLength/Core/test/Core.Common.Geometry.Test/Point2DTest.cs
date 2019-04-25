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

namespace Core.Common.Geometry.Test
{
    [TestFixture]
    public class Point2DTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var point = new Point2D();

            // Assert
            Assert.Zero(point.X);
            Assert.Zero(point.Y);
        }

        [Test]
        public void Constructor_WithConcreteValues_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            double x = random.NextDouble();
            double y = random.NextDouble();

            // Call
            var point = new Point2D(x, y);

            // Assert
            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Constructor_WithDiscreteValues_ExpectedValues(double coordinate)
        {
            // Call
            var point = new Point2D(coordinate, coordinate);

            // Assert
            Assert.AreEqual(coordinate, point.X);
            Assert.AreEqual(coordinate, point.Y);
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            // Setup
            var random = new Random(21);
            var point = new Point2D(random.NextDouble(), random.NextDouble());

            // Call 
            bool result = point.Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_DifferentObject_ReturnsFalse()
        {
            // Setup
            var random = new Random(21);
            var point = new Point2D(random.NextDouble(), random.NextDouble());

            // Call 
            bool result = point.Equals(new object());

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_HavingSameReference_ReturnsTrue()
        {
            // Setup
            var random = new Random(21);
            var point1 = new Point2D(random.NextDouble(), random.NextDouble());
            Point2D point2 = point1;

            // Call 
            bool result12 = point1.Equals(point2);
            bool result21 = point2.Equals(point1);

            // Assert
            Assert.IsTrue(result12);
            Assert.IsTrue(result21);
        }

        [Test]
        public void Equals_HavingSameValues_ReturnsTrue()
        {
            // Setup
            var random = new Random(21);
            double xCoordinate = random.NextDouble();
            double yCoordinate = random.NextDouble();

            var point1 = new Point2D(xCoordinate, yCoordinate);
            var point2 = new Point2D(xCoordinate, yCoordinate);

            // Call
            bool result12 = point1.Equals(point2);
            bool result21 = point2.Equals(point1);

            // Assert
            Assert.IsTrue(result12);
            Assert.IsTrue(result21);
        }

        [Test]
        [TestCase(0, 10)]
        [TestCase(10, 0)]
        public void Equals_HavingDifferentValues_ReturnsFalse(double xOffset, double yOffset)
        {
            // Setup
            var random = new Random(21);
            double xCoordinate = random.NextDouble();
            double yCoordinate = random.NextDouble();

            var point1 = new Point2D(xCoordinate, yCoordinate);
            var point2 = new Point2D(xCoordinate + xOffset, yCoordinate + yOffset);

            // Call
            bool result12 = point1.Equals(point2);
            bool result21 = point2.Equals(point1);

            // Assert
            Assert.IsFalse(result12);
            Assert.IsFalse(result21);
        }

        [Test]
        public void GetHashCode_EqualPoints_ReturnsSameHashCode()
        {
            // Setup
            var random = new Random(21);
            double xCoordinate = random.NextDouble();
            double yCoordinate = random.NextDouble();

            var point1 = new Point2D(xCoordinate, yCoordinate);
            var point2 = new Point2D(xCoordinate, yCoordinate);

            // Call 
            int hashCodePoint1 = point1.GetHashCode();
            int hashCodePoint2 = point2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodePoint1, hashCodePoint2);
        }
    }
}