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
using Core.Common.Data.DataModel.ValidationRules;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel.ValidationRules
{
    [TestFixture]
    public class IntegerParameterGreaterThanRuleTest
    {
        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            int lowerLimit = random.Next();
            int value = lowerLimit + 1;

            // Call
            var rule = new IntegerParameterGreaterThanRule(parameterName, value, lowerLimit);

            // Assert
            Assert.That(rule, Is.InstanceOf<ParameterRuleBase>());
        }

        [Test]
        [TestCase(1)]
        public void Execute_WithValuesSatisfyingLimit_ReturnsValidResult(int offset)
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            int lowerLimit = random.Next();

            var rule = new IntegerParameterGreaterThanRule(parameterName, lowerLimit + offset, lowerLimit);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result, Is.SameAs(ValidationRuleResult.ValidResult));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Execute_WithValuesNotSatisfyingLimit_ReturnsInvalidResult(int offset)
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            int lowerLimit = random.Next();
            int invalidValue = lowerLimit + offset;

            var rule = new IntegerParameterGreaterThanRule(parameterName, invalidValue, lowerLimit);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result.IsValid, Is.False);
            var expectedMessage = $"{parameterName} must be > {lowerLimit}. Current value: {invalidValue}";
            Assert.That(result.ValidationMessage, Is.EqualTo(expectedMessage));
        }
    }
}