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
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using Simulator.Calculator.AggregatedDistanceCalculator;

namespace Application.BalancedFieldLength.KernelWrapper
{
    /// <summary>
    /// Interface describing a module to validate and calculate
    /// a balanced field length calculation.
    /// </summary>
    public interface IBalancedFieldLengthCalculationModule
    {
        /// <summary>
        /// Validates a <see cref="BalancedFieldLengthCalculation"/>.
        /// </summary>
        /// <param name="calculation">The calculation to validate.</param>
        /// <returns>A collection of validation messages if the validation failed, empty otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculation"/>
        /// is <c>null</c>.</exception>
        IEnumerable<string> Validate(BalancedFieldLengthCalculation calculation);

        /// <summary>
        /// Calculates the <see cref="BalancedFieldLengthOutput"/> based on <paramref name="calculation"/>.
        /// </summary>
        /// <param name="calculation">The <see cref="BalancedFieldLengthCalculation"/> to calculate for.</param>
        /// <returns>A <see cref="BalancedFieldLengthOutput"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculation"/> is <c>null</c>.</exception>
        /// <exception cref="CreateKernelDataException">Thrown when the calculation input
        /// could not be created for the kernel.</exception>
        /// <exception cref="KernelCalculationException">Thrown when <see cref="AggregatedDistanceOutput"/>
        /// could not be calculated.</exception>
        BalancedFieldLengthOutput Calculate(BalancedFieldLengthCalculation calculation);
    }
}