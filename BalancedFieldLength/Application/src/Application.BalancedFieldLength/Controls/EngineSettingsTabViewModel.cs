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
using Application.BalancedFieldLength.WPFCommon;

namespace Application.BalancedFieldLength.Controls
{
    /// <summary>
    /// View model to display engine settings in a tab.
    /// </summary>
    public class EngineSettingsTabViewModel : ViewModelBase, ITabViewModel
    {
        private double thrustPerEngine;
        private int nrOfEngines;

        /// <summary>
        /// Creates a new instance of <see cref="EngineSettingsTabViewModel"/>.
        /// </summary>
        public EngineSettingsTabViewModel()
        {
            thrustPerEngine = double.NaN;
            TotalThrust = double.NaN;
        }

        /// <summary>
        /// Gets or sets the thrust per engine. [kN]
        /// </summary>
        public double ThrustPerEngine
        {
            get
            {
                return thrustPerEngine;
            }
            set
            {
                if (double.IsNaN(thrustPerEngine) ||
                    double.IsNaN(value) ||
                    Math.Abs(value - thrustPerEngine) > 1e-5)
                {
                    thrustPerEngine = value;
                    TotalThrust = value * NrOfEngines;

                    OnPropertyChanged(nameof(ThrustPerEngine));
                    OnPropertyChanged(nameof(TotalThrust));
                }
            }
        }

        /// <summary>
        /// Gets the total thrust. [kN]
        /// </summary>
        public double TotalThrust { get; private set; }

        /// <summary>
        /// Gets or sets the number of engines.
        /// </summary>
        public int NrOfEngines
        {
            get
            {
                return nrOfEngines;
            }
            set
            {
                if (value != nrOfEngines)
                {
                    nrOfEngines = value;

                    TotalThrust = thrustPerEngine * value;

                    OnPropertyChanged(nameof(NrOfEngines));
                    OnPropertyChanged(nameof(TotalThrust));
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of failed engines.
        /// </summary>
        public int NrOfFailedEngines { get; set; }

        public string TabName
        {
            get
            {
                return "Engine data";
            }
        }
    }
}