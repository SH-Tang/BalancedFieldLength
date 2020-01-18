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
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using Application.BalancedFieldLength.KernelWrapper.Factories;
using Application.BalancedFieldLength.KernelWrapper.TestUtils;
using Core.Common.TestUtil;
using NUnit.Framework;
using KernelAircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper.Test.Factories
{
    [TestFixture]
    public class AircraftDataFactoryTest
    {
        [Test]
        public void Create_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => AircraftDataFactory.Create(null, new EngineData());

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("aircraftData"));
        }

        [Test]
        public void Create_EngineDataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => AircraftDataFactory.Create(new AircraftData(), null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("engineData"));
        }

        [Test]
        public void Create_WithArguments_ReturnsExpectedAircraftData()
        {
            // Setup
            var random = new Random(21);

            var aircraftData = new AircraftData
            {
                WingSurfaceArea = random.NextDouble(),
                AspectRatio = random.NextDouble(),
                OswaldFactor = random.NextDouble(),
                MaximumLiftCoefficient = random.NextDouble(),
                LiftCoefficientGradient = random.NextDouble(),
                ZeroLiftAngleOfAttack = random.NextAngle(),
                RestDragCoefficient = random.NextDouble(),
                RestDragCoefficientWithEngineFailure = random.NextDouble(),
                MaximumPitchAngle = random.NextAngle(),
                PitchGradient = random.NextAngle(),
                RollResistanceCoefficient = random.NextDouble(),
                RollResistanceWithBrakesCoefficient = random.NextDouble(),
                TakeOffWeight = random.NextDouble()
            };

            var engineData = new EngineData
            {
                NrOfEngines = random.Next(),
                ThrustPerEngine = random.NextDouble()
            };

            // Call 
            KernelAircraftData data = AircraftDataFactory.Create(aircraftData, engineData);

            // Assert
            Assert.That(data.MaximumPitchAngle, Is.EqualTo(aircraftData.MaximumPitchAngle));
            Assert.That(data.PitchAngleGradient, Is.EqualTo(aircraftData.PitchGradient));
            Assert.That(data.RollingResistanceCoefficient, Is.EqualTo(aircraftData.RollResistanceCoefficient));
            Assert.That(data.BrakingResistanceCoefficient, Is.EqualTo(aircraftData.RollResistanceWithBrakesCoefficient));

            Assert.That(data.NrOfEngines, Is.EqualTo(engineData.NrOfEngines));
            Assert.That(data.MaximumThrustPerEngine, Is.EqualTo(engineData.ThrustPerEngine));
            Assert.That(data.TakeOffWeight, Is.EqualTo(aircraftData.TakeOffWeight));

            AerodynamicsDataTestHelper.AssertAerodynamicsData(aircraftData, data.AerodynamicsData);
        }

        [Test]
        public void Create_WithDataResultingInArgumentOutOfRangeException_ThrowsKernelCreateException()
        {
            // Setup
            var random = new Random(21);

            var aircraftData = new AircraftData
            {
                WingSurfaceArea = random.NextDouble(),
                AspectRatio = random.NextDouble(),
                OswaldFactor = random.NextDouble(),
                MaximumLiftCoefficient = random.NextDouble(),
                LiftCoefficientGradient = random.NextDouble(),
                ZeroLiftAngleOfAttack = random.NextAngle(),
                RestDragCoefficient = random.NextDouble(),
                RestDragCoefficientWithEngineFailure = random.NextDouble(),
                MaximumPitchAngle = random.NextAngle(),
                PitchGradient = random.NextAngle(),
                RollResistanceCoefficient = random.NextDouble(),
                RollResistanceWithBrakesCoefficient = random.NextDouble(),
                TakeOffWeight = random.NextDouble()
            };

            var engineData = new EngineData
            {
                NrOfEngines = random.Next(),
                ThrustPerEngine = -random.NextDouble()
            };

            // Call 
            TestDelegate call = () => AircraftDataFactory.Create(aircraftData, engineData);

            // Assert
            var exception = Assert.Throws<CreateKernelDataException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(exception.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public void Create_WithDataResultingInArgumentException_ThrowsKernelCreateException()
        {
            // Setup
            var random = new Random(21);

            var aircraftData = new AircraftData
            {
                WingSurfaceArea = random.NextDouble(),
                AspectRatio = random.NextDouble(),
                OswaldFactor = random.NextDouble(),
                MaximumLiftCoefficient = random.NextDouble(),
                LiftCoefficientGradient = random.NextDouble(),
                ZeroLiftAngleOfAttack = random.NextAngle(),
                RestDragCoefficient = random.NextDouble(),
                RestDragCoefficientWithEngineFailure = random.NextDouble(),
                MaximumPitchAngle = random.NextAngle(),
                PitchGradient = random.NextAngle(),
                RollResistanceCoefficient = random.NextDouble(),
                RollResistanceWithBrakesCoefficient = random.NextDouble(),
                TakeOffWeight = random.NextDouble()
            };

            var engineData = new EngineData
            {
                NrOfEngines = random.Next(),
                ThrustPerEngine = double.NaN
            };

            // Call 
            TestDelegate call = () => AircraftDataFactory.Create(aircraftData, engineData);

            // Assert
            var exception = Assert.Throws<CreateKernelDataException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.Message, Is.EqualTo(exception.Message));
        }
    }
}