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
using System.Linq;

namespace Core.Common.Data.DataModel
{
    /// <summary>
    /// Class which represents the validation result of the <see cref="DataModelValidator"/>.
    /// </summary>
    public class DataModelValidatorResult
    {
        /// <summary>
        /// Field representing a valid result.
        /// </summary>
        public static readonly DataModelValidatorResult ValidResult = 
            new DataModelValidatorResult(true, Enumerable.Empty<string>());

        private DataModelValidatorResult(bool isValid, IEnumerable<string> validationMessages)
        {
            IsValid = isValid;
            ValidationMessages = validationMessages;
        }

        /// <summary>
        /// Indicator whether the result is valid or not.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the collection of validation messages.
        /// </summary>
        public IEnumerable<string> ValidationMessages { get; }

        /// <summary>
        /// Creates a <see cref="DataModelValidatorResult"/> which represents an invalid result.
        /// </summary>
        /// <param name="messages">The collection of validation messages.</param>
        /// <returns>A configured <see cref="DataModelValidatorResult"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="messages"/> is <c>null</c>.</exception>
        internal static DataModelValidatorResult CreateInvalidResult(IEnumerable<string> messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            return new DataModelValidatorResult(false, messages);
        }
    }
}