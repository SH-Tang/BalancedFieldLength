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

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils.Test
{
    [TestFixture]
    public class BalancedFieldLengthKernelFactoryConfigTest
    {
        [Test]
        public void Constructor_TestFactoryNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new BalancedFieldLengthKernelFactoryConfig(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("testFactory"));
        }

        [Test]
        public void Constructor_CanBeDisposed()
        {
            // Setup
            var testFactory = Substitute.For<IBalancedFieldLengthKernelFactory>();

            // Call 
            var config = new BalancedFieldLengthKernelFactoryConfig(testFactory);

            // Assert
            Assert.That(config, Is.InstanceOf<IDisposable>());
            Assert.That(() => config.Dispose(), Throws.Nothing);
        }

        [Test]
        public void Constructor_Always_SetsInstanceOfKernelFactory()
        {
            // Setup
            var testFactory = Substitute.For<IBalancedFieldLengthKernelFactory>();
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                // Call 
                IBalancedFieldLengthKernelFactory instance = BalancedFieldLengthKernelFactory.Instance;

                // Assert
                Assert.That(instance, Is.SameAs(testFactory));
            }
        }

        [Test]
        public void GivenConfigWithTestFactory_WhenDisposing_ThenOriginalInstanceRestored()
        {
            // Given
            IBalancedFieldLengthKernelFactory originalInstance = BalancedFieldLengthKernelFactory.Instance;
            var testFactory = Substitute.For<IBalancedFieldLengthKernelFactory>();

            var config = new BalancedFieldLengthKernelFactoryConfig(testFactory);

            // Precondition
            Assert.That(BalancedFieldLengthKernelFactory.Instance, Is.SameAs(testFactory));

            // When 
            config.Dispose();

            // Then
            Assert.That(BalancedFieldLengthKernelFactory.Instance, Is.SameAs(originalInstance));
        }
    }
}