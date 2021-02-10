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
using System.Linq;
using Core.Common.Data.DataModel;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel
{
    [TestFixture]
    public class DataModelValidatorResultTest
    {
        [Test]
        public void ValidResult_Always_ReturnsExpectedValidationRuleResult()
        {
            // Call 
            DataModelValidatorResult result = DataModelValidatorResult.ValidResult;

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.ValidationMessages, Is.Empty);
        }


        [Test]
        public void CreateInvalidResult_MessagesNull_ThrowsArgumentNullException()
        {
            // Call 
            TestDelegate call = () => DataModelValidatorResult.CreateInvalidResult(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("messages"));
        }

        [Test]
        public void CreateInvalidResult_WithValidMessage_ReturnsExpectedValidationRuleResult()
        {
            // Setup
            IEnumerable<string> messages = Enumerable.Empty<string>();

            // Call 
            var result = DataModelValidatorResult.CreateInvalidResult(messages);

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ValidationMessages, Is.SameAs(messages));
        }
    }
}