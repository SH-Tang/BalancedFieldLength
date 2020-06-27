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

using System.Collections.Generic;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;

namespace Application.BalancedFieldLength.KernelWrapper.TestUtils {
    /// <summary>
    /// Implementation of <see cref="IBalancedFieldLengthCalculationModule"/> which can be
    /// used for testing purposes.
    /// </summary>
    public class TestBalancedFieldLengthCalculationModule : IBalancedFieldLengthCalculationModule
    {
        /// <summary>
        /// Indicator whether a <see cref="CreateKernelDataException"/> should be thrown.
        /// </summary>
        public bool ThrowCreateKernelDataException { private get; set; }

        /// <summary>
        /// Sets the collection of messages to return.
        /// </summary>
        public IEnumerable<string> ValidationMessages { private get; set; }

        /// <summary>
        /// Gets the <see cref="BalancedFieldLengthCalculation"/> which was used for the function calls.
        /// </summary>
        public BalancedFieldLengthCalculation InputCalculation { get; private set; }

        /// <summary>
        /// Indicator whether a <see cref="KernelCalculationException"/> should be thrown.
        /// </summary>
        public bool ThrowKernelCalculationException { private get; set; }

        /// <summary>
        /// Sets the output to return.
        /// </summary>
        public BalancedFieldLengthOutput Output { private get; set; }

        public IEnumerable<string> Validate(BalancedFieldLengthCalculation calculation)
        {
            if (ThrowCreateKernelDataException)
            {
                throw new CreateKernelDataException("Exception");
            }

            InputCalculation = calculation;
            return ValidationMessages;
        }

        public BalancedFieldLengthOutput Calculate(BalancedFieldLengthCalculation calculation)
        {
            if (ThrowCreateKernelDataException)
            {
                throw new CreateKernelDataException("Exception");
            }

            if (ThrowKernelCalculationException)
            {
                throw new KernelCalculationException("Exception");
            }

            InputCalculation = calculation;
            return Output;
        }
    }
}