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

namespace Simulator.Kernel
{
    /// <summary>
    /// Class which holds the validation result after validating data that is used by the kernel.
    /// </summary>
    public class KernelValidationResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="KernelValidationResult"/>.
        /// </summary>
        /// <param name="isValid">Indicator whether the data was valid.</param>
        /// <param name="validationErrors">A collection containing the errors that occurred during the validation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validationErrors"/>
        /// is <c>null</c>.</exception>
        internal KernelValidationResult(bool isValid, IEnumerable<KernelValidationError> validationErrors)
        {
            ValidationErrors = validationErrors;
            IsValid = isValid;
        }

        /// <summary>
        /// Indicator whether the input is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the validation messages.
        /// </summary>
        public IEnumerable<KernelValidationError> ValidationErrors { get; }
    }
}