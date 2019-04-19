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

namespace Simulator.Kernel
{
    /// <summary>
    /// Enum representing the kernel validation errors.
    /// </summary>
    public enum KernelValidationError
    {
        /// <summary>
        /// Represents an invalid density.
        /// </summary>
        InvalidDensity,

        /// <summary>
        /// Represents an invalid gravitational acceleration.
        /// </summary>
        InvalidGravitationalAcceleration,

        /// <summary>
        /// Indicates an invalid number of failed engines
        /// </summary>
        InvalidNrOfFailedEngines
    }
}