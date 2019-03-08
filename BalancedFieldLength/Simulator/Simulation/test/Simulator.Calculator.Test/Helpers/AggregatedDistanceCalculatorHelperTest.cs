using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Simulator.Calculator.Test.Helpers
{
    [TestFixture]
    public class AggregatedDistanceCalculatorHelperTest
    {
        [Test]
        public void DetermineCrossing_OutputsNeverIntersect_ReturnsNaN()
        {
            // Setup
            var outputs = new[]
                          {
                              new AggregatedDistanceOutput(10, 10, 10),
                              new AggregatedDistanceOutput(11, 5, 15),
                              new AggregatedDistanceOutput(12, 0, 20)
                          };

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.IsNaN(output.FailureSpeed);
            Assert.IsNaN(output.AbortedTakeOffDistance);
            Assert.IsNaN(output.ContinuedTakeOffDistance);
        }

        [Test]
        public void DetermineCrossing_OutputsParallel_ReturnsNaN()
        {
            // Setup
            var outputs = new[]
                          {
                              new AggregatedDistanceOutput(10, 10, 10),
                              new AggregatedDistanceOutput(11, 10, 10),
                              new AggregatedDistanceOutput(12, 10, 10)
                          };

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.IsNaN(output.FailureSpeed);
            Assert.IsNaN(output.AbortedTakeOffDistance);
            Assert.IsNaN(output.ContinuedTakeOffDistance);
        }

        [Test]
        public void DetermineCrossing_OutputsIntersectWithAbortedDistanceIncreasingRejectedDistanceDecreasingAndPreciseIntersection_ReturnsExpectedIntersection()
        {
            // Setup
            var outputs = new[]
                          {
                              new AggregatedDistanceOutput(10, 10, 30),
                              new AggregatedDistanceOutput(11, 20, 20),
                              new AggregatedDistanceOutput(12, 30, 10)
                          };

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.AreEqual(11, output.FailureSpeed);
            Assert.AreEqual(20, output.AbortedTakeOffDistance);
            Assert.AreEqual(20, output.ContinuedTakeOffDistance);
        }
    }

    /// <summary>
    /// Helper class which contain functions determining the intersection between the
    /// distances covered of the rejected and the continued take off.
    /// </summary>
    public static class AggregatedDistanceCalculatorHelper
    {
        /// <summary>
        /// Determines the intersection between the distances covered by the rejected
        /// and the continued take off.
        /// </summary>
        /// <param name="outputs">The collection of <see cref="AggregatedDistanceOutput"/>
        /// to determine the intersection for.</param>
        /// <returns></returns>
        public static AggregatedDistanceOutput DetermineCrossing(IEnumerable<AggregatedDistanceOutput> outputs)
        {
            AggregatedDistanceOutput[] outputArray = outputs.ToArray();

            AggregatedDistanceOutput firstOutput = outputArray[0];
            for (int i = 1; i < outputArray.Length; i++)
            {
                AggregatedDistanceOutput currentOutput = outputArray[i];

                // Create line segments
                var continuedTakeOffSegment = new LineSegment(new Point2D(firstOutput.FailureSpeed, firstOutput.ContinuedTakeOffDistance),
                                                              new Point2D(currentOutput.FailureSpeed, currentOutput.ContinuedTakeOffDistance));

                var abortedTakeOffSegment = new LineSegment(new Point2D(firstOutput.FailureSpeed, firstOutput.AbortedTakeOffDistance),
                                                            new Point2D(currentOutput.FailureSpeed, currentOutput.AbortedTakeOffDistance));

                // Determine whether lines cross
                Point2D crossingPoint = GeometryHelper.DetermineLineIntersection(continuedTakeOffSegment.StartPoint, continuedTakeOffSegment.EndPoint,
                                                                                 abortedTakeOffSegment.StartPoint, abortedTakeOffSegment.EndPoint);

                // Determine if the result is valid
                if (!double.IsNaN(crossingPoint.X) && !double.IsNaN(crossingPoint.Y))
                {
                    return new AggregatedDistanceOutput(crossingPoint.X, crossingPoint.Y, crossingPoint.Y);
                }
            }

            return new AggregatedDistanceOutput(double.NaN, double.NaN, double.NaN);
        }

        private class LineSegment
        {
            public LineSegment(Point2D startPoint, Point2D endPoint)
            {
                StartPoint = startPoint;
                EndPoint = endPoint;
            }

            public Point2D StartPoint { get; }
            public Point2D EndPoint { get; }
        }
    }

    /// <summary>
    /// Class which represents a coordinate in the Euclidean plane.
    /// </summary>
    public struct Point2D
    {
        /// <summary>
        /// Creates a new instance of <see cref="Point2D"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public double Y { get; }
    }

    public static class GeometryHelper
    {
        private const double tolerance = 1e-6;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPoint1"></param>
        /// <param name="endPoint1"></param>
        /// <param name="startPoint2"></param>
        /// <param name="endPoint2"></param>
        /// <returns></returns>
        /// <remarks>Algorithm is based on: https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection </remarks>
        public static Point2D DetermineLineIntersection(Point2D startPoint1, Point2D endPoint1, Point2D startPoint2, Point2D endPoint2)
        {
            double determinant = (startPoint1.X - endPoint1.X) * (startPoint2.Y - endPoint2.Y)
                                 - (startPoint1.Y - endPoint1.Y) * (startPoint2.X - endPoint2.X);

            // If the determinant is larger than 0, that means there's an intersection.
            // Otherwise, the lines are either parallel or on top of each other.
            if (determinant > tolerance)
            {
                double xCoordinate = (startPoint1.X * endPoint1.Y - startPoint1.Y * endPoint1.X) * (startPoint2.X - endPoint2.X) -
                                     (startPoint1.X - endPoint1.X) * (startPoint2.X * endPoint2.Y - startPoint2.Y * endPoint2.X);

                double yCoordinate = (startPoint1.X * endPoint1.Y - startPoint1.Y * endPoint1.X) * (startPoint2.Y - endPoint2.Y) -
                                     (startPoint1.Y - endPoint1.Y) * (startPoint2.X * endPoint2.Y - startPoint2.Y * endPoint2.X);

                return new Point2D(xCoordinate / determinant, yCoordinate / determinant);
            }

            return new Point2D(double.NaN, double.NaN);
        }
    }
}