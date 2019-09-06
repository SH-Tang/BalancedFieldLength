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
using System.Collections.Generic;
using WPF.Core;

namespace Application.BalancedFieldLength.Controls
{
    /// <summary>
    /// View model to display messages.
    /// </summary>
    public class MessageWindowViewModel : ViewModelBase
    {
        private readonly List<MessageContext> messages;

        /// <summary>
        /// Creates a new instance of <see cref="MessageWindowViewModel"/>.
        /// </summary>
        public MessageWindowViewModel()
        {
            messages = new List<MessageContext>();
        }

        /// <summary>
        /// Gets the collection of messages.
        /// </summary>
        public IEnumerable<MessageContext> Messages
        {
            get
            {
                return messages;
            }
        }

        /// <summary>
        /// Adds a message to the message window.
        /// </summary>
        /// <param name="message">The <see cref="MessageContext"/> to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/>
        /// is <c>null</c>.</exception>
        public void AddMessage(MessageContext message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            messages.Add(message);
            OnPropertyChanged(nameof(Messages));
        }
    }
}