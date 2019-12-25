﻿// Copyright (C) 2018 Dennis Tang. All rights reserved.
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

using NUnit.Framework;
using Simulator.Kernel;

namespace Application.BalancedFieldLength.KernelWrapper.Test
{
    [TestFixture]
    public class BalancedFieldLengthKernelFactoryTest
    {
        [Test]
        public void Instance_Always_ReturnsFactory()
        {
            // Call 
            BalancedFieldLengthKernelFactory factory = BalancedFieldLengthKernelFactory.Instance;

            // Assert
            Assert.That(factory, Is.InstanceOf<IBalancedFieldLengthKernelFactory>());
        }

        [Test]
        public void Instance_Always_ReturnsSameInstance()
        {
            // Call 
            BalancedFieldLengthKernelFactory firstFactory = BalancedFieldLengthKernelFactory.Instance;
            BalancedFieldLengthKernelFactory secondFactory = BalancedFieldLengthKernelFactory.Instance;

            // Assert
            Assert.That(firstFactory, Is.SameAs(secondFactory));
        }

        [Test]
        public void CreateDistanceCalculatorKernel_Always_ReturnsKernel()
        {
            // Setup
            BalancedFieldLengthKernelFactory factory = BalancedFieldLengthKernelFactory.Instance;

            // Call 
            IAggregatedDistanceCalculatorKernel kernel = factory.CreateDistanceCalculatorKernel();

            // Assert
            Assert.That(kernel, Is.TypeOf<AggregatedDistanceCalculatorKernel>());
        }
    }
}