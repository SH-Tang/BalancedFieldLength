using System;

namespace Core.Common.Data
{
    /// <summary>
    /// Represents an angle between 0 and 360 degrees.
    /// </summary>
    public struct Angle
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Angle"/>
        /// with a given angle.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when
        /// <paramref name="angle"/> is not between [0, 360] degrees.</exception>
        public Angle(double angle)
        {
            if (angle < 0 || angle > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(angle),
                                                      "Invalid angle, angle must be in the range of [0, 360]");
            }

            Degrees = angle;
            Radians = (Degrees * Math.PI) / 180;
        }

        public double Degrees { get; }
        public double Radians { get; }
    }
}