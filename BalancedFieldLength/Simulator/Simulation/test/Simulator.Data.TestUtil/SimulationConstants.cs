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

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Class which contains constants which can be used for testing.
    /// </summary>
    public static class SimulationConstants
    {
        /// <summary>
        /// Represents the tolerance of double deviations due to rounding.
        /// </summary>
        public const double Tolerance = 10e-6;

        /// <summary>
        /// Represents the gravitational acceleration. m/s^2
        /// </summary>
        public const double GravitationalAcceleration = 9.81;

        /// <summary>
        /// Represents the air density. [kg/m^3] 
        /// </summary>
        public const double Density = 1.225;
    }
}