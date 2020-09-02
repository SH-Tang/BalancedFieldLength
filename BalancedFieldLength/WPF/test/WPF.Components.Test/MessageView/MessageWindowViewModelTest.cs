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
using NUnit.Framework;
using WPF.Components.MessageView;
using WPF.Core;

namespace WPF.Components.Test.MessageView
{
    [TestFixture]
    public class MessageWindowViewModelTest
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
        public void GivenViewModelWithMessages_WhenAddingNewMessage_ThenMessageAddedAtHead()
        {
            // Given
            var messageOne = new MessageContext(MessageType.Warning, "Message1");

            var viewModel = new MessageWindowViewModel();
            viewModel.AddMessage(messageOne);

            // Precondition
            CollectionAssert.AreEqual(new[]
            {
                messageOne
            }, viewModel.Messages);

            var messageTwo = new MessageContext(MessageType.Warning, "Message2");

            // When 
            viewModel.AddMessage(messageTwo);

            // Then
            CollectionAssert.AreEqual(new[]
            {
                messageTwo,
                messageOne
            }, viewModel.Messages);
        }
    }
}