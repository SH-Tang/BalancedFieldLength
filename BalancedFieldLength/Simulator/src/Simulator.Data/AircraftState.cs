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

using Core.Common.Data;

namespace Simulator.Data
{
    /// <summary>
    /// Class which contains the aircraft states.
    /// </summary>
    public class AircraftState
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftState"/>
        /// with zero values for the states.
        /// </summary>
        public AircraftState() {}

        /// <summary>
        /// Creates a new instance of <see cref="AircraftState"/>.
        /// </summary>
        /// <param name="pitchAngle">The pitch angle.</param>
        /// <param name="flightPathAngle">The flight path angle.</param>
        /// <param name="trueAirspeed">The true airspeed. [m/s]</param>
        /// <param name="height">The height. [m]</param>
        /// <param name="distance">The horizontal distance from the starting point. [m]</param>
        public AircraftState(Angle pitchAngle, Angle flightPathAngle, double trueAirspeed, double height, double distance)
        {
            PitchAngle = pitchAngle;
            FlightPathAngle = flightPathAngle;
            TrueAirspeed = trueAirspeed;
            Height = height;
            Distance = distance;
        }

        /// <summary>
        /// Gets the pitch angle.
        /// </summary>
        /// <remarks>Also denoted as theta.</remarks>
        public Angle PitchAngle { get; }

        /// <summary>
        /// Gets the flight path angle.
        /// </summary>
        /// <remarks>Also denoted as gamma.</remarks>
        public Angle FlightPathAngle { get; }

        /// <summary>
        /// Gets the true airspeed. [m/s]
        /// </summary>
        /// <remarks>Also denoted as Vtas.</remarks>
        public double TrueAirspeed { get; }

        /// <summary>
        /// Gets the height. [m]
        /// </summary>
        /// <remarks>Also denoted as h.</remarks>
        public double Height { get; }

        /// <summary>
        /// Gets the horizontal distance. [m]
        /// </summary>
        /// <remarks>Also denoted as x or d.</remarks>
        public double Distance { get; }
    }
}