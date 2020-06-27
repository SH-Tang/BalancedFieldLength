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
using NUnit.Framework;
using Simulator.Data;

namespace Application.BalancedFieldLength.KernelWrapper.Test.Factories
{
    [TestFixture]
    public class CalculationSettingsFactoryTest
    {
        [Test]
        public void Create_SettingsNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);

            // Call
            TestDelegate call = () => CalculationSettingsFactory.Create(null, random.Next());

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("settings"));
        }

        [Test]
        public void Create_WithValidGeneralSimulationSettingsData_ReturnsExpectedCalculationSettings()
        {
            // Setup
            var random = new Random(21);
            int failureSpeed = random.Next();

            var settings = new GeneralSimulationSettingsData
            {
                TimeStep = random.NextDouble(),
                MaximumNrOfIterations = random.Next()
            };

            // Call 
            CalculationSettings calculationSettings = CalculationSettingsFactory.Create(settings, failureSpeed);

            // Assert
            CalculationSettingsTestHelper.AssertCalculationSettings(settings, failureSpeed, calculationSettings);
        }

        [Test]
        public void Create_WithGeneralSimulationSettingsDataResultingInArgumentException_ThrowsKernelDataCreateException()
        {
            // Setup
            var random = new Random(21);

            var settings = new GeneralSimulationSettingsData
            {
                TimeStep = double.NaN,
                MaximumNrOfIterations = random.Next()
            };

            // Call 
            TestDelegate call = () => CalculationSettingsFactory.Create(settings, random.Next());

            // Assert
            var exception = Assert.Throws<CreateKernelDataException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.Message, Is.EqualTo(innerException.Message));
        }

        [Test]
        public void Create_WithGeneralSimulationSettingsDataResultingInArgumentOutOfRangeException_ThrowsKernelDataCreateException()
        {
            // Setup
            var random = new Random(21);

            var settings = new GeneralSimulationSettingsData
            {
                TimeStep = random.NextDouble(),
                MaximumNrOfIterations = -1 * random.Next()
            };

            // Call 
            TestDelegate call = () => CalculationSettingsFactory.Create(settings, random.Next());

            // Assert
            var exception = Assert.Throws<CreateKernelDataException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(exception.Message, Is.EqualTo(innerException.Message));
        }
    }
}