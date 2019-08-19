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

using Application.BalancedFieldLength.WPFCommon;

namespace Application.BalancedFieldLength.Controls
{
    /// <summary>
    /// View model to display general simulation settings in a tab.
    /// </summary>
    public class GeneralSimulationSettingsTabViewModel : ViewModelBase, ITabViewModel
    {
        private int maximumNrOfIterations;
        private double timeStep;
        private int endFailureVelocity;
        private double gravitationalAcceleration;
        private double density;

        /// <summary>
        /// Creates a new instance of <see cref="GeneralSimulationSettingsTabViewModel"/>.
        /// </summary>
        public GeneralSimulationSettingsTabViewModel()
        {
            timeStep = double.NaN;
            gravitationalAcceleration = 9.81;
            density = 1.225;
        }

        /// <summary>
        /// Gets or sets the maximum number of iterations each simulation may run. [-]
        /// </summary>
        public int MaximumNrOfIterations
        {
            get
            {
                return maximumNrOfIterations;
            }
            set
            {
                maximumNrOfIterations = value;
                OnPropertyChanged(nameof(MaximumNrOfIterations));
            }
        }

        /// <summary>
        /// Gets or sets the time past during each iteration. [s]
        /// </summary>
        public double TimeStep
        {
            get
            {
                return timeStep;
            }
            set
            {
                timeStep = value;
                OnPropertyChanged(nameof(TimeStep));
            }
        }

        /// <summary>
        /// Gets or sets the end failure velocity for which the simulation should stop. [m/s]
        /// </summary>
        public int EndFailureVelocity
        {
            get
            {
                return endFailureVelocity;
            }
            set
            {
                endFailureVelocity = value;
                OnPropertyChanged(nameof(EndFailureVelocity));
            }
        }

        /// <summary>
        /// Gets or sets the gravitational acceleration for the simulation. [m/s^2]
        /// </summary>
        public double GravitationalAcceleration
        {
            get
            {
                return gravitationalAcceleration;
            }
            set
            {
                gravitationalAcceleration = value;
                OnPropertyChanged(nameof(GravitationalAcceleration));
            }
        }

        /// <summary>
        /// Gets or sets the density for the simulation. [kg/m^3]
        /// </summary>
        public double Density
        {
            get
            {
                return density;
            }
            set
            {
                density = value;
                OnPropertyChanged(nameof(Density));
            }
        }

        public string TabName
        {
            get
            {
                return "Simulation";
            }
        }
    }
}