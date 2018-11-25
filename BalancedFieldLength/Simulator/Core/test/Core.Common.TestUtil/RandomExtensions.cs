using System;
using Core.Common.Data;

namespace Core.Common.TestUtil
{
    /// <summary>
    /// Extension methods for <see cref="Random"/>.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Creates an <see cref="Angle"/> with random values.
        /// </summary>
        /// <param name="random">A pseudo-random generator.</param>
        /// <returns>An <see cref="Angle"/> with random values.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="random"/>
        /// is <c>null</c>.</exception>
        public static Angle NextAngle(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            return Angle.FromDegrees(random.NextDouble());
        }
    }
}