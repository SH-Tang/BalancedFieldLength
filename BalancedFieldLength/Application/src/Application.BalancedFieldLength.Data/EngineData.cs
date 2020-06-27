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
    /// Class to hold the engine data.
    /// </summary>
    public class EngineData
    {
        /// <summary>
        /// Creates a new instance of <see cref="EngineData"/>.
        /// </summary>
        public EngineData()
        {
            ThrustPerEngine = double.NaN;
            NrOfEngines = 2;
            NrOfFailedEngines = 1;
        }

        /// <summary>
        /// Gets or sets the total number of engines.
        /// </summary>
        public int NrOfEngines { get; set; }

        /// <summary>
        /// Gets or sets the total number of failed engines.
        /// </summary>
        public int NrOfFailedEngines { get; set; }

        /// <summary>
        /// Gets or sets the thrust per engine. [kN]
        /// </summary>
        public double ThrustPerEngine { get; set; }
    }
}