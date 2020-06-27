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
using System.Windows.Input;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using Application.BalancedFieldLength.Views.OutputView;
using Application.BalancedFieldLength.Views.TabViews;
using WPF.Components.MessageView;
using WPF.Components.TabControl;
using WPF.Core;

namespace Application.BalancedFieldLength
{
    /// <summary>
    /// The view model of the application.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private OutputViewModel outputViewModel;
        private readonly BalancedFieldLengthCalculation calculation;

        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel()
        {
            calculation = new BalancedFieldLengthCalculation();

            var tabControlViewModel = new TabControlViewModel();
            var generalSettingsTab = new GeneralSimulationSettingsTabViewModel(calculation.SimulationSettings);
            tabControlViewModel.Tabs.Add(generalSettingsTab);
            tabControlViewModel.Tabs.Add(new EngineSettingsTabViewModel(calculation.EngineData));
            tabControlViewModel.Tabs.Add(new AircraftDataTabViewModel(calculation.AircraftData));
            tabControlViewModel.SelectedTabItem = generalSettingsTab;

            TabControlViewModel = tabControlViewModel;

            MessageWindowViewModel = new MessageWindowViewModel();
            OutputViewModel = null;

            CalculateCommand = new RelayCommand(Calculate);
        }

        public TabControlViewModel TabControlViewModel { get; }

        public OutputViewModel OutputViewModel
        {
            get
            {
                return outputViewModel;

            }
            private set
            {
                outputViewModel = value;
                OnPropertyChanged(nameof(OutputViewModel));
            }
        }

        public MessageWindowViewModel MessageWindowViewModel { get; }

        public ICommand CalculateCommand { get; }

        private void Calculate()
        {
            try
            {
                IBalancedFieldLengthCalculationModuleFactory factory = BalancedFieldLengthCalculationModuleFactory.Instance;
                IBalancedFieldLengthCalculationModule calculationModule = factory.CreateModule();
                BalancedFieldLengthOutput output = calculationModule.Calculate(calculation);

                OutputViewModel = new OutputViewModel(output);
            }
            catch (Exception e) when (e is CreateKernelDataException || e is KernelCalculationException)
            {
                MessageWindowViewModel.AddMessage(new MessageContext(MessageType.Error, e.Message));
            }
        }
    }
}