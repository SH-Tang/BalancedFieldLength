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

namespace Application.BalancedFieldLength.KernelWrapper
{
    /// <summary>
    /// Interface for describing the creation of calculation modules to perform calculations with.
    /// </summary>
    public interface IBalancedFieldLengthCalculationModuleFactory
    {
        /// <summary>
        /// Creates a calculation module to perform the balanced field length calculation with.
        /// </summary>
        /// <returns>A new <see cref="IBalancedFieldLengthCalculationModule"/>.</returns>
        IBalancedFieldLengthCalculationModule CreateModule();
    }
}