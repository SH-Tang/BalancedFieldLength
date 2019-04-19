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

namespace Simulator.Calculator
{
    /// <summary>
    /// Class to hold the calculated distance for a given failure speed.
    /// </summary>
    public class DistanceCalculatorOutput
    {
        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculatorOutput"/>.
        /// </summary>
        /// <param name="failureSpeed">The speed at which the failure occurred. [m/s]</param>
        /// <param name="distance">The distance covered before the stopping criteria was satisfied. [m]</param>
        internal DistanceCalculatorOutput(int failureSpeed, double distance)
        {
            FailureSpeed = failureSpeed;
            Distance = distance;
        }

        /// <summary>
        /// Gets the speed at which the failure occurred. [m/s]
        /// </summary>
        public int FailureSpeed { get; }

        /// <summary>
        /// Gets the distance that was covered. [m]
        /// </summary>
        public double Distance { get; }
    }
}