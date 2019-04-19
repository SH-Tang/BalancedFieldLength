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