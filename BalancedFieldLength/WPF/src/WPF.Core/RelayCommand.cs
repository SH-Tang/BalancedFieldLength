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
using System.Windows.Input;

namespace WPF.Core
{
    /// <summary>
    /// Realization of <see cref="ICommand"/>.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action executeAction;
        private readonly Func<bool> canExecuteFunc;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="executeAction">The action to execute when <see cref="Execute"/>
        /// is invoked.</param>
        /// <param name="canExecuteFunc">The function which determines whether the command
        /// can be executed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="executeAction"/>
        /// is <c>null</c>.</exception>
        public RelayCommand(Action executeAction, Func<bool> canExecuteFunc)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException(nameof(executeAction));
            }

            this.executeAction = executeAction;
            this.canExecuteFunc = canExecuteFunc;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="executeAction">The action to execute when <see cref="Execute"/>
        /// is invoked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="executeAction"/>
        /// is <c>null</c>.</exception>
        public RelayCommand(Action executeAction) : this(executeAction, null) {}

        public bool CanExecute(object parameter)
        {
            if (canExecuteFunc == null)
            {
                return true;
            }

            return canExecuteFunc();
        }

        public void Execute(object parameter)
        {
            executeAction();
        }
    }
}