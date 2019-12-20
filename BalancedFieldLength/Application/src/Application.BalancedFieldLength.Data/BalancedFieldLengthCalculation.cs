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
    /// Class which holds data for a balanced field length calculation.
    /// </summary>
    public class BalancedFieldLengthCalculation
    {
        /// <summary>
        /// Creates a new instance of <see cref="BalancedFieldLengthCalculation"/>.
        /// </summary>
        public BalancedFieldLengthCalculation()
        {
            SimulationSettings = new GeneralSimulationSettingsData();
            AircraftData = new AircraftData();
            EngineData = new EngineData();
        }

        /// <summary>
        /// Gets the <see cref="Data.GeneralSimulationSettingsData"/>.
        /// </summary>
        public GeneralSimulationSettingsData SimulationSettings { get; }

        /// <summary>
        /// Gets the <see cref="Data.AircraftData"/>.
        /// </summary>
        public AircraftData AircraftData { get; }

        /// <summary>
        /// Gets the <see cref="Data.EngineData"/>.
        /// </summary>
        public EngineData EngineData { get; }
    }
}