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

namespace Core.Common.Data
{
    /// <summary>
    /// Extension methods for <see cref="Angle"/>.
    /// </summary>
    public static class AngleExtensions
    {
        /// <summary>
        /// Gets an indicator whether the <see cref="Angle"/> represents a concrete value.
        /// (not <see cref="double.NaN"/>, <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.
        /// </summary>
        /// <param name="angle">The angle to determine whether it represents a concrete value.</param>
        /// <returns><c>true</c> if the <paramref name="angle"/> represents a concrete value, <c>false</c> otherwise.</returns>
        public static bool IsConcreteAngle(this Angle angle)
        {
            return !double.IsNaN(angle.Radians) && !double.IsInfinity(angle.Radians);
        }
    }
}