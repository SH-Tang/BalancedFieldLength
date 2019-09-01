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
using System.Linq;
using Application.BalancedFieldLength.Controls;
using Application.BalancedFieldLength.WPFCommon;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Controls
{
    [TestFixture]
    public class EngineSettingsTabViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new EngineSettingsTabViewModel();

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel, Is.InstanceOf<ITabViewModel>());
            Assert.That(viewModel.TabName, Is.EqualTo("Engine data"));

            Assert.That(viewModel.ThrustPerEngine, Is.NaN);
            Assert.That(viewModel.TotalThrust, Is.NaN);
            Assert.That(viewModel.NrOfEngines, Is.Zero);
            Assert.That(viewModel.NrOfFailedEngines, Is.Zero);
            Assert.That(viewModel.MaximumNrOfFailedEngines, Is.Zero);
        }

        [Test]
        public void NrOfFailedEngines_SettingNewValue_RaisesOnPropertyChangedEvent()
        {
            // Setup
            var viewModel = new EngineSettingsTabViewModel();

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);

            // Call 
            viewModel.NrOfFailedEngines = random.Next();

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(EngineSettingsTabViewModel.NrOfFailedEngines)));
        }

        [Test]
        public void GivenViewModelWithNrOfEnginesNotZero_WhenThrustPerEngineSet_ThenNotifyPropertyChangedEventsFiredAndTotalThrustUpdated()
        {
            // Given
            const int nrOfEngines = 4;
            var viewModel = new EngineSettingsTabViewModel
            {
                NrOfEngines = nrOfEngines
            };

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            var random = new Random(21);
            double thrustPerEngine = random.NextDouble();

            // When 
            viewModel.ThrustPerEngine = thrustPerEngine;
            double totalThrust = viewModel.TotalThrust;

            // Then
            Assert.That(totalThrust, Is.EqualTo(nrOfEngines * thrustPerEngine).Within(1e-5));
            CollectionAssert.AreEquivalent(new[]
            {
                "ThrustPerEngine",
                "TotalThrust"
            }, eventArgsCollection.Select(e => e.PropertyName));
        }

        [Test]
        public void GivenViewModelWithThrustPerEngineNotZero_WhenNrOfEnginesSet_ThenNotifyPropertyChangedEventsFiredAndTotalThrustUpdated()
        {
            // Given
            var random = new Random(21);
            double thrustPerEngine = random.NextDouble();
            var viewModel = new EngineSettingsTabViewModel
            {
                ThrustPerEngine = thrustPerEngine
            };

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            const int nrOfEngines = 4;

            // When 
            viewModel.NrOfEngines = nrOfEngines;
            double totalThrust = viewModel.TotalThrust;
            int maximumNrOfFailedEngines = viewModel.MaximumNrOfFailedEngines;

            // Then
            Assert.That(maximumNrOfFailedEngines, Is.EqualTo(nrOfEngines - 1));
            Assert.That(totalThrust, Is.EqualTo(nrOfEngines * thrustPerEngine).Within(1e-5));
            CollectionAssert.AreEquivalent(new[]
            {
                "NrOfEngines",
                "TotalThrust",
                "MaximumNrOfFailedEngines"
            }, eventArgsCollection.Select(e => e.PropertyName));
        }

        [Test]
        public void GivenViewModelWithNrOfEnginesAndThrustPerEngineNotZero_WhenSettingSameNrOfEngines_ThenNoEventsFired()
        {
            // Given
            const int nrOfEngines = 4;

            var random = new Random(21);
            var viewModel = new EngineSettingsTabViewModel
            {
                NrOfEngines = nrOfEngines,
                ThrustPerEngine = random.NextDouble()
            };

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            // When 
            viewModel.NrOfEngines = nrOfEngines;

            // Then
            Assert.That(eventArgsCollection, Is.Empty);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1e-5)]
        [TestCase(+1e-5)]
        public void GivenViewModelWithNrOfEnginesAndThrustPerEngineNotZero_WhenSettingSameThrustPerEngine_ThenNoEventsFired(double tolerance)
        {
            // Given
            const int nrOfEngines = 4;

            var random = new Random(21);
            double thrustPerEngine = random.NextDouble();
            var viewModel = new EngineSettingsTabViewModel
            {
                NrOfEngines = nrOfEngines,
                ThrustPerEngine = thrustPerEngine
            };

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            // When 
            viewModel.ThrustPerEngine = thrustPerEngine + tolerance;

            // Then
            Assert.That(eventArgsCollection, Is.Empty);
        }

        [Test]
        public void GivenViewModelWithNrOfEnginesAndThrustPerEngineNotZero_WhenSettingThrustPerEngineNaN_ThenEventsFired()
        {
            // Given
            const int nrOfEngines = 4;

            var random = new Random(21);
            double thrustPerEngine = random.NextDouble();
            var viewModel = new EngineSettingsTabViewModel
            {
                NrOfEngines = nrOfEngines,
                ThrustPerEngine = thrustPerEngine
            };

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            // When 
            viewModel.ThrustPerEngine = double.NaN;
            double totalThrust = viewModel.TotalThrust;

            // Then
            Assert.That(totalThrust, Is.NaN);
            CollectionAssert.AreEquivalent(new[]
            {
                "ThrustPerEngine",
                "TotalThrust"
            }, eventArgsCollection.Select(e => e.PropertyName));
        }

        [Test]
        public void GivenViewModelWithMaximumNrOfFailedEngines_WhenNrOfEnginesDecreasesBelowMaximumAllowed_ThenMaximumNrOfEnginesCappedAndEventsFired()
        {
            // Given
            const int nrOfEngines = 4;

            var viewModel = new EngineSettingsTabViewModel
            {
                NrOfEngines = nrOfEngines,
                NrOfFailedEngines = nrOfEngines - 1
            };

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            // When 
            const int updatedNrOfEngines = 3;
            viewModel.NrOfEngines = updatedNrOfEngines;
            int updatedNrOfFailedEngines = viewModel.NrOfFailedEngines;

            // Then
            Assert.That(updatedNrOfFailedEngines, Is.EqualTo(updatedNrOfEngines - 1));
            CollectionAssert.AreEquivalent(new[]
            {
                "NrOfEngines",
                "TotalThrust",
                "MaximumNrOfFailedEngines",
                "NrOfFailedEngines"
            }, eventArgsCollection.Select(e => e.PropertyName));
        }
    }
}