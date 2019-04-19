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

namespace Core.Common.Geometry
{
    /// <summary>
    /// Class which represents a line segment in a 2D plane.
    /// </summary>
    public class LineSegment
    {
        /// <summary>
        /// Creates a new instance of <see cref="LineSegment"/>.
        /// </summary>
        /// <param name="startPoint">The start point of the segment.</param>
        /// <param name="endPoint">The end point of the segment.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="startPoint"/>
        /// and <paramref name="endPoint"/> are identical.</exception>
        public LineSegment(Point2D startPoint, Point2D endPoint)
        {
            if (startPoint.Equals(endPoint))
            {
                throw new ArgumentException("A line must consist of two distinct points.");
            }

            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        /// <summary>
        /// Gets the starting point of the line segment.
        /// </summary>
        public Point2D StartPoint { get; }

        /// <summary>
        /// Gets the end point of the line segment.
        /// </summary>
        public Point2D EndPoint { get; }
    }
}