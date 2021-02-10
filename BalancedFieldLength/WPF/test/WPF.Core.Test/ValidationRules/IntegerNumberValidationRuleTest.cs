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

using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using NUnit.Framework;
using WPF.Core.ValidationRules;

namespace WPF.Core.Test.ValidationRules
{
    [TestFixture]
    public class IntegerNumberValidationRuleTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var rule = new IntegerNumberValidationRule();

            // Assert
            Assert.That(rule, Is.InstanceOf<ValidationRule>());
            Assert.That(rule.IsWhitespaceStringValid, Is.False);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidValidationObjects))]
        public void Validate_InvalidValidationObjects_ReturnsExpectedValidationResult(object validationObject)
        {
            // Setup
            var rule = new IntegerNumberValidationRule();

            // Call 
            ValidationResult result = rule.Validate(validationObject, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorContent, Is.EqualTo("value must be a string."));
        }

        [Test]
        [TestCaseSource(nameof(GetValidValidationValues))]
        public void Validate_ValidValidationValues_ReturnsValidResult(object validationObject)
        {
            // Setup
            var rule = new IntegerNumberValidationRule();

            // Call 
            ValidationResult result = rule.Validate(validationObject, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo(ValidationResult.ValidResult));
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidValidationValues))]
        public void Validate_InvalidValidationValues_ReturnsExpectedValidationResult(object validationObject)
        {
            // Setup
            var rule = new IntegerNumberValidationRule();

            // Call 
            ValidationResult result = rule.Validate(validationObject, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorContent, Is.EqualTo($"{validationObject} could not be parsed as an integer."));
        }

        [Test]
        [TestCaseSource(nameof(GetWhitespaceStrings))]
        public void GivenRuleWithIsWhitespaceStringValidTrue_WhenValidateWithEmptyOrWhiteSpaceString_ThenValidResultReturned(object validationObject)
        {
            // Given
            var rule = new IntegerNumberValidationRule
            {
                IsWhitespaceStringValid = true
            };

            // When 
            ValidationResult result = rule.Validate(validationObject, CultureInfo.InvariantCulture);

            // Then
            Assert.That(result, Is.EqualTo(ValidationResult.ValidResult));
        }

        [Test]
        [TestCaseSource(nameof(GetWhitespaceStrings))]
        public void GivenRuleWithIsWhitespaceStringValidFalse_WhenValidateWithEmptyOrWhiteSpaceString_TheExpectedValidationResult(object validationObject)
        {
            // Given
            var rule = new IntegerNumberValidationRule
            {
                IsWhitespaceStringValid = false
            };

            // When 
            ValidationResult result = rule.Validate(validationObject, CultureInfo.InvariantCulture);

            // Then
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorContent, Is.EqualTo($"{validationObject} could not be parsed as an integer."));
        }

        private static IEnumerable<TestCaseData> GetInvalidValidationObjects()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new object());
        }

        private static IEnumerable<TestCaseData> GetWhitespaceStrings()
        {
            yield return new TestCaseData(string.Empty);
            yield return new TestCaseData("   ");
            yield return new TestCaseData("                    ");
        }

        private static IEnumerable<TestCaseData> GetValidValidationValues()
        {
            yield return new TestCaseData("10");
            yield return new TestCaseData("1");
            yield return new TestCaseData("10    ");
            yield return new TestCaseData("    -10");
        }

        private static IEnumerable<TestCaseData> GetInvalidValidationValues()
        {
            yield return new TestCaseData("10.5.5");
            yield return new TestCaseData("1.1,0");
            yield return new TestCaseData("1.10");
            yield return new TestCaseData("1,10");
            yield return new TestCaseData("random string");
        }
    }
}