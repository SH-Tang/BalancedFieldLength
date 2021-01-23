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
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel.ValidationRules
{
    [TestFixture]
    public class ParameterRuleBaseTest
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Constructor_WithInvalidParameterName_ThrowsArgumentException(string invalidParameterName)
        {
            // Call 
            TestDelegate call = () => new TestParameterRuleBase(invalidParameterName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "Value cannot be null or whitespace.");
        }

        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            const string parameterName = "ParameterName";

            // Call
            var rule = new TestParameterRuleBase(parameterName);

            // Assert
            Assert.That(rule, Is.InstanceOf<IDataModelValidationRule>());
        }

        private class TestParameterRuleBase : ParameterRuleBase
        {
            public TestParameterRuleBase(string parameterName) : base(parameterName) {}
            public override ValidationRuleResult Execute()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}