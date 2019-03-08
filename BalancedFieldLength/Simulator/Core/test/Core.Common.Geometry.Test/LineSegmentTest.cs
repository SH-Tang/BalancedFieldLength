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