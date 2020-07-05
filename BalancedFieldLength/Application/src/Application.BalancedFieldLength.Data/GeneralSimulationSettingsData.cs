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

namespace Application.BalancedFieldLength.Data
{
    /// <summary>
    /// Class to hold general simulation settings.
    /// </summary>
    public class GeneralSimulationSettingsData
    {
        public GeneralSimulationSettingsData()
        {
            GravitationalAcceleration = 9.81;
            Density = 1.225;

            TimeStep = double.NaN;
        }

        /// <summary>
        /// Gets or sets the maximum number of iterations each simulation may run. [-]
        /// </summary>
        public int MaximumNrOfIterations { get; set; }

        /// <summary>
        /// Gets or sets the time past during each iteration. [s]
        /// </summary>
        public double TimeStep { get; set; }

        /// <summary>
        /// Gets or sets the end failure velocity for which the simulation should stop. [m/s]
        /// </summary>
        public int EndFailureVelocity { get; set; }

        /// <summary>
        /// Gets or sets the gravitational acceleration for the simulation. [m/s^2]
        /// </summary>
        public double GravitationalAcceleration { get; set; }

        /// <summary>
        /// Gets or sets the density for the simulation. [kg/m^3]
        /// </summary>
        public double Density { get; set; }
    }
}