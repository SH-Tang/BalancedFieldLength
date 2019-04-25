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
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Geometry.Test
{
    [TestFixture]
    public class LineSegmentTest
    {
        [Test]
        public void Constructor_IdenticalPoints_ThrowsArgumentException()
        {
            // Setup
            var random = new Random(21);
            double xCoordinate = random.NextDouble();
            double yCoordinate = random.NextDouble();
            var startPoint = new Point2D(xCoordinate, yCoordinate);
            var endPoint = new Point2D(xCoordinate, yCoordinate);

            // Call 
            TestDelegate call = () => new LineSegment(startPoint, endPoint);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "A line must consist of two distinct points.");
        }

        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            var startPoint = new Point2D(random.NextDouble(), random.NextDouble());
            var endPoint = new Point2D(random.NextDouble(), random.NextDouble());

            // Call
            var line = new LineSegment(startPoint, endPoint);

            // Assert
            Assert.AreEqual(startPoint, line.StartPoint);
            Assert.AreEqual(endPoint, line.EndPoint);
        }
    }
}