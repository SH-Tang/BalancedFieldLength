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
    public class DoubleParameterConcreteNumberRuleTest : DoubleParameterRuleBaseTestFixture<DoubleParameterConcreteNumberRule>
    {
        [Test]
        public void Execute_WithValidValue_ReturnsValidResult()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            double value = random.NextDouble();

            var rule = new DoubleParameterConcreteNumberRule(parameterName, value);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result, Is.SameAs(ValidationRuleResult.ValidResult));
        }

        protected override DoubleParameterConcreteNumberRule CreateRule(string parameterName, double value)
        {
            return new DoubleParameterConcreteNumberRule(parameterName, value);
        }
    }
}