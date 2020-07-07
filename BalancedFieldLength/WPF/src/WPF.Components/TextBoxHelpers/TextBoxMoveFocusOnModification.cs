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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Components.TextBoxHelpers
{
    /// <summary>
    /// Helper class which moves the focus of a text box to its next control element after hitting the return key.
    /// </summary>
    public static class TextBoxMoveFocusOnModification
    {
        /// <summary>
        /// Creates a move focus behaviour on the input argument after the return key was pressed.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> to move the focus for for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="textBox" />
        /// is <c>null</c>.</exception>
        public static void Create(TextBox textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException(nameof(textBox));
            }

            textBox.KeyDown += (sender, e) => OnKeyDown(e, textBox);
        }

        private static void OnKeyDown(KeyEventArgs e, UIElement textBox)
        {
            if (e.Key.Equals(Key.Return))
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}