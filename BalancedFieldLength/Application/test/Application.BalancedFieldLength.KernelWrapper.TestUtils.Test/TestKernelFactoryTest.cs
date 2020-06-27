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
using NSubstitute;
using NUnit.Framework;
using Simulator.Kernel;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils.Test
{
    [TestFixture]
    public class TestKernelFactoryTest
    {
        [Test]
        public void Constructor_TestKernelNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new TestKernelFactory(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("testKernel"));
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            var testKernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();

            // Call
            var factory = new TestKernelFactory(testKernel);

            // Assert
            Assert.That(factory, Is.InstanceOf<IBalancedFieldLengthKernelFactory>());
        }

        [Test]
        public void CreateDistanceCalculatorKernel_Always_ReturnsExpectedKernel()
        {
            // Setup
            var testKernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            var factory = new TestKernelFactory(testKernel);

            // Call
            IAggregatedDistanceCalculatorKernel kernel = factory.CreateDistanceCalculatorKernel();

            // Assert
            Assert.That(kernel, Is.SameAs(testKernel));
        }
    }
}