﻿// Copyright (C) 2018 Dennis Tang. All rights reserved.
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
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.Views.OutputView;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Views.OutputView
{
    [TestFixture]
    public class OutputViewModelTest
    {
        [Test]
        public void Constructor_OutputNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new OutputViewModel(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("output"));
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            var output = new BalancedFieldLengthOutput(random.NextDouble(),
                                                       random.NextDouble());

            // Call
            var viewModel = new OutputViewModel(output);

            // Assert
            Assert.That(viewModel.BalancedFieldLengthDistance, Is.EqualTo(output.Distance));
            Assert.That(viewModel.BalancedFieldLengthVelocity, Is.EqualTo(output.Velocity));
        }
    }
}