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

using System.ComponentModel;
using NUnit.Framework;

namespace WPF.Core.Test
{
    [TestFixture]
    public class ViewModelBaseTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new TestViewModelBase();

            // Assert
            Assert.That(viewModel, Is.InstanceOf<INotifyPropertyChanged>());
        }

        [Test]
        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("PropertyName")]
        public void OnPropertyChanged_WithValidArguments_RaisesPropertyChangedEvent(string propertyName)
        {
            // Setup
            var viewModel = new TestViewModelBase();

            PropertyChangedEventArgs eventArgs = null;
            bool propertyChangedTriggered = false;
            viewModel.PropertyChanged += (o, e) =>
            {
                eventArgs = e;
                propertyChangedTriggered = true;
            };

            // Call 
            viewModel.PublicOnPropertyChanged(propertyName);

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(propertyName));
        }

        private class TestViewModelBase : ViewModelBase
        {
            public void PublicOnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(propertyName);
            }
        }
    }
}