namespace Core.Common.Geometry
{
    /// <summary>
    /// Class which represents a coordinate in the 2D Euclidean plane.
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && Equals((Point2D) obj);
        }

        public bool Equals(Point2D other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }
}