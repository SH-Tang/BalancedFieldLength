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
    /// Class which contains the time derivatives of the aircraft states.
    /// </summary>
    public class AircraftAccelerations
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftAccelerations"/>.
        /// </summary>
        /// <param name="pitchRate">The pitch rate. [1/s]</param>
        /// <param name="climbRate">The climb rate. [m/s]</param>
        /// <param name="trueAirSpeedRate">The true airspeed rate. [m/s^2]</param>
        /// <param name="flightPathRate">The flight path angle rate. [1/s]</param>
        public AircraftAccelerations(Angle pitchRate, double climbRate, double trueAirSpeedRate, Angle flightPathRate)
        {
            PitchRate = pitchRate;
            ClimbRate = climbRate;
            TrueAirSpeedRate = trueAirSpeedRate;
            FlightPathRate = flightPathRate;
        }

        /// <summary>
        /// Gets the pitch rate. [1/s]
        /// </summary>
        /// <remarks>Also denoted as dTheta/dt.</remarks>
        public Angle PitchRate { get; }

        /// <summary>
        /// Gets the climb rate. [m/s]
        /// </summary>
        /// <remarks>Also denoted as dh/dt.</remarks>
        public double ClimbRate { get; }

        /// <summary>
        /// Gets the true airspeed rate. [m/s^2]
        /// </summary>
        /// <remarks>Also denoted as dVtas/dt.</remarks>
        public double TrueAirSpeedRate { get; }

        /// <summary>
        /// Gets the flight path angle rate. [1/s]
        /// </summary>
        /// <remarks>Also  denoted as dGamma/dt.</remarks>
        public Angle FlightPathRate { get; }
    }
}