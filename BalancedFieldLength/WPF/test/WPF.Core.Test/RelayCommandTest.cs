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
using NUnit.Framework;

namespace WPF.Core.Test
{
    [TestFixture]
    public class RelayCommandTest
    {
        [Test]
        public void Constructor_ExecuteActionNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new RelayCommand(null);

            // Assert
            Assert.That(call, Throws.TypeOf<ArgumentNullException>()
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("executeAction"));
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var command = new RelayCommand(() => {}, null);

            // Assert
            Assert.That(command, Is.InstanceOf<ICommand>());
        }

        [Test]
        public void Execute_WithoutCanExecuteFunction_InvokesExecuteAction()
        {
            // Setup
            bool isExecuted = false;
            var command = new RelayCommand(() => isExecuted = true);

            // Call
            command.Execute(null);

            // Assert
            Assert.That(isExecuted, Is.True);
        }

        [Test]
        public void CanExecute_WithoutCanExecuteFunction_InvokesExecuteAction()
        {
            // Setup
            var command = new RelayCommand(() => {});

            // Call
            bool canExecute = command.CanExecute(null);

            // Assert
            Assert.That(canExecute, Is.True);
        }

        [Test]
        public void ConstructorWithCanExecuteFunction_ExecuteActionNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new RelayCommand(null, null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("executeAction", exception.ParamName);
        }

        [Test]
        public void Execute_WithCanExecuteFunction_InvokesExecuteAction()
        {
            // Setup
            bool isExecuted = false;
            var command = new RelayCommand(() => isExecuted = true, null);

            // Call
            command.Execute(null);

            // Assert
            Assert.That(isExecuted, Is.True);
        }

        [Test]
        public void CanExecute_WithCanExecuteFunction_InvokesExecuteAction()
        {
            // Setup
            const bool canExecuteResult = true;
            bool isCanExecuteFunc = false;
            var command = new RelayCommand(() => {}, () =>
            {
                isCanExecuteFunc = true;
                return canExecuteResult;
            });

            // Call
            bool canExecute = command.CanExecute(null);

            // Assert
            Assert.That(canExecute, Is.EqualTo(canExecuteResult));
            Assert.That(isCanExecuteFunc, Is.True);
        }
    }
}