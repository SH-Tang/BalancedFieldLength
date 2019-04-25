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
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Geometry
{
    /// <summary>
    /// Helper class which defines geometric methods for operations in the 2D plane.
    /// </summary>
    public static class Geometry2DHelper
    {
        private const double tolerance = 1e-6;

        /// <summary>
        /// Determines whether two line segments intersect.
        /// </summary>
        /// <param name="line1">The first line.</param>
        /// <param name="line2">The second line.</param>
        /// <returns>A <see cref="Point2D"/> with <see cref="double.NaN"/> coordinates when:
        /// <list type="bullet">
        /// <item>the lines are parallel,</item>
        /// <item>the lines are on top of each other,</item>
        /// <item>no crossing was found.</item>
        /// </list></returns>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
        /// <remarks>Algorithm is based on: https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection </remarks>
        public static Point2D DetermineLineIntersection(LineSegment line1, LineSegment line2)
        {
            if (line1 == null)
            {
                throw new ArgumentNullException(nameof(line1));
            }

            if (line2 == null)
            {
                throw new ArgumentNullException(nameof(line2));
            }

            Point2D[] points =
            {
                line1.StartPoint,
                line1.EndPoint,
                line2.StartPoint,
                line2.EndPoint
            };

            double determinant = (line1.StartPoint.X - line1.EndPoint.X) * (line2.StartPoint.Y - line2.EndPoint.Y)
                                 - (line1.StartPoint.Y - line1.EndPoint.Y) * (line2.StartPoint.X - line2.EndPoint.X);

            // If the determinant equals 0, the lines are either parallel or on top of each other.
            if (Math.Abs(determinant) > tolerance)
            {
                // Check if lines cross at the beginning or the end of the line segments
                if (line1.StartPoint.Equals(line2.StartPoint) || line1.StartPoint.Equals(line2.EndPoint))
                {
                    return line1.StartPoint;
                }
                if (line1.EndPoint.Equals(line2.StartPoint) || line1.EndPoint.Equals(line2.EndPoint))
                {
                    return line1.EndPoint;
                }

                double xCoordinate = (line1.StartPoint.X * line1.EndPoint.Y - line1.StartPoint.Y * line1.EndPoint.X) * (line2.StartPoint.X - line2.EndPoint.X) -
                                     (line1.StartPoint.X - line1.EndPoint.X) * (line2.StartPoint.X * line2.EndPoint.Y - line2.StartPoint.Y * line2.EndPoint.X);

                double yCoordinate = (line1.StartPoint.X * line1.EndPoint.Y - line1.StartPoint.Y * line1.EndPoint.X) * (line2.StartPoint.Y - line2.EndPoint.Y) -
                                     (line1.StartPoint.Y - line1.EndPoint.Y) * (line2.StartPoint.X * line2.EndPoint.Y - line2.StartPoint.Y * line2.EndPoint.X);

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