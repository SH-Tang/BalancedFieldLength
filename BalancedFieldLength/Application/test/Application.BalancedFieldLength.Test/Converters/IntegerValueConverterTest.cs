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
using System.Windows.Data;
using Application.BalancedFieldLength.Converters;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Converters
{
    [TestFixture]
    public class IntegerValueConverterTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var converter = new IntegerValueConverter();

            // Assert
            Assert.That(converter, Is.InstanceOf<IValueConverter>());
        }

        [Test]
        [TestCase(10, "10")]
        [TestCase(1, "1")]
        public void Convert_WithValidValue_ReturnsExpectedString(int? value, string expectedValue)
        {
            // Setup
            var converter = new IntegerValueConverter();

            // Call 
            object result = converter.Convert(value, typeof(string), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCaseSource(nameof(GetNonIntegerValueObject))]
        public void Convert_WithUnsupportedValue_ThrowsNotSupportedException(object unsupportedValue)
        {
            // Setup
            var converter = new IntegerValueConverter();

            // Call 
            TestDelegate call = () => converter.Convert(unsupportedValue, typeof(string), null, null);

            // Assert
            string expectedMessage = $"Conversion from {unsupportedValue?.GetType().Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        public void Convert_WithUnsupportedType_ThrowsNotSupportedException()
        {
            // Setup
            var random = new Random(21);
            int valueToConvert = random.Next();

            var converter = new IntegerValueConverter();

            Type unsupportedType = typeof(object);

            // Call 
            TestDelegate call = () => converter.Convert(valueToConvert, unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCase("10", 10)]
        [TestCase("1", 1)]
        public void ConvertBack_WithIntegerValueType_ReturnsExpectedResult(string value, int expectedResult)
        {
            // Setup
            var converter = new IntegerValueConverter();

            // Call 
            object result = converter.ConvertBack(value, typeof(int), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ConvertBack_WithUnsupportedType_ThrowsNotSupportedException()
        {
            // Setup
            var converter = new IntegerValueConverter();

            Type unsupportedType = typeof(object);

            // Call 
            TestDelegate call = () => converter.ConvertBack("1", unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCaseSource(nameof(GetNonIntegerValueObject))]
        public void ConvertBack_WithNonIntegerValueType_ReturnsBindingDoNothing(object value)
        {
            // Setup
            var converter = new IntegerValueConverter();

            // Call 
            object result = converter.ConvertBack(value, typeof(int), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(Binding.DoNothing));
        }

        private static IEnumerable<TestCaseData> GetNonIntegerValueObject()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new object());
        }
    }
}