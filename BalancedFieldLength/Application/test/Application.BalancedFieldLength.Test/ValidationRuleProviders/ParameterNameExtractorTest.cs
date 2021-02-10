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
using Application.BalancedFieldLength.ValidationRuleProviders;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.ValidationRuleProviders
{
    [TestFixture]
    public class ParameterNameExtractorTest
    {
        [Test]
        public void ExtractParameterName_ValueNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => ParameterNameExtractor.ExtractParameterName(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("value"));
        }

        [Test]
        [TestCaseSource(nameof(GetValues))]
        public void ExtractParameterName_WithValues_ReturnsExpectedValues(string value, string expectedValue)
        {
            // Call 
            string extractedName = ParameterNameExtractor.ExtractParameterName(value);

            // Assert
            Assert.That(extractedName, Is.EqualTo(expectedValue));
        }

        private static IEnumerable<TestCaseData> GetValues()
        {
            yield return new TestCaseData("ParameterName", "ParameterName");
            yield return new TestCaseData("Parameter Name     ", "Parameter Name");
            yield return new TestCaseData("    Parameter Name", "    Parameter Name");
            yield return new TestCaseData(string.Empty, string.Empty);
            yield return new TestCaseData("    ", string.Empty);
            yield return new TestCaseData("ParameterName [unit]", "ParameterName");
            yield return new TestCaseData("ParameterName [unit", "ParameterName");
            yield return new TestCaseData("ParameterName unit]", "ParameterName unit");
            yield return new TestCaseData("Parameter Name [unit]", "Parameter Name");
        }
    }
}