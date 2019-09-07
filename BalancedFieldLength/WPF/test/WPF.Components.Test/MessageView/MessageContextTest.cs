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

namespace WPF.Components.Test.MessageView
{
    [TestFixture]
    public class MessageContextTest
    {
        [Test]
        public void Constructor_MessageNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new MessageContext(MessageType.Error, null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("message"));
        }

        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            const string message = "just a message";
            const MessageType messageType = MessageType.Error;

            // Call
            var context = new MessageContext(messageType, message);

            // Assert
            Assert.That(context.Message, Is.EqualTo(message));
            Assert.That(context.MessageType, Is.EqualTo(messageType));
        }
    }
}