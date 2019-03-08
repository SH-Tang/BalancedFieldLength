using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Geometry {
    /// <summary>
    /// Helper class which defines geometric methods for operations in the 2D plane.
    /// </summary>
    public static class Geometry2DHelper
    {
        private const double tolerance = 1e-6;

        /// <summary>
        /// Determines whether two line segments intersect.
        /// </summary>
        /// <param name="startPoint1"></param>
        /// <param name="endPoint1"></param>
        /// <param name="startPoint2"></param>
        /// <param name="endPoint2"></param>
        /// <returns>A <see cref="Point2D"/> with <see cref="double.NaN"/> coordinates when:
        /// <list type="bullet">
        /// <item>The lines are parallel.</item>
        /// <item>The lines are on top of each other.</item>
        /// </list></returns>
        /// <remarks>Algorithm is based on: https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection </remarks>
        public static Point2D DetermineLineIntersection(Point2D startPoint1, Point2D endPoint1, Point2D startPoint2, Point2D endPoint2)
        {
            Point2D[] points =
            {
                startPoint1,
                endPoint1,
                startPoint2,
                endPoint2
            };

            double determinant = (startPoint1.X - endPoint1.X) * (startPoint2.Y - endPoint2.Y)
                                 - (startPoint1.Y - endPoint1.Y) * (startPoint2.X - endPoint2.X);

            // If the determinant equal 0, the lines are either parallel or on top of each other.
            if (Math.Abs(determinant) > tolerance)
            {
                double xCoordinate = (startPoint1.X * endPoint1.Y - startPoint1.Y * endPoint1.X) * (startPoint2.X - endPoint2.X) -
                                     (startPoint1.X - endPoint1.X) * (startPoint2.X * endPoint2.Y - startPoint2.Y * endPoint2.X);

                double yCoordinate = (startPoint1.X * endPoint1.Y - startPoint1.Y * endPoint1.X) * (startPoint2.Y - endPoint2.Y) -
                                     (startPoint1.Y - endPoint1.Y) * (startPoint2.X * endPoint2.Y - startPoint2.Y * endPoint2.X);

                // Check interval. If the new coordinates are outside the defined start and end points, then it is not an intersection.
                double xMaxInterval = GetMaximumValue(points.Select(p => p.X));
                double xMinInterval = GetMinimumValue(points.Select(p => p.X));

                double yMaxInterval = GetMaximumValue(points.Select(p => p.Y));
                double yMinInterval = GetMinimumValue(points.Select(p => p.Y));

                double intersectionXCoordinate = xCoordinate / determinant;
                double intersectionYCoordinate = yCoordinate / determinant;

                if (intersectionXCoordinate < xMaxInterval
                    && intersectionXCoordinate > xMinInterval
                    && intersectionYCoordinate < yMaxInterval
                    && intersectionYCoordinate > yMinInterval)
                {
                    return new Point2D(intersectionXCoordinate, intersectionYCoordinate);
                }
            }

            return new Point2D(double.NaN, double.NaN);
        }

        private static double GetMinimumValue(IEnumerable<double> numbers)
        {
            return numbers.Min();
        }

        private static double GetMaximumValue(IEnumerable<double> numbers)
        {
            return numbers.Max();
        }
    }
}