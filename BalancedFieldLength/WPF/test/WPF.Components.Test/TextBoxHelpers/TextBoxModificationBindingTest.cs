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
using WPF.Components.TextBoxHelpers;

namespace WPF.Components.Test.TextBoxHelpers
{
    [TestFixture]
    public class TextBoxModificationBindingTest
    {
        [Test]
        public void Create_TextBoxNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => TextBoxModificationBinding.Create(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("textBox"));
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void GivenTextBoxWithBinding_WhenReturnKeyPressed_ThenValueSetToBinding()
        {
            // Given
            var textBoxDataContext = new TestBindingSource();

            var textBox = new TextBox();
            textBox.DataContext = textBoxDataContext;
            textBox.SetBinding(TextBox.TextProperty, nameof(TestBindingSource.TestProperty));

            TextBoxModificationBinding.Create(textBox);

            // When 
            textBox.RaiseEvent(new KeyEventArgs(Keyboard.PrimaryDevice,
                                                Substitute.For<PresentationSource>(),
                                                0, Key.Return)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            });

            // Then
            Assert.That(textBoxDataContext.SetCount, Is.EqualTo(1));
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void GivenTextBoxWithBinding_WhenFocusLost_ThenValueSetToBinding()
        {
            // Given
            var textBoxDataContext = new TestBindingSource();

            var textBox = new TextBox();
            textBox.DataContext = textBoxDataContext;
            textBox.SetBinding(TextBox.TextProperty, nameof(TestBindingSource.TestProperty));

            TextBoxModificationBinding.Create(textBox);

            var otherTextBox = new TextBox();
            new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        textBox,
                        otherTextBox
                    }
                }
            }.Show();

            // When 
            textBox.Focus();
            otherTextBox.Focus();

            // Then
            Assert.That(textBoxDataContext.SetCount, Is.EqualTo(1));
        }

        private class TestBindingSource
        {
            public string TestProperty
            {
                get
                {
                    return string.Empty;
                }
                set
                {
                    SetCount++;
                }
            }

            public int SetCount { get; private set; }
        }
    }
}