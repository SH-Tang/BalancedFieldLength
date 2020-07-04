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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WPF.Components
{
    /// <summary>
    /// Helper class which commits values of a text box after losing focus or hitting the return key.
    /// </summary>
    public static class TextBoxModificationBinding
    {
        /// <summary>
        /// Creates a modification binding on the input argument to commit values after losing
        /// focus or hitting the return key.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> to create the binding for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="textBox" />
        /// is <c>null</c>.</exception>
        public static void Create(TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException(nameof(textBox));
            }

            BindingExpression textBoxBinding = textBox.GetBindingExpression(TextBox.TextProperty);
            textBox.KeyDown += (sender, e) => OnKeyDown(e, textBoxBinding);
            textBox.LostFocus += (sender, e) => OnLostFocus(textBoxBinding);
        }

        private static void OnKeyDown(KeyEventArgs e, BindingExpressionBase binding)
        {
            if (e.Key.Equals(Key.Return))
            {
                binding.UpdateSource();
            }
        }

        private static void OnLostFocus(BindingExpressionBase binding)
        {
            binding.UpdateSource();
        }
    }
}