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

using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.Views.OutputView;
using Application.BalancedFieldLength.Views.TabViews;
using WPF.Components.MessageView;
using WPF.Components.TabControl;

namespace Application.BalancedFieldLength
{
    /// <summary>
    /// The view model of the application.
    /// </summary>
    public class MainViewModel
    {
        

        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel()
        {
            var calculation = new BalancedFieldLengthCalculation();
            
            var tabControlViewModel = new TabControlViewModel();
            var generalSettingsTab = new GeneralSimulationSettingsTabViewModel(calculation.SimulationSettings);
            tabControlViewModel.Tabs.Add(generalSettingsTab);
            tabControlViewModel.Tabs.Add(new EngineSettingsTabViewModel(calculation.EngineData));
            tabControlViewModel.Tabs.Add(new AircraftDataTabViewModel(calculation.AircraftData));
            tabControlViewModel.SelectedTabItem = generalSettingsTab;

            TabControlViewModel = tabControlViewModel;

            MessageWindowViewModel = new MessageWindowViewModel();
            MessageWindowViewModel.AddMessage(new MessageContext(MessageType.Info, "Hello World."));
            MessageWindowViewModel.AddMessage(new MessageContext(MessageType.Info, "Hello World1."));
            MessageWindowViewModel.AddMessage(new MessageContext(MessageType.Info, "Hello World2."));
            MessageWindowViewModel.AddMessage(new MessageContext(MessageType.Info, "Hello World3."));

            OutputViewModel = new OutputViewModel();
        }

        public TabControlViewModel TabControlViewModel { get; }

        public OutputViewModel OutputViewModel { get; }

        public MessageWindowViewModel MessageWindowViewModel { get; }
    }
}