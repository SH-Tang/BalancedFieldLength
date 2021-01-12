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

using System.Globalization;
using System.Windows.Controls;

namespace WPF.Core.ValidationRules
{
    /// <summary>
    /// Validation rule for determining whether the user input is a floating number.
    /// </summary>
    public class FloatingNumberValidationRule : ValidationRule
    {
        private const NumberStyles numberStyle = NumberStyles.AllowLeadingSign
                                                 | NumberStyles.AllowExponent
                                                 | NumberStyles.AllowDecimalPoint
                                                 | NumberStyles.AllowThousands;

        /// <summary>
        /// Gets or sets whether a string value consisting of whitespace is valid
        /// </summary>
        public bool IsWhitespaceStringValid { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = value as string;
            if (stringValue == null)
            {
                return new ValidationResult(false, $"{nameof(value)} must be a string.");
            }

            if (IsWhitespaceStringValid && string.IsNullOrWhiteSpace(stringValue))
            {
                return ValidationResult.ValidResult;
            }

            double parsedValue;
            string trimmedStringValue = stringValue.Trim();
            if (!double.TryParse(trimmedStringValue, numberStyle, CultureInfo.InvariantCulture, out parsedValue))
            {
                return new ValidationResult(false, $"{stringValue} could not be parsed as a floating number.");
            }

            return ValidationResult.ValidResult;
        }
    }
}