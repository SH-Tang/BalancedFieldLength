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
using Application.BalancedFieldLength.Controls;
using Application.BalancedFieldLength.WPFCommon;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Controls
{
    [TestFixture]
    public class MessageViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new MessageWindowViewModel();

            // Assert
            Assert.That(viewModel, Is.InstanceOf<ViewModelBase>());
            Assert.That(viewModel.Messages, Is.Not.Null.And.Empty);
        }

        [Test]
        public void AddMessage_MessageNull_ThrowsArgumentNullException()
        {
            // Setup
            var viewModel = new MessageWindowViewModel();

            // Call 
            TestDelegate call = () => viewModel.AddMessage(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("message"));
        }

        [Test]
        public void AddMessage_MessageValid_AddsMessageToCollectionAndNotifyPropertyChanged()
        {
            // Setup
            var viewModel = new MessageWindowViewModel();

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            viewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var message = new MessageContext(MessageType.Warning, string.Empty);

            // Call 
            viewModel.AddMessage(message);

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(viewModel.Messages)));
        }
    }
}