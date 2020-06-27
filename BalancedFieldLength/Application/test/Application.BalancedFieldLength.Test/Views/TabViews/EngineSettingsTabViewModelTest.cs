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
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.Views.TabViews;
using NUnit.Framework;
using WPF.Components.TabControl;
using WPF.Core;

namespace Application.BalancedFieldLength.Test.Views.TabViews
{
    [TestFixture]
    public class EngineSettingsTabViewModelTest
    {
        [Test]
        public void Constructor_EngineDataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new EngineSettingsTabViewModel(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("engineData"));
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var engineData = new EngineData();

            // Call
            var viewModel = new EngineSettingsTabViewModel(engineData);

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel, Is.InstanceOf<ITabViewModel>());
            Assert.That(viewModel.TabName, Is.EqualTo("Engine data"));

            Assert.That(viewModel.ThrustPerEngine, Is.EqualTo(engineData.ThrustPerEngine));
            Assert.That(viewModel.NrOfEngines, Is.EqualTo(engineData.NrOfEngines));
            Assert.That(viewModel.NrOfFailedEngines, Is.EqualTo(engineData.NrOfFailedEngines));
        }

        [Test]
        public void NrOfEngines_SettingNewValue_SetsEngineData()
        {
            // Setup
            var engineData = new EngineData();
            var viewModel = new EngineSettingsTabViewModel(engineData);

            var random = new Random(21);
            int newValue = random.Next();

            // Call 
            viewModel.NrOfEngines = newValue;

            // Assert
            Assert.That(engineData.NrOfEngines, Is.EqualTo(newValue));
        }

        [Test]
        public void ThrustPerEngine_SettingNewValue_SetsEngineData()
        {
            // Setup
            var engineData = new EngineData();
            var viewModel = new EngineSettingsTabViewModel(engineData);

            var random = new Random(21);
            double newValue = random.NextDouble();

            // Call 
            viewModel.ThrustPerEngine = newValue;

            // Assert
            Assert.That(engineData.ThrustPerEngine, Is.EqualTo(newValue));
        }

        [Test]
        public void NrOfFailedEngines_SettingNewValue_RaisesOnPropertyChangedEventAndSetsEngineDat()
        {
            // Setup
            var engineData = new EngineData();
            var viewModel = new EngineSettingsTabViewModel(engineData);

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
            viewModel.NrOfFailedEngines = newValue;

            // Assert
            Assert.That(engineData.NrOfFailedEngines, Is.EqualTo(newValue));
            Assert.That(propertyChangedTriggered, Is.True);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(EngineSettingsTabViewModel.NrOfFailedEngines)));
        }

        [Test]
        public void GivenViewModelWithNrOfEnginesNotZero_WhenThrustPerEngineSet_ThenNotifyPropertyChangedEventsFiredAndTotalThrustUpdated()
        {
            // Given
            const int nrOfEngines = 4;
            var engineData = new EngineData
            {
                NrOfEngines = nrOfEngines
            };
            var viewModel = new EngineSettingsTabViewModel(engineData);

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
            double expectedTotalThrust = nrOfEngines * thrustPerEngine;
            Assert.That(totalThrust, Is.EqualTo(expectedTotalThrust).Within(1e-5));
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
            var engineData = new EngineData
            {
                ThrustPerEngine = thrustPerEngine
            };
            var viewModel = new EngineSettingsTabViewModel(engineData);

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
            var engineData = new EngineData
            {
                NrOfEngines = nrOfEngines,
                ThrustPerEngine = random.NextDouble()
            };
            var viewModel = new EngineSettingsTabViewModel(engineData);

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
            var engineData = new EngineData
            {
                NrOfEngines = nrOfEngines,
                ThrustPerEngine = thrustPerEngine
            };
            var viewModel = new EngineSettingsTabViewModel(engineData);

            var eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            // When 
            viewModel.ThrustPerEngine = thrustPerEngine + tolerance;

            // Then
            Assert.That(engineData.ThrustPerEngine, Is.EqualTo(thrustPerEngine));
            Assert.That(eventArgsCollection, Is.Empty);
        }

        [Test]
        public void GivenViewModelWithNrOfEnginesAndThrustPerEngineNotZero_WhenSettingThrustPerEngineNaN_ThenEventsFired()
        {
            // Given
            const int nrOfEngines = 4;

            var random = new Random(21);
            double thrustPerEngine = random.NextDouble();
            var engineData = new EngineData
            {
                NrOfEngines = nrOfEngines,
                ThrustPerEngine = thrustPerEngine
            };
            var viewModel = new EngineSettingsTabViewModel(engineData);

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

            var engineData = new EngineData
            {
                NrOfEngines = nrOfEngines,
                NrOfFailedEngines = nrOfEngines - 1
            };
            var viewModel = new EngineSettingsTabViewModel(engineData);

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
            Assert.That(engineData.NrOfEngines, Is.EqualTo(updatedNrOfEngines));

            const int expectedNrOfFailedEngines = updatedNrOfEngines - 1;
            Assert.That(engineData.NrOfFailedEngines, Is.EqualTo(expectedNrOfFailedEngines));
            Assert.That(updatedNrOfFailedEngines, Is.EqualTo(expectedNrOfFailedEngines));

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