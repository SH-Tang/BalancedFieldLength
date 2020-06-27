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
using System.ComponentModel;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.Views.TabViews;
using NUnit.Framework;
using WPF.Components.TabControl;
using WPF.Core;

namespace Application.BalancedFieldLength.Test.Views.TabViews
{
    [TestFixture]
    public class GeneralSimulationSettingsTabViewModelTest
    {
        [Test]
        public void Constructor_SettingsNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new GeneralSimulationSettingsTabViewModel(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("settings"));
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var settings = new GeneralSimulationSettingsData();

            // Call
            var viewModel = new GeneralSimulationSettingsTabViewModel(settings);

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ITabViewModel>());
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel.TabName, Is.EqualTo("Simulation Settings"));

            Assert.That(viewModel.MaximumNrOfIterations, Is.EqualTo(settings.MaximumNrOfIterations));
            Assert.That(viewModel.TimeStep, Is.EqualTo(settings.TimeStep));
            Assert.That(viewModel.EndFailureVelocity, Is.EqualTo(settings.EndFailureVelocity));
            Assert.That(viewModel.GravitationalAcceleration, Is.EqualTo(settings.GravitationalAcceleration));
            Assert.That(viewModel.Density, Is.EqualTo(settings.Density));
        }

        [Test]
        public void MaximumNrOfIterations_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var settings = new GeneralSimulationSettingsData();
            var viewModel = new GeneralSimulationSettingsTabViewModel(settings);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            int newValue = random.Next();

            // Call 
            viewModel.MaximumNrOfIterations = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(GeneralSimulationSettingsTabViewModel.MaximumNrOfIterations)));
            Assert.That(settings.MaximumNrOfIterations, Is.EqualTo(newValue));
        }

        [Test]
        public void TimeStep_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var settings = new GeneralSimulationSettingsData();
            var viewModel = new GeneralSimulationSettingsTabViewModel(settings);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            double newValue = random.NextDouble();

            // Call 
            viewModel.TimeStep = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(GeneralSimulationSettingsTabViewModel.TimeStep)));
            Assert.That(settings.TimeStep, Is.EqualTo(newValue));
        }

        [Test]
        public void EndFailureVelocity_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var settings = new GeneralSimulationSettingsData();
            var viewModel = new GeneralSimulationSettingsTabViewModel(settings);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            int newValue = random.Next();

            // Call 
            viewModel.EndFailureVelocity = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(GeneralSimulationSettingsTabViewModel.EndFailureVelocity)));
            Assert.That(settings.EndFailureVelocity, Is.EqualTo(newValue));
        }

        [Test]
        public void GravitationalAcceleration_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var settings = new GeneralSimulationSettingsData();
            var viewModel = new GeneralSimulationSettingsTabViewModel(settings);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            double newValue = random.NextDouble();

            // Call 
            viewModel.GravitationalAcceleration = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(GeneralSimulationSettingsTabViewModel.GravitationalAcceleration)));
            Assert.That(settings.GravitationalAcceleration, Is.EqualTo(newValue));
        }

        [Test]
        public void Density_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var settings = new GeneralSimulationSettingsData();
            var viewModel = new GeneralSimulationSettingsTabViewModel(settings);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            double newValue = random.NextDouble();

            // Call 
            viewModel.Density = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(GeneralSimulationSettingsTabViewModel.Density)));
            Assert.That(settings.Density, Is.EqualTo(newValue));
        }
    }
}