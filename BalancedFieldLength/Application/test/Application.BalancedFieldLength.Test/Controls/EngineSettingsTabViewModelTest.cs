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

            List<PropertyChangedEventArgs> eventArgsCollection = new List<PropertyChangedEventArgs>();
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

            List<PropertyChangedEventArgs> eventArgsCollection = new List<PropertyChangedEventArgs>();
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgsCollection.Add(e);
            };

            const int nrOfEngines = 4;

            // When 
            viewModel.NrOfEngines = nrOfEngines;
            double totalThrust = viewModel.TotalThrust;

            // Then
            Assert.That(totalThrust, Is.EqualTo(nrOfEngines * thrustPerEngine).Within(1e-5));
            CollectionAssert.AreEquivalent(new[]
            {
                "NrOfEngines",
                "TotalThrust"
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

            List<PropertyChangedEventArgs> eventArgsCollection = new List<PropertyChangedEventArgs>();
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

            List<PropertyChangedEventArgs> eventArgsCollection = new List<PropertyChangedEventArgs>();
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

            List<PropertyChangedEventArgs> eventArgsCollection = new List<PropertyChangedEventArgs>();
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
    }
}