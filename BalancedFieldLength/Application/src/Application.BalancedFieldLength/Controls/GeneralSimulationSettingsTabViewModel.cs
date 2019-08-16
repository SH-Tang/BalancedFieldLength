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

        /// <summary>
        /// Creates a new instance of <see cref="GeneralSimulationSettingsTabViewModel"/>.
        /// </summary>
        public GeneralSimulationSettingsTabViewModel()
        {
            TimeStep = double.NaN;
        }

        /// <summary>
        /// Gets or sets the maximum number of iterations each simulation may run.
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
        /// Gets or sets the time past during each iteration.
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
        /// Gets or sets the end failure velocity for which the simulation should stop.
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

        public string TabName
        {
            get
            {
                return "Simulation";
            }
        }
    }
}