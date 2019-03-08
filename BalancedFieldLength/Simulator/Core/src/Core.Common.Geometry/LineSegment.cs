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