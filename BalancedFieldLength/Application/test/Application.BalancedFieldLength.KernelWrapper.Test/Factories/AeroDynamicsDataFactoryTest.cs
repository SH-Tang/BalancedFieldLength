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
using Core.Common.TestUtil;
using NUnit.Framework;
using KernelAerodynamicsData = Simulator.Data.AerodynamicsData;

namespace Application.BalancedFieldLength.KernelWrapper.Test.Factories
{
    [TestFixture]
    public class AerodynamicsDataFactoryTest
    {
        [Test]
        public void Create_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => AerodynamicsDataFactory.Create(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("aircraftData"));
        }

        [Test]
        public void Create_WithCalculation_ReturnsExpectedAircraftData()
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
                RestDragCoefficientWithEngineFailure = random.NextDouble()
            };

            // Call 
            KernelAerodynamicsData data = AerodynamicsDataFactory.Create(aircraftData);

            // Assert
            Assert.That(data.AspectRatio, Is.EqualTo(aircraftData.AspectRatio));
            Assert.That(data.WingArea, Is.EqualTo(aircraftData.WingSurfaceArea));

            Assert.That(data.LiftCoefficientGradient, Is.EqualTo(aircraftData.LiftCoefficientGradient));
            Assert.That(data.MaximumLiftCoefficient, Is.EqualTo(aircraftData.MaximumLiftCoefficient));
            Assert.That(data.ZeroLiftAngleOfAttack, Is.EqualTo(aircraftData.ZeroLiftAngleOfAttack));
            Assert.That(data.OswaldFactor, Is.EqualTo(aircraftData.OswaldFactor));

            Assert.That(data.RestDragCoefficientWithEngineFailure, Is.EqualTo(aircraftData.RestDragCoefficientWithEngineFailure));
            Assert.That(data.RestDragCoefficientWithoutEngineFailure, Is.EqualTo(aircraftData.RestDragCoefficient));
        }

        [Test]
        public void Create_WithAircraftDataResultingInArgumentException_ThrowsCreateKernelDataException()
        {
            // Setup
            var random = new Random(21);
            var aircraftData = new AircraftData
            {
                WingSurfaceArea = double.NaN,
                AspectRatio = random.NextDouble(),
                OswaldFactor = random.NextDouble(),
                MaximumLiftCoefficient = random.NextDouble(),
                LiftCoefficientGradient = random.NextDouble(),
                ZeroLiftAngleOfAttack = random.NextAngle(),
                RestDragCoefficient = random.NextDouble(),
                RestDragCoefficientWithEngineFailure = random.NextDouble()
            };

            // Call 
            TestDelegate call = () => AerodynamicsDataFactory.Create(aircraftData);

            // Assert
            var exception = Assert.Throws<CreateKernelDataException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public void Create_WithAircraftDataResultingInArgumentOutOfRangeException_ThrowsCreateKernelDataException()
        {
            // Setup
            var random = new Random(21);
            var aircraftData = new AircraftData
            {
                WingSurfaceArea = -1 * random.NextDouble(),
                AspectRatio = random.NextDouble(),
                OswaldFactor = random.NextDouble(),
                MaximumLiftCoefficient = random.NextDouble(),
                LiftCoefficientGradient = random.NextDouble(),
                ZeroLiftAngleOfAttack = random.NextAngle(),
                RestDragCoefficient = random.NextDouble(),
                RestDragCoefficientWithEngineFailure = random.NextDouble()
            };

            // Call 
            TestDelegate call = () => AerodynamicsDataFactory.Create(aircraftData);

            // Assert
            var exception = Assert.Throws<CreateKernelDataException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(exception.Message, Is.EqualTo(exception.Message));
        }
    }
}