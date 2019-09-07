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

namespace WPF.Components.MessageView
{
    /// <summary>
    /// Enum representing the type of message.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Represents an information message.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Represents a warning message.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Represents an error message.
        /// </summary>
        Error = 3
    }
}