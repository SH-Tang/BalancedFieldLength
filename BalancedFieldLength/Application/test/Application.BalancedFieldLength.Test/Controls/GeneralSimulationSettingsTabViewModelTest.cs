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
using System.ComponentModel;
using Application.BalancedFieldLength.Controls;
using Application.BalancedFieldLength.WPFCommon;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Controls
{
    [TestFixture]
    public class GeneralSimulationSettingsTabViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new GeneralSimulationSettingsTabViewModel();

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ITabViewModel>());
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel.TabName, Is.EqualTo("Simulation"));
            Assert.That(viewModel.MaximumNrOfIterations, Is.Zero);
            Assert.That(viewModel.TimeStep, Is.NaN);
            Assert.That(viewModel.EndFailureVelocity, Is.Zero);
            Assert.That(viewModel.GravitationalAcceleration, Is.EqualTo(9.81));
            Assert.That(viewModel.Density, Is.EqualTo(1.225));
        }

        [Test]
        [TestCaseSource(nameof(GetNotifyPropertyChangedTestCases))]
        public void Property_ValueChanges_RaisesNotifyPropertyChangedEvent(Action<GeneralSimulationSettingsTabViewModel> propertyChangeAction,
                                                                           string propertyName) 
        {
            // Setup
            var viewModel = new GeneralSimulationSettingsTabViewModel();

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            // Call 
            propertyChangeAction(viewModel);

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(propertyName));
        }

        private static IEnumerable<TestCaseData> GetNotifyPropertyChangedTestCases()
        {
            yield return new TestCaseData(new Action<GeneralSimulationSettingsTabViewModel>(vm => vm.MaximumNrOfIterations = 1),
                                          nameof(GeneralSimulationSettingsTabViewModel.MaximumNrOfIterations))
                .SetName("Maximum number of iterations");
            yield return new TestCaseData(new Action<GeneralSimulationSettingsTabViewModel>(vm => vm.TimeStep = 0.1),
                                          nameof(GeneralSimulationSettingsTabViewModel.TimeStep))
                .SetName("Time step");
            yield return new TestCaseData(new Action<GeneralSimulationSettingsTabViewModel>(vm => vm.EndFailureVelocity = 1),
                                          nameof(GeneralSimulationSettingsTabViewModel.EndFailureVelocity))
                .SetName("End failure velocity");

            yield return new TestCaseData(new Action<GeneralSimulationSettingsTabViewModel>(vm => vm.GravitationalAcceleration = 0),
                                          nameof(GeneralSimulationSettingsTabViewModel.GravitationalAcceleration))
                .SetName("Gravitational acceleration");
            yield return new TestCaseData(new Action<GeneralSimulationSettingsTabViewModel>(vm => vm.Density = 2.0),
                                          nameof(GeneralSimulationSettingsTabViewModel.Density))
                .SetName("Gravitational acceleration");
        }
    }
}