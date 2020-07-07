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
using NUnit.Framework;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils.Test
{
    [TestFixture]
    public class BalancedFieldLengthCalculationModuleFactoryConfigTest
    {
        [Test]
        public void Constructor_CanBeDisposed()
        {
            // Call 
            var config = new BalancedFieldLengthCalculationModuleFactoryConfig();

            // Assert
            Assert.That(config, Is.InstanceOf<IDisposable>());
            Assert.That(() => config.Dispose(), Throws.Nothing);
        }

        [Test]
        public void Constructor_Always_ReturnsTestFactory()
        {
            // Setup
            using (new BalancedFieldLengthCalculationModuleFactoryConfig())
            {
                // Call 
                IBalancedFieldLengthCalculationModuleFactory instance = BalancedFieldLengthCalculationModuleFactory.Instance;

                // Assert
                Assert.That(instance, Is.TypeOf<TestBalancedFieldLengthCalculationModuleFactory>());
            }
        }

        [Test]
        public void GivenConfigWithTestFactory_WhenDisposing_ThenOriginalInstanceRestored()
        {
            // Given
            IBalancedFieldLengthCalculationModuleFactory originalInstance = BalancedFieldLengthCalculationModuleFactory.Instance;

            var config = new BalancedFieldLengthCalculationModuleFactoryConfig();

            // Precondition
            Assert.That(BalancedFieldLengthCalculationModuleFactory.Instance, Is.Not.SameAs(originalInstance));

            // When 
            config.Dispose();

            // Then
            Assert.That(BalancedFieldLengthCalculationModuleFactory.Instance, Is.SameAs(originalInstance));
        }
    }
}