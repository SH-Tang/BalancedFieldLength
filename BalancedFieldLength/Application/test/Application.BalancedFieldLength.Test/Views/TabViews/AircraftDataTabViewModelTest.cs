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
using Application.BalancedFieldLength.Views.TabViews;
using NUnit.Framework;
using WPF.Components.TabControl;
using WPF.Core;

namespace Application.BalancedFieldLength.Test.Views.TabViews
{
    [TestFixture]
    public class AircraftDataTabViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new AircraftDataTabViewModel();

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ITabViewModel>());
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel.TabName, Is.EqualTo("Aircraft data"));

            Assert.That(viewModel.TakeOffWeight, Is.NaN);
            Assert.That(viewModel.PitchGradient, Is.NaN);
            Assert.That(viewModel.MaximumPitchAngle, Is.NaN);

            Assert.That(viewModel.WingSurfaceArea, Is.NaN);
            Assert.That(viewModel.AspectRatio, Is.NaN);
            Assert.That(viewModel.OswaldFactor, Is.NaN);

            Assert.That(viewModel.MaximumLiftCoefficient, Is.NaN);
            Assert.That(viewModel.LiftCoefficientGradient, Is.NaN);
            Assert.That(viewModel.ZeroLiftAngleOfAttack, Is.NaN);

            Assert.That(viewModel.RestDragCoefficientWithEngineFailure, Is.NaN);
            Assert.That(viewModel.RestDragCoefficient, Is.NaN);

            Assert.That(viewModel.RollResistanceCoefficient, Is.NaN);
            Assert.That(viewModel.RollResistanceWithBrakesCoefficient, Is.NaN);
        }

        [Test]
        [TestCaseSource(nameof(GetNotifyPropertyChangedTestCases))]
        public void Property_ValueChanges_RaisesNotifyPropertyChangedEvent(Action<AircraftDataTabViewModel> propertyChangeAction,
                                                                           string propertyName)
        {
            // Setup
            var viewModel = new AircraftDataTabViewModel();

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
            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.TakeOffWeight = 3.14),
                                          nameof(AircraftDataTabViewModel.TakeOffWeight))
                .SetName("Take off weight");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.PitchGradient = 3.14),
                                          nameof(AircraftDataTabViewModel.PitchGradient))
                .SetName("Pitch gradient");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.MaximumPitchAngle = 3.14),
                                          nameof(AircraftDataTabViewModel.MaximumPitchAngle))
                .SetName("Maximum pitch angle");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.WingSurfaceArea = 3.14),
                                          nameof(AircraftDataTabViewModel.WingSurfaceArea))
                .SetName("Wing surface area");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.AspectRatio = 3.14),
                                          nameof(AircraftDataTabViewModel.AspectRatio))
                .SetName("Aspect ratio");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.OswaldFactor = 3.14),
                                          nameof(AircraftDataTabViewModel.OswaldFactor))
                .SetName("Oswald Factor");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.MaximumLiftCoefficient = 3.14),
                                          nameof(AircraftDataTabViewModel.MaximumLiftCoefficient))
                .SetName("Maximum lift coefficient");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.LiftCoefficientGradient = 3.14),
                                          nameof(AircraftDataTabViewModel.LiftCoefficientGradient))
                .SetName("Lift coefficient gradient");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.ZeroLiftAngleOfAttack = 3.14),
                                          nameof(AircraftDataTabViewModel.ZeroLiftAngleOfAttack))
                .SetName("Zero lift angle of attack");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.RestDragCoefficient = 3.14),
                                          nameof(AircraftDataTabViewModel.RestDragCoefficient))
                .SetName("Rest drag coefficient");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.RestDragCoefficientWithEngineFailure = 3.14),
                                          nameof(AircraftDataTabViewModel.RestDragCoefficientWithEngineFailure))
                .SetName("Rest drag coefficient with engine failure");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.RollResistanceCoefficient = 3.14),
                                          nameof(AircraftDataTabViewModel.RollResistanceCoefficient))
                .SetName("Roll  resistance coefficient");

            yield return new TestCaseData(new Action<AircraftDataTabViewModel>(vm => vm.RollResistanceWithBrakesCoefficient = 3.14),
                                          nameof(AircraftDataTabViewModel.RollResistanceWithBrakesCoefficient))
                .SetName("Roll resistance with brakes coefficient");
        }
    }
}