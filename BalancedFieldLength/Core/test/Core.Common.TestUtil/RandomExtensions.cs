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