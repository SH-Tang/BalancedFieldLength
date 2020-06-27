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

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils
{
    /// <summary>
    /// This class can be used to set a temporary <see cref="IBalancedFieldLengthKernelFactory"/>
    /// for <see cref="BalancedFieldLengthKernelFactory.Instance"/> while testing. 
    /// Disposing an instance of this class will revert the 
    /// <see cref="BalancedFieldLengthKernelFactory.Instance"/>.
    /// </summary>
    /// <example>
    /// The following is an example for how to use this class:
    /// <code>
    /// using(new BalancedFieldLengthKernelFactoryConfig())
    /// {
    ///     var testFactory = (TestBalancedFieldLengthKernelFactoryConfig) BalancedFieldLengthKernelFactoryConfig.Instance;
    /// 
    ///     // Perform tests with testFactory
    /// }
    /// </code>
    /// </example>
    public sealed class BalancedFieldLengthKernelFactoryConfig : IDisposable
    {
        private readonly IBalancedFieldLengthKernelFactory originalInstance;

        /// <summary>
        /// Creates a new instance of <see cref="BalancedFieldLengthKernelFactoryConfig"/> with a
        /// specific <see cref="IBalancedFieldLengthKernelFactory"/>.
        /// </summary>
        /// <param name="testFactory">The <see cref="IBalancedFieldLengthKernelFactory"/>
        /// to set the <see cref="BalancedFieldLengthKernelFactory.Instance"/> with.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="testFactory"/>
        /// is <c>null</c>.</exception>
        public BalancedFieldLengthKernelFactoryConfig(IBalancedFieldLengthKernelFactory testFactory)
        {
            if (testFactory == null)
            {
                throw new ArgumentNullException(nameof(testFactory));
            }

            originalInstance = BalancedFieldLengthKernelFactory.Instance;
            BalancedFieldLengthKernelFactory.Instance = testFactory;
        }

        public void Dispose()
        {
            BalancedFieldLengthKernelFactory.Instance = originalInstance;
        }
    }
}