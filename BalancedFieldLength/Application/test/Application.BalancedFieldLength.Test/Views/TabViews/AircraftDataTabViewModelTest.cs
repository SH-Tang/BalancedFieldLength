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
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;
using WPF.Components.TabControl;
using WPF.Core;

namespace Application.BalancedFieldLength.Test.Views.TabViews
{
    [TestFixture]
    public class AircraftDataTabViewModelTest
    {
        [Test]
        public void Constructor_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new AircraftDataTabViewModel(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("aircraftData"));
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var data = new AircraftData();

            // Call
            var viewModel = new AircraftDataTabViewModel(data);

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ITabViewModel>());
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel.TabName, Is.EqualTo("Aircraft data"));

            Assert.That(viewModel.TakeOffWeight, Is.EqualTo(data.TakeOffWeight));
            Assert.That(viewModel.PitchGradient, Is.EqualTo(data.PitchGradient));
            Assert.That(viewModel.MaximumPitchAngle, Is.EqualTo(data.MaximumPitchAngle));

            Assert.That(viewModel.WingSurfaceArea, Is.EqualTo(data.WingSurfaceArea));
            Assert.That(viewModel.AspectRatio, Is.EqualTo(data.AspectRatio));
            Assert.That(viewModel.OswaldFactor, Is.EqualTo(data.OswaldFactor));

            Assert.That(viewModel.MaximumLiftCoefficient, Is.EqualTo(data.MaximumLiftCoefficient));
            Assert.That(viewModel.LiftCoefficientGradient, Is.EqualTo(data.LiftCoefficientGradient));
            Assert.That(viewModel.ZeroLiftAngleOfAttack, Is.EqualTo(data.ZeroLiftAngleOfAttack));

            Assert.That(viewModel.RestDragCoefficientWithEngineFailure, Is.EqualTo(data.RestDragCoefficientWithEngineFailure));
            Assert.That(viewModel.RestDragCoefficient, Is.EqualTo(data.RestDragCoefficient));

            Assert.That(viewModel.RollResistanceCoefficient, Is.EqualTo(data.RollResistanceCoefficient));
            Assert.That(viewModel.RollResistanceWithBrakesCoefficient, Is.EqualTo(data.RollResistanceWithBrakesCoefficient));
        }

        [Test]
        public void TakeOffWeight_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.TakeOffWeight = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.TakeOffWeight)));
            Assert.That(data.TakeOffWeight, Is.EqualTo(newValue));
        }

        [Test]
        public void PitchGradient_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            Angle newValue = random.NextAngle();

            // Call 
            viewModel.PitchGradient = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.PitchGradient)));
            Assert.That(data.PitchGradient, Is.EqualTo(newValue));
        }

        [Test]
        public void MaximumPitchAngle_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            Angle newValue = random.NextAngle();

            // Call 
            viewModel.MaximumPitchAngle = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.MaximumPitchAngle)));
            Assert.That(data.MaximumPitchAngle, Is.EqualTo(newValue));
        }

        [Test]
        public void WingSurfaceArea_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.WingSurfaceArea = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.WingSurfaceArea)));
            Assert.That(data.WingSurfaceArea, Is.EqualTo(newValue));
        }

        [Test]
        public void AspectRatio_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.AspectRatio = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.AspectRatio)));
            Assert.That(data.AspectRatio, Is.EqualTo(newValue));
        }

        [Test]
        public void OswaldFactor_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.OswaldFactor = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.OswaldFactor)));
            Assert.That(data.OswaldFactor, Is.EqualTo(newValue));
        }

        [Test]
        public void MaximumLiftCoefficient_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.MaximumLiftCoefficient = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.MaximumLiftCoefficient)));
            Assert.That(data.MaximumLiftCoefficient, Is.EqualTo(newValue));
        }

        [Test]
        public void LiftCoefficientGradient_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.LiftCoefficientGradient = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.LiftCoefficientGradient)));
            Assert.That(data.LiftCoefficientGradient, Is.EqualTo(newValue));
        }

        [Test]
        public void ZeroLiftAngleOfAttack_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var random = new Random(21);
            Angle newValue = random.NextAngle();

            // Call 
            viewModel.ZeroLiftAngleOfAttack = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.ZeroLiftAngleOfAttack)));
            Assert.That(data.ZeroLiftAngleOfAttack, Is.EqualTo(newValue));
        }

        [Test]
        public void RestDragCoefficient_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.RestDragCoefficient = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.RestDragCoefficient)));
            Assert.That(data.RestDragCoefficient, Is.EqualTo(newValue));
        }

        [Test]
        public void RestDragCoefficientWithEngineFailure_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.RestDragCoefficientWithEngineFailure = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.RestDragCoefficientWithEngineFailure)));
            Assert.That(data.RestDragCoefficientWithEngineFailure, Is.EqualTo(newValue));
        }

        [Test]
        public void RollResistanceCoefficient_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.RollResistanceCoefficient = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.RollResistanceCoefficient)));
            Assert.That(data.RollResistanceCoefficient, Is.EqualTo(newValue));
        }

        [Test]
        public void RollResistanceWithBrakesCoefficient_ValueChanges_RaisesNotifyPropertyChangedEvent()
        {
            // Setup
            var data = new AircraftData();
            var viewModel = new AircraftDataTabViewModel(data);

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
            viewModel.RollResistanceWithBrakesCoefficient = newValue;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(AircraftDataTabViewModel.RollResistanceWithBrakesCoefficient)));
            Assert.That(data.RollResistanceWithBrakesCoefficient, Is.EqualTo(newValue));
        }
    }
}