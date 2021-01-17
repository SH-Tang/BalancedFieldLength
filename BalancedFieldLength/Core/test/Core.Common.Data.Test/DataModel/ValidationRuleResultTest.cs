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
using Core.Common.Data.DataModel;
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel
{
    [TestFixture]
    public class ValidationRuleResultTest
    {
        [Test]
        public void CreateValidResult_Always_ReturnsExpectedValidationRuleResult()
        {
            // Call 
            ValidationRuleResult result = ValidationRuleResult.CreateValidResult();

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.ValidationMessage, Is.Empty);
        }

        [Test]
        public void CreateValidResult_Always_ReturnsSameInstance()
        {
            // Call
            ValidationRuleResult resultOne = ValidationRuleResult.CreateValidResult();
            ValidationRuleResult resultTwo = ValidationRuleResult.CreateValidResult();

            // Assert
            Assert.That(resultOne, Is.SameAs(resultTwo));
        }

        [Test]
        public void CreateInvalidResult_WithValidMessage_ReturnsExpectedValidationRuleResult()
        {
            // Setup
            const string message = "ValidationMessage";

            // Call 
            ValidationRuleResult result = ValidationRuleResult.CreateInvalidResult(message);

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ValidationMessage, Is.EqualTo(message));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase("  ")]
        public void CreateInvalidResult_WithInvalidMessage_ThrowsArgumentException(string invalidMessage)
        {
            // Call 
            TestDelegate call = () => ValidationRuleResult.CreateInvalidResult(invalidMessage);

            // Assert
            const string expectedMessage = "Value cannot be null or whitespace.";
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, expectedMessage);
        }
    }
}