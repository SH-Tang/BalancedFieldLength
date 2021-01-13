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
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF.Components.MessageView
{
    /// <summary>
    /// Converter which converts the <see cref="MessageType"/>.
    /// </summary>
    public class MessageTypeConverter : IValueConverter
    {
        /// <inheritdoc/>
        /// <exception cref="InvalidEnumArgumentException">Thrown when <paramref name="value"/>
        /// is an invalid <see cref="MessageType"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when:
        /// <list type="bullet">
        /// <item>The <paramref name="value"/> is not of type <see cref="MessageType"/>.</item>
        /// <item>The <paramref name="value"/> is a valid <see cref="MessageType"/> but not supported.</item>
        /// <item>the <paramref name="targetType"/> is not of type <see cref="Brush"/>
        /// or of type <see cref="object"/>.</item>
        /// </list></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MessageType))
            {
                throw new NotSupportedException($"Conversion from {value?.GetType().Name} is not supported.");
            }

            if (targetType != typeof(Brush) && targetType != typeof(object))
            {
                throw new NotSupportedException($"Conversion to {targetType.Name} is not supported.");
            }

            var valueToConvert = (MessageType) value;
            if (!Enum.IsDefined(typeof(MessageType), valueToConvert))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int) valueToConvert, typeof(MessageType));
            }

            switch (valueToConvert)
            {
                case MessageType.Info:
                    return Brushes.CornflowerBlue;
                case MessageType.Error:
                    return Brushes.Crimson;
                case MessageType.Warning:
                    return Brushes.Gold;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Thrown when function is called.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack operation is not supported.");
        }
    }
}