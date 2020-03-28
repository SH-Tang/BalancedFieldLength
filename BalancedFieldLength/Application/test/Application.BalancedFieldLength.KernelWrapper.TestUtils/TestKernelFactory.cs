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
using Simulator.Kernel;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils
{
    /// <summary>
    /// Mockup class for creating instances of <see cref="IAggregatedDistanceCalculatorKernel"/>
    /// which can be used for testing in combination with the <see cref="BalancedFieldLengthKernelFactoryConfig"/>.
    /// </summary>
    public class TestKernelFactory : IBalancedFieldLengthKernelFactory
    {
        private readonly IAggregatedDistanceCalculatorKernel testKernel;

        /// <summary>
        /// Creates a new instance of <see cref="TestKernelFactory"/>.
        /// </summary>
        /// <param name="testKernel">The <see cref="IAggregatedDistanceCalculatorKernel"/>
        /// to run the factory with.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="testKernel"/>
        /// is <c>null</c>.</exception>
        public TestKernelFactory(IAggregatedDistanceCalculatorKernel testKernel)
        {
            if (testKernel == null)
            {
                throw new ArgumentNullException(nameof(testKernel));
            }

            this.testKernel = testKernel;
        }

        public IAggregatedDistanceCalculatorKernel CreateDistanceCalculatorKernel()
        {
            return testKernel;
        }
    }
}