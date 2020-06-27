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

using NUnit.Framework;

namespace Application.BalancedFieldLength.KernelWrapper.Test
{
    [TestFixture]
    public class BalancedFieldLengthCalculationModuleFactoryTest
    {
        [Test]
        public void Instance_Always_ReturnsFactory()
        {
            // Call 
            var factory = BalancedFieldLengthCalculationModuleFactory.Instance;

            // Assert
            Assert.That(factory, Is.TypeOf<BalancedFieldLengthCalculationModuleFactory>());
        }

        [Test]
        public void Instance_Always_ReturnsSameInstance()
        {
            // Call 
            var firstFactory = BalancedFieldLengthCalculationModuleFactory.Instance;
            var secondFactory = BalancedFieldLengthCalculationModuleFactory.Instance;

            // Assert
            Assert.That(firstFactory, Is.SameAs(secondFactory));
        }

        [Test]
        public void CreateModule_Always_ReturnsKernel()
        {
            // Setup
            var factory = BalancedFieldLengthCalculationModuleFactory.Instance;

            // Call 
            IBalancedFieldLengthCalculationModule calculationModule = factory.CreateModule();

            // Assert
            Assert.That(calculationModule, Is.TypeOf<BalancedFieldLengthCalculationModule>());
        }
    }
}