using System;
using Core.Common.Data;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Test helper which can be used for testing <see cref="AerodynamicsData"/>.
    /// </summary>
    public static class AerodynamicsDataTestHelper
    {
        /// <summary>
        /// Gets a valid angle of attack (alpha) based on <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The <see cref="AerodynamicsData"/> to generate a
        /// valid angle of attack for.</param>
        /// <returns>A valid angle of attack.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <remarks>As the angle of attack is defined as:
        /// <code>
        /// Flight Path Angle + Angle Of Attack = Pitch Angle
        /// </code>
        /// By setting the pitch angle equal to the angle of attack and
        /// flight path angle to zero, the desired result can be obtained.
        /// </remarks>
        public static Angle GetValidAngleOfAttack(AerodynamicsData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data.ZeroLiftAngleOfAttack + GetMidPoint(data.MaximumLiftCoefficient, data.LiftCoefficientGradient);
        }

        private static Angle GetMidPoint(double maximumLiftCoefficient, double liftGradient)
        {
            double midPointAngle = maximumLiftCoefficient / (2 * liftGradient);
            return Angle.FromRadians(midPointAngle);
        }
    }
}