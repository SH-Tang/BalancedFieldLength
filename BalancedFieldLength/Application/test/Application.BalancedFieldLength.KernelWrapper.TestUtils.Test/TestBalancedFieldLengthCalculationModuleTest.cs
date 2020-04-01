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
using System.Linq;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using NUnit.Framework;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils.Test
{
    [TestFixture]
    public class TestBalancedFieldLengthCalculationModuleTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var calculationModule = new TestBalancedFieldLengthCalculationModule();

            // Assert
            Assert.That(calculationModule, Is.InstanceOf<IBalancedFieldLengthCalculationModule>());
        }

        [Test]
        public void Validate_WithThrowCreateKernelDataExceptionTrue_ThrowsCreateKernelDataException()
        {
            // Given
            var calculationModule = new TestBalancedFieldLengthCalculationModule
            {
                ThrowCreateKernelDataException = true
            };

            // When
            TestDelegate call = () => calculationModule.Validate(null);

            // Then
            Assert.That(call, Throws.Exception.TypeOf<CreateKernelDataException>()
                                    .And.Message.Not.Empty);
        }

        [Test]
        public void Validate_WithConfiguredCalculationModule_ExpectedValues()
        {
            // Given
            IEnumerable<string> messages = Enumerable.Empty<string>();
            var calculationModule = new TestBalancedFieldLengthCalculationModule
            {
                ValidationMessages = messages
            };

            var calculation = new BalancedFieldLengthCalculation();

            // When
            IEnumerable<string> actualMessages = calculationModule.Validate(calculation);

            // Then
            Assert.That(actualMessages, Is.SameAs(messages));
            Assert.That(calculationModule.InputCalculation, Is.SameAs(calculation));
        }

        [Test]
        public void Calculate_WithThrowCreateKernelDataExceptionTrue_ThrowsCreateKernelDataException()
        {
            // Given
            var calculationModule = new TestBalancedFieldLengthCalculationModule
            {
                ThrowCreateKernelDataException = true
            };

            // When
            TestDelegate call = () => calculationModule.Calculate(null);

            // Then
            Assert.That(call, Throws.Exception.TypeOf<CreateKernelDataException>()
                                    .And.Message.Not.Empty);
        }

        [Test]
        public void Calculate_WithThrowKernelCalculationExceptionTrue_ThrowsKernelCalculationException()
        {
            // Given
            var calculationModule = new TestBalancedFieldLengthCalculationModule
            {
                ThrowKernelCalculationException = true
            };

            // When
            TestDelegate call = () => calculationModule.Calculate(null);

            // Then
            Assert.That(call, Throws.Exception.TypeOf<KernelCalculationException>()
                                    .And.Message.Not.Empty);
        }

        [Test]
        public void Calculate_WithConfiguredCalculationModule_ExpectedValues()
        {
            // Given
            var random = new Random(21);
            var output = new BalancedFieldLengthOutput(random.NextDouble(), random.NextDouble());
            var calculationModule = new TestBalancedFieldLengthCalculationModule
            {
                Output = output
            };

            var calculation = new BalancedFieldLengthCalculation();

            // When
            BalancedFieldLengthOutput actualOutput = calculationModule.Calculate(calculation);

            // Then
            Assert.That(actualOutput, Is.SameAs(output));
            Assert.That(calculationModule.InputCalculation, Is.SameAs(calculation));
        }
    }
}