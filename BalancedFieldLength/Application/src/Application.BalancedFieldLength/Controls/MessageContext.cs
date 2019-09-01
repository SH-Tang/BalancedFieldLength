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

namespace Application.BalancedFieldLength.Controls
{
    /// <summary>
    /// A class containing the context of which a message was generated.
    /// </summary>
    public class MessageContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="MessageContext"/>.
        /// </summary>
        /// <param name="messageType">The <see cref="Controls.MessageType"/>.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/>
        /// is <c>null</c>.</exception>
        public MessageContext(MessageType messageType, string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            MessageType = messageType;
            Message = message;
        }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageType MessageType { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }
    }
}