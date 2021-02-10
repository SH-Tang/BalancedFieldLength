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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper;
using Application.BalancedFieldLength.KernelWrapper.TestUtils;
using Application.BalancedFieldLength.Views.OutputView;
using Application.BalancedFieldLength.Views.TabViews;
using Core.Common.TestUtil;
using NUnit.Framework;
using WPF.Components.MessageView;
using WPF.Components.TabControl;

namespace Application.BalancedFieldLength.Test
{
    [TestFixture]
    public class MainViewModelIntegrationTest
    {
        [Test]
        public void GivenViewModelWithInvalidCalculation_WhenCalculationPressed_ThenErrorLoggedAndInputNotSent()
        {
            // Given
            var random = new Random(21);

            var viewModel = new MainViewModel();
            TabControlViewModel tabControlViewModel = viewModel.TabControlViewModel;
            ObservableCollection<ITabViewModel> tabs = tabControlViewModel.Tabs;

            // Precondition
            Assert.That(tabs, Has.Count.EqualTo(3));
            Assert.That(tabs[0], Is.TypeOf<GeneralSimulationSettingsTabViewModel>());
            Assert.That(tabs[1], Is.TypeOf<EngineSettingsTabViewModel>());
            Assert.That(tabs[2], Is.TypeOf<AircraftDataTabViewModel>());

            var generalSimulationSettingsViewModel = (GeneralSimulationSettingsTabViewModel) tabs[0];
            var engineSettingsViewModel = (EngineSettingsTabViewModel) tabs[1];
            var aircraftDataViewModel = (AircraftDataTabViewModel) tabs[2];
            SetGeneralSettings(generalSimulationSettingsViewModel, random.Next());
            SetEngineData(engineSettingsViewModel, random.Next());
            SetAircraftData(aircraftDataViewModel, random.Next());

            aircraftDataViewModel.TakeOffWeight = double.NaN; // Set an invalid value

            using (new BalancedFieldLengthCalculationModuleFactoryConfig())
            {
                var instance = (TestBalancedFieldLengthCalculationModuleFactory)
                    BalancedFieldLengthCalculationModuleFactory.Instance;

                TestBalancedFieldLengthCalculationModule testModule = instance.TestModule;

                // Precondition
                MessageWindowViewModel messageWindowViewModel = viewModel.MessageWindowViewModel;
                Assert.That(messageWindowViewModel.Messages, Is.Empty);

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                Assert.That(testModule.InputCalculation, Is.Null);

                OutputViewModel outputViewModel = viewModel.OutputViewModel;
                Assert.That(outputViewModel, Is.Null);

                IEnumerable<MessageType> messageTypes = messageWindowViewModel.Messages.Select(m => m.MessageType);
                Assert.That(messageTypes, Is.All.EqualTo(MessageType.Error));

                MessageContext firstMessage = messageWindowViewModel.Messages.First();
                Assert.That(firstMessage.Message, Is.EqualTo("Calculation failed."));
            }
        }

        [Test]
        public void GivenViewModel_WhenCalculationPressed_ThenInputArgumentSent()
        {
            // Given
            var random = new Random(21);

            var viewModel = new MainViewModel();
            TabControlViewModel tabControlViewModel = viewModel.TabControlViewModel;
            ObservableCollection<ITabViewModel> tabs = tabControlViewModel.Tabs;

            // Precondition
            Assert.That(tabs, Has.Count.EqualTo(3));
            Assert.That(tabs[0], Is.TypeOf<GeneralSimulationSettingsTabViewModel>());
            Assert.That(tabs[1], Is.TypeOf<EngineSettingsTabViewModel>());
            Assert.That(tabs[2], Is.TypeOf<AircraftDataTabViewModel>());

            var generalSimulationSettingsViewModel = (GeneralSimulationSettingsTabViewModel) tabs[0];
            var engineSettingsViewModel = (EngineSettingsTabViewModel) tabs[1];
            var aircraftDataViewModel = (AircraftDataTabViewModel) tabs[2];

            SetGeneralSettings(generalSimulationSettingsViewModel, random.Next());
            SetEngineData(engineSettingsViewModel, random.Next());
            SetAircraftData(aircraftDataViewModel, random.Next());

            using (new BalancedFieldLengthCalculationModuleFactoryConfig())
            {
                var instance = (TestBalancedFieldLengthCalculationModuleFactory)
                    BalancedFieldLengthCalculationModuleFactory.Instance;

                TestBalancedFieldLengthCalculationModule testModule = instance.TestModule;
                testModule.Output = new BalancedFieldLengthOutput(random.NextDouble(), random.NextDouble());

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                BalancedFieldLengthCalculation calculationInput = testModule.InputCalculation;
                AssertGeneralSettings(generalSimulationSettingsViewModel, calculationInput.SimulationSettings);
                AssertEngineData(engineSettingsViewModel, calculationInput.EngineData);
                AssertAircraftData(aircraftDataViewModel, calculationInput.AircraftData);
            }
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

                // Precondition
                MessageWindowViewModel messageWindowViewModel = viewModel.MessageWindowViewModel;
                Assert.That(messageWindowViewModel.Messages, Is.Empty);

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                OutputViewModel outputViewModel = viewModel.OutputViewModel;
                Assert.That(outputViewModel, Is.Not.Null);
                Assert.That(outputViewModel.BalancedFieldLengthDistance, Is.EqualTo(calculatedOutput.Distance));
                Assert.That(outputViewModel.BalancedFieldLengthVelocity, Is.EqualTo(calculatedOutput.Velocity));

                Assert.That(messageWindowViewModel.Messages, Has.Count.EqualTo(1));
                MessageContext message = messageWindowViewModel.Messages.Single();
                Assert.That(message.MessageType, Is.EqualTo(MessageType.Info));
                Assert.That(message.Message, Is.EqualTo("Calculation completed."));
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
                ReadOnlyObservableCollection<MessageContext> logMessages = messageWindowViewModel.Messages;
                Assert.That(logMessages, Is.Empty);

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                Assert.That(viewModel.OutputViewModel, Is.Null);

                Assert.That(logMessages, Has.Count.EqualTo(2));
                Assert.That(logMessages.Select(m => m.MessageType), Is.All.EqualTo(MessageType.Error));

                Assert.That(logMessages[0].MessageType, Is.EqualTo(MessageType.Error));
                Assert.That(logMessages[0].Message, Is.EqualTo("Calculation failed."));

                Assert.That(logMessages[1].MessageType, Is.EqualTo(MessageType.Error));
                Assert.That(logMessages[1].Message, Is.EqualTo("Exception"));
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
                ReadOnlyObservableCollection<MessageContext> logMessages = messageWindowViewModel.Messages;
                Assert.That(logMessages, Is.Empty);

                // When
                viewModel.CalculateCommand.Execute(null);

                // Then
                Assert.That(viewModel.OutputViewModel, Is.Null);

                Assert.That(logMessages, Has.Count.EqualTo(2));
                Assert.That(logMessages.Select(m => m.MessageType), Is.All.EqualTo(MessageType.Error));

                Assert.That(logMessages[0].MessageType, Is.EqualTo(MessageType.Error));
                Assert.That(logMessages[0].Message, Is.EqualTo("Calculation failed."));

                Assert.That(logMessages[1].MessageType, Is.EqualTo(MessageType.Error));
                Assert.That(logMessages[1].Message, Is.EqualTo("Exception"));
            }
        }

        private static void SetGeneralSettings(GeneralSimulationSettingsTabViewModel viewModel, int seed)
        {
            var random = new Random(seed);
            viewModel.MaximumNrOfIterations = random.Next();
            viewModel.TimeStep = random.NextDouble();
            viewModel.EndFailureVelocity = random.Next();
            viewModel.GravitationalAcceleration = random.NextDouble();
            viewModel.Density = random.NextDouble();
        }

        private static void SetEngineData(EngineSettingsTabViewModel viewModel, int seed)
        {
            var random = new Random(seed);
            viewModel.ThrustPerEngine = random.Next();
            viewModel.NrOfEngines = random.Next();
            viewModel.NrOfFailedEngines = viewModel.NrOfEngines - 1;

            // Precondition
            var defaultSettings = new EngineData();
            Assert.That(viewModel.NrOfEngines, Is.Not.EqualTo(defaultSettings.NrOfEngines));
            Assert.That(viewModel.NrOfFailedEngines, Is.Not.EqualTo(defaultSettings.NrOfFailedEngines));
        }

        private static void SetAircraftData(AircraftDataTabViewModel viewModel, int seed)
        {
            var random = new Random(seed);

            viewModel.TakeOffWeight = random.NextDouble();
            viewModel.PitchGradient = random.NextAngle();
            viewModel.MaximumPitchAngle = random.NextAngle();

            viewModel.WingSurfaceArea = random.NextDouble();
            viewModel.AspectRatio = random.NextDouble();
            viewModel.OswaldFactor = random.NextDouble();

            viewModel.MaximumLiftCoefficient = random.NextDouble();
            viewModel.LiftCoefficientGradient = random.NextDouble();
            viewModel.ZeroLiftAngleOfAttack = random.NextAngle();

            viewModel.RestDragCoefficient = random.NextDouble();
            viewModel.RestDragCoefficientWithEngineFailure = random.NextDouble() + viewModel.RestDragCoefficient;

            viewModel.RollResistanceCoefficient = random.NextDouble();
            viewModel.RollResistanceWithBrakesCoefficient = random.NextDouble() + viewModel.RollResistanceCoefficient;
        }

        private static void AssertGeneralSettings(GeneralSimulationSettingsTabViewModel expected,
                                                  GeneralSimulationSettingsData actual)
        {
            Assert.That(actual.MaximumNrOfIterations, Is.EqualTo(expected.MaximumNrOfIterations));
            Assert.That(actual.TimeStep, Is.EqualTo(expected.TimeStep));
            Assert.That(actual.EndFailureVelocity, Is.EqualTo(expected.EndFailureVelocity));
            Assert.That(actual.GravitationalAcceleration, Is.EqualTo(expected.GravitationalAcceleration));
            Assert.That(actual.Density, Is.EqualTo(expected.Density));
        }

        private static void AssertEngineData(EngineSettingsTabViewModel expected,
                                             EngineData actual)
        {
            Assert.That(actual.ThrustPerEngine, Is.EqualTo(expected.ThrustPerEngine));
            Assert.That(actual.NrOfEngines, Is.EqualTo(expected.NrOfEngines));
            Assert.That(actual.NrOfFailedEngines, Is.EqualTo(expected.NrOfFailedEngines));
        }

        private static void AssertAircraftData(AircraftDataTabViewModel expected,
                                               AircraftData actual)
        {
            Assert.That(actual.TakeOffWeight, Is.EqualTo(expected.TakeOffWeight));
            Assert.That(actual.PitchGradient, Is.EqualTo(expected.PitchGradient));
            Assert.That(actual.MaximumPitchAngle, Is.EqualTo(expected.MaximumPitchAngle));

            Assert.That(actual.WingSurfaceArea, Is.EqualTo(expected.WingSurfaceArea));
            Assert.That(actual.AspectRatio, Is.EqualTo(expected.AspectRatio));
            Assert.That(actual.OswaldFactor, Is.EqualTo(expected.OswaldFactor));

            Assert.That(actual.MaximumLiftCoefficient, Is.EqualTo(expected.MaximumLiftCoefficient));
            Assert.That(actual.LiftCoefficientGradient, Is.EqualTo(expected.LiftCoefficientGradient));
            Assert.That(actual.ZeroLiftAngleOfAttack, Is.EqualTo(expected.ZeroLiftAngleOfAttack));

            Assert.That(actual.RestDragCoefficientWithEngineFailure, Is.EqualTo(expected.RestDragCoefficientWithEngineFailure));
            Assert.That(actual.RestDragCoefficient, Is.EqualTo(expected.RestDragCoefficient));

            Assert.That(actual.RollResistanceCoefficient, Is.EqualTo(expected.RollResistanceCoefficient));
            Assert.That(actual.RollResistanceWithBrakesCoefficient, Is.EqualTo(expected.RollResistanceWithBrakesCoefficient));
        }
    }
}