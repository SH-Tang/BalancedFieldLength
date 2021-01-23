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
    public class DoubleParameterGreaterThanRuleTest : DoubleParameterRuleBaseTestFixture<DoubleParameterGreaterThanRule>
    {
        [Test]
        [TestCase(double.Epsilon)]
        [TestCase(1)]
        public void Execute_WithValuesSatisfyingLimit_ReturnsValidResult(double offset)
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            double lowerLimit = random.NextDouble();

            var rule = new DoubleParameterGreaterThanRule(parameterName, lowerLimit + offset, lowerLimit);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result, Is.SameAs(ValidationRuleResult.ValidResult));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1e-5)]
        public void Execute_WithValuesNotSatisfyingLimit_ReturnsInvalidResult(double offset)
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            double lowerLimit = random.NextDouble();
            double invalidValue = lowerLimit + offset;

            var rule = new DoubleParameterGreaterThanRule(parameterName, invalidValue, lowerLimit);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result.IsValid, Is.False);
            var expectedMessage = $"{parameterName} must be > {lowerLimit}. Current value: {invalidValue}";
            Assert.That(result.ValidationMessage, Is.EqualTo(expectedMessage));
        }

        protected override DoubleParameterGreaterThanRule CreateRule(string parameterName, double value)
        {
            var random = new Random(21);
            return new DoubleParameterGreaterThanRule(parameterName, value, random.NextDouble());
        }
    }
}