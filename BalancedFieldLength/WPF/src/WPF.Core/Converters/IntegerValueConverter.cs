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
using System.Globalization;
using System.Windows.Data;

namespace WPF.Core.Converters
{
    /// <summary>
    /// Value converter to convert integers.
    /// </summary>
    public class IntegerValueConverter : IValueConverter
    {
        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Thrown when:
        /// <list type="bullet">
        /// <item>The <paramref name="value"/> is not of type <see cref="int"/>.</item>
        /// <item>the <paramref name="targetType"/> is not of type <see cref="string"/>.</item>
        /// </list></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new NotSupportedException($"Conversion to {targetType.Name} is not supported.");
            }

            if (!(value is int))
            {
                throw new NotSupportedException($"Conversion from {value?.GetType().Name} is not supported.");
            }

            return value?.ToString();
        }

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="targetType"/>
        /// is not of type <see cref="int"/>.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(int))
            {
                throw new NotSupportedException($"Conversion to {targetType.Name} is not supported.");
            }

            string integerValue = value as string;
            int parsedResult;
            if (integerValue != null && int.TryParse(integerValue, out parsedResult))
            {
                return parsedResult;
            }

            return null;
        }
    }
}