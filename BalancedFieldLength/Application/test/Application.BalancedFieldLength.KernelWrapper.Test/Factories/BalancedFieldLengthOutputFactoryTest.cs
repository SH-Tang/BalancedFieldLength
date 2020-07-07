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
using Application.BalancedFieldLength.KernelWrapper.Factories;
using NUnit.Framework;
using Simulator.Calculator.AggregatedDistanceCalculator;

namespace Application.BalancedFieldLength.KernelWrapper.Test.Factories
{
    [TestFixture]
    public class BalancedFieldLengthOutputFactoryTest
    {
        [Test]
        public void Create_OutputsNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => BalancedFieldLengthOutputFactory.Create(null);

            // Assert
            Assert.That(call, Throws.TypeOf<ArgumentNullException>()
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("outputs"));
        }

        [Test]
        public void Create_WithValidCollection_ReturnsExpectedBalancedFieldLengthOutput()
        {
            // Setup
            const double expectedVelocity = 11;
            const double expectedDistance = 20;
            var outputs = new[]
            {
                new AggregatedDistanceOutput(10, 10, 30),
                new AggregatedDistanceOutput(expectedVelocity, expectedDistance, 20),
                new AggregatedDistanceOutput(12, 30, 10)
            };

            // Call
            BalancedFieldLengthOutput output = BalancedFieldLengthOutputFactory.Create(outputs);

            // Assert
            Assert.That(output.Distance, Is.EqualTo(expectedDistance));
            Assert.That(output.Velocity, Is.EqualTo(expectedVelocity));
        }

        [Test]
        public void Create_InputArgumentCausesException_ThrowsKernelCalculationException()
        {
            // Setup
            IEnumerable<AggregatedDistanceOutput> outputs = Enumerable.Empty<AggregatedDistanceOutput>();

            // Call
            TestDelegate call = () => BalancedFieldLengthOutputFactory.Create(outputs);

            // Assert
            var exception = Assert.Throws<KernelCalculationException>(call);
            Exception innerException = exception.InnerException;
            Assert.That(innerException, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(innerException.Message));
        }
    }
}