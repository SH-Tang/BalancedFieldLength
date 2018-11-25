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
        /// <param name="radians">The angle in radians.</param>
        private Angle(double radians)
        {
            Degrees = RadiansToDegrees(radians);
            Radians = radians;
        }

        /// <summary>
        /// Gets the angle in degrees.
        /// </summary>
        public double Degrees { get; }

        /// <summary>
        /// Gets the angle in radians.
        /// </summary>
        public double Radians { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Angle"/>
        /// based on <paramref name="degrees"/>.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>A <see cref="Angle"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when
        /// <paramref name="degrees"/> is not between [0, 360] degrees.</exception>
        public static Angle FromDegrees(double degrees)
        {
            if (degrees < 0 || degrees > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(degrees),
                                                      "Invalid angle, angle must be in the range of [0, 360] degrees");
            }

            return new Angle(DegreesToRadians(degrees));
        }

        /// <summary>
        /// Creates a new instance of <see cref="Angle"/>
        /// based on <paramref name="radians"/>.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>A <see cref="Angle"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when
        /// <paramref name="radians"/> is not between [0, 2 PI] radians.</exception>
        public static Angle FromRadians(double radians)
        {
            if (radians < 0 || radians > 2 * Math.PI)
            {
                throw new ArgumentOutOfRangeException(nameof(radians),
                                                      "Invalid angle, angle must be in the range of [0, 2 PI] radians");
            }

            return new Angle(radians);
        }

        private static double RadiansToDegrees(double radians)
        {
            return (radians * 180) / Math.PI;
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }
    }
}