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

namespace Simulator.Calculator.AggregatedDistanceCalculator
{
    /// <summary>
    /// Class to hold the aggregated output of the calculated distances of a continued
    /// and a rejected take off for a given failure speed.
    /// </summary>
    public class AggregatedDistanceOutput
    {
        /// <summary>
        /// Creates a new instance of <see cref="AggregatedDistanceOutput"/>.
        /// </summary>
        /// <param name="failureSpeed">The failure speed for which the output was calculated. [m/s]</param>
        /// <param name="abortedTakeOffDistance">The distance belonging to an aborted take off. [m]</param>
        /// <param name="continuedTakeOffDistance">The distance belonging to a continued take off. [m]</param>
        public AggregatedDistanceOutput(double failureSpeed, double abortedTakeOffDistance, double continuedTakeOffDistance)
        {
            FailureSpeed = failureSpeed;
            AbortedTakeOffDistance = abortedTakeOffDistance;
            ContinuedTakeOffDistance = continuedTakeOffDistance;
        }

        /// <summary>
        /// Gets the failure speed. [m/s]
        /// </summary>
        public double FailureSpeed { get; }

        /// <summary>
        /// Gets the distance of an aborted take off. [m]
        /// </summary>
        public double AbortedTakeOffDistance { get; }

        /// <summary>
        /// Gets the distance of a continued take off. [m]
        /// </summary>
        public double ContinuedTakeOffDistance { get; }
    }
}