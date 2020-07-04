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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NSubstitute;
using NUnit.Framework;

namespace WPF.Components.Test
{
    [TestFixture]
    public class TextBoxMoveFocusOnModificationTest
    {
        [Test]
        public void Create_TextBoxNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => TextBoxMoveFocusOnModification.Create(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("textBox"));
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void GivenViewWithNextElementInHost_WhenReturnKeyPressed_ThenFocusMovedToNextElement()
        {
            // Given
            var textBox = new TextBox();

            TextBoxMoveFocusOnModification.Create(textBox);

            var upperTextBox = new TextBox();
            var lowerTextBox = new TextBox();
            new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        upperTextBox,
                        textBox,
                        lowerTextBox
                    }
                }
            }.Show();
            textBox.Focus();

            // Precondition
            Assert.That(upperTextBox.IsFocused, Is.False);
            Assert.That(textBox.IsFocused, Is.True);
            Assert.That(lowerTextBox.IsFocused, Is.False);

            // When 
            textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice,
                                                Substitute.For<PresentationSource>(),
                                                0, Key.Return)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            });

            // Then
            Assert.That(upperTextBox.IsFocused, Is.False);
            Assert.That(textBox.IsFocused, Is.False);
            Assert.That(lowerTextBox.IsFocused, Is.True);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void GivenViewWithNoOtherElementInHost_WhenReturnKeyPressed_ThenFocusNotMoved()
        {
            // Given
            var textBox = new TextBox();

            TextBoxMoveFocusOnModification.Create(textBox);

            new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        textBox
                    }
                }
            }.Show();
            textBox.Focus();

            // Precondition
            Assert.That(textBox.IsFocused, Is.True);

            // When 
            textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice,
                                                Substitute.For<PresentationSource>(),
                                                0, Key.Return)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            });

            // Then
            Assert.That(textBox.IsFocused, Is.True);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void GivenViewWithOtherElementInHost_WhenReturnKeyPressed_ThenFocusNotMoved()
        {
            // Given
            var textBox = new TextBox();

            TextBoxMoveFocusOnModification.Create(textBox);

            var upperTextBox = new TextBox();
            new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        upperTextBox,
                        textBox
                    }
                }
            }.Show();
            textBox.Focus();

            // Precondition
            Assert.That(upperTextBox.IsFocused, Is.False);
            Assert.That(textBox.IsFocused, Is.True);

            // When 
            textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice,
                                                Substitute.For<PresentationSource>(),
                                                0, Key.Return)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            });

            // Then
            Assert.That(textBox.IsFocused, Is.False);
            Assert.That(upperTextBox.IsFocused, Is.True);
        }
    }
}