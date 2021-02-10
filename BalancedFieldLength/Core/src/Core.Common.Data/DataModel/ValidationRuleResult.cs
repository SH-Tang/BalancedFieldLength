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

namespace Core.Common.Data.DataModel
{
    /// <summary>
    /// Class which represents the validation result of a validation rule.
    /// </summary>
    public class ValidationRuleResult
    {
        /// <summary>
        /// Field representing a valid result.
        /// </summary>
        public static readonly ValidationRuleResult ValidResult = 
            new ValidationRuleResult(true, string.Empty);

        private ValidationRuleResult(bool isValid, string validationMessage)
        {
            IsValid = isValid;
            ValidationMessage = validationMessage;
        }

        /// <summary>
        /// Gets the indicator whether the validation was valid or not.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        public string ValidationMessage { get; }

        /// <summary>
        /// Creates a <see cref="ValidationRuleResult"/> which represents an invalid result.
        /// </summary>
        /// <param name="message">The message which caused the result to be invalid.</param>
        /// <returns>A configured <see cref="ValidationRuleResult"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="message"/>
        /// is <c>null</c>, empty or consists of whitespaces.</exception>
        public static ValidationRuleResult CreateInvalidResult(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            return new ValidationRuleResult(false, message);
        }
    }
}