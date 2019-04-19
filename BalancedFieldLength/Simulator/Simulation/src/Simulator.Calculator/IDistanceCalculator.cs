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

using Simulator.Data.Exceptions;

namespace Simulator.Calculator
{
    /// <summary>
    /// Interface describing the calculator for calculating the traversed distance.
    /// </summary>
    public interface IDistanceCalculator
    {
        /// <summary>
        /// Calculates the distance which is needed before the aircraft reaches either the screen height
        /// or comes to a standstill.
        /// </summary>
        /// <returns>The <see cref="DistanceCalculatorOutput"/> with the calculated result.</returns>
        /// <exception cref="CalculatorException">Thrown when the calculator cannot calculate the covered distance.</exception>
        DistanceCalculatorOutput Calculate();
    }
}