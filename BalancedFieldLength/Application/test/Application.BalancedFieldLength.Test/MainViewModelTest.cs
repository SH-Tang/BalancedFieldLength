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
using System.Collections.ObjectModel;
using System.Linq;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper;
using Application.BalancedFieldLength.KernelWrapper.TestUtils;
using Application.BalancedFieldLength.Views.OutputView;
using Application.BalancedFieldLength.Views.TabViews;
using NUnit.Framework;
using WPF.Components.MessageView;
using WPF.Components.TabControl;

namespace Application.BalancedFieldLength.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var mainViewModel = new MainViewModel();

            // Assert
            Assert.That(mainViewModel.OutputViewModel, Is.Null);
            Assert.That(mainViewModel.MessageWindowViewModel, Is.Not.Null);
            Assert.That(mainViewModel.TabControlViewModel, Is.Not.Null);
            Assert.That(mainViewModel.CalculateCommand, Is.Not.Null);

            TabControlViewModel tabControlViewModel = mainViewModel.TabControlViewModel;
            ObservableCollection<ITabViewModel> tabs = tabControlViewModel.Tabs;
            Assert.That(tabs, Has.Count.EqualTo(3));
            Assert.That(tabs[0], Is.TypeOf<GeneralSimulationSettingsTabViewModel>());
            Assert.That(tabs[1], Is.TypeOf<EngineSettingsTabViewModel>());
            Assert.That(tabs[2], Is.TypeOf<AircraftDataTabViewModel>());
            Assert.That(tabControlViewModel.SelectedTabItem, Is.SameAs(tabs[0]));
        }

        [Test]
        public void GivenViewModelAndCalculationSuccessful_WhenCalculating_ThenOutputGenerated()
        {
            // Given
            var random = new Random(21);
            var calculatedOutput = new BalancedFieldLengthOutput(random.NextDouble(), random.NextDouble());

            var viewModel = new MainViewModel();

            using (new BalancedFieldLengthCalculationModuleFactoryConfig())
            {
                var instance = (TestBalancedFieldLengthCalculationModuleFactory)
                    BalancedFieldLengthCalculationModuleFactory.Instance;
                
                TestBalancedFieldLengthCalculationModule testModule = instance.TestModule;
                testModule.Output = calculatedOutput;

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                OutputViewModel outputViewModel = viewModel.OutputViewModel;
                Assert.That(outputViewModel, Is.Not.Null);
                Assert.That(outputViewModel.BalancedFieldLengthDistance, Is.EqualTo(calculatedOutput.Distance));
                Assert.That(outputViewModel.BalancedFieldLengthVelocity, Is.EqualTo(calculatedOutput.Velocity));
            }
        }

        [Test]
        public void GivenViewModelAndCalculationThrowsCreateKernelDataException_WhenCalculating_ThenErrorMessagesLogged()
        {
            // Given
            var viewModel = new MainViewModel();

            using (new BalancedFieldLengthCalculationModuleFactoryConfig())
            {
                var instance = (TestBalancedFieldLengthCalculationModuleFactory)
                    BalancedFieldLengthCalculationModuleFactory.Instance;

                TestBalancedFieldLengthCalculationModule testModule = instance.TestModule;
                testModule.ThrowCreateKernelDataException = true;

                // Precondition
                MessageWindowViewModel messageWindowViewModel = viewModel.MessageWindowViewModel;
                Assert.That(messageWindowViewModel.Messages, Is.Empty);

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                Assert.That(viewModel.OutputViewModel, Is.Null);

                Assert.That(messageWindowViewModel.Messages, Has.Count.EqualTo(1));
                MessageContext message = messageWindowViewModel.Messages.Single();
                Assert.That(message.MessageType, Is.EqualTo(MessageType.Error));
                Assert.That(message.Message, Is.EqualTo("Exception"));
            }
        }

        [Test]
        public void GivenViewModelAndCalculationThrowsKernelCalculationException_WhenCalculating_ThenErrorMessagesLogged()
        {
            // Given
            var viewModel = new MainViewModel();

            using (new BalancedFieldLengthCalculationModuleFactoryConfig())
            {
                var instance = (TestBalancedFieldLengthCalculationModuleFactory)
                    BalancedFieldLengthCalculationModuleFactory.Instance;

                TestBalancedFieldLengthCalculationModule testModule = instance.TestModule;
                testModule.ThrowKernelCalculationException = true;

                // Precondition
                MessageWindowViewModel messageWindowViewModel = viewModel.MessageWindowViewModel;
                Assert.That(messageWindowViewModel.Messages, Is.Empty);

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                Assert.That(viewModel.OutputViewModel, Is.Null);

                Assert.That(messageWindowViewModel.Messages, Has.Count.EqualTo(1));
                MessageContext message = messageWindowViewModel.Messages.Single();
                Assert.That(message.MessageType, Is.EqualTo(MessageType.Error));
                Assert.That(message.Message, Is.EqualTo("Exception"));
            }
        }
    }
}