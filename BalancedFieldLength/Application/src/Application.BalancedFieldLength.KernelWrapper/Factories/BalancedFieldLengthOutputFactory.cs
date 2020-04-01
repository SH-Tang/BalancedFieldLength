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
using Simulator.Calculator.BalancedFieldLengthCalculator;
using KernelBalancedFieldLengthOutput = Simulator.Calculator.BalancedFieldLengthCalculator.BalancedFieldLength;

namespace Application.BalancedFieldLength.KernelWrapper.Factories
{
    /// <summary>
    /// Factory for creating <see cref="BalancedFieldLengthOutput"/>.
    /// </summary>
    public static class BalancedFieldLengthOutputFactory
    {
        /// <summary>
        /// Creates a <see cref="BalancedFieldLengthOutput"/> based on <see cref="AggregatedDistanceOutput"/>.
        /// </summary>
        /// <param name="outputs">The collection of <see cref="AggregatedDistanceOutput"/>
        /// to create the output for.</param>
        /// <returns>A <see cref="BalancedFieldLengthOutput"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="outputs"/> is <c>null</c>.</exception>
        /// <exception cref="KernelCalculationException">Thrown when <see cref="BalancedFieldLengthOutput"/>
        /// could not be calculated.</exception>
        public static BalancedFieldLengthOutput Create(IEnumerable<AggregatedDistanceOutput> outputs)
        {
            if (outputs == null)
            {
                throw new ArgumentNullException(nameof(outputs));
            }

            try
            {
                KernelBalancedFieldLengthOutput output = BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);
                return new BalancedFieldLengthOutput(output.Velocity, output.Distance);
            }
            catch (ArgumentException e)
            {
                throw new KernelCalculationException(e.Message, e);
            }
        }
    }
}