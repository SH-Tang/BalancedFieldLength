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

namespace Application.BalancedFieldLength.ValidationRuleProviders
{
    /// <summary>
    /// Helper class to extract parameter names.
    /// </summary>
    public static class ParameterNameExtractor
    {
        private static readonly char[] splitOptions = new[]
        {
            '[',
            ']'
        };

        /// <summary>
        /// Extract a parameter name from its string argument.
        /// </summary>
        /// <param name="value">The string to extract a parameter name from.</param>
        /// <returns>A parameter name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is empty.</exception>
        public static string ExtractParameterName(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            string[] substrings = value.Split(splitOptions);
            return substrings[0].TrimEnd();
        }
    }
}