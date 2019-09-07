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
using NUnit.Framework;
using WPF.Core.Converters;

namespace WPF.Core.Test.Converters
{
    [TestFixture]
    public class NaNToEmptyValueConverterTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var converter = new NaNToEmptyValueConverter();

            // Assert
            Assert.That(converter, Is.InstanceOf<IValueConverter>());
        }

        [Test]
        [SetCulture("en-US")]
        [TestCaseSource(nameof(GetConvertCases))]
        public void Convert_WithValidValueAndEnglishCultureAndTargetTypeIsString_ReturnsExpectedString(double value, string expectedValue)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            // Call 
            object result = converter.Convert(value, typeof(string), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        [SetCulture("nl-NL")]
        [TestCaseSource(nameof(GetConvertCases))]
        public void Convert_WithValidValueAndDutchCultureAndTargetTypeIsString_ReturnsExpectedString(double value, string expectedValue)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            // Call 
            object result = converter.Convert(value, typeof(string), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        [SetCulture("en-US")]
        [TestCaseSource(nameof(GetConvertCases))]
        public void Convert_WithValidValueAndEnglishCultureAndTargetTypeIsObject_ReturnsExpectedString(double value, string expectedValue)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            // Call 
            object result = converter.Convert(value, typeof(object), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        [SetCulture("nl-NL")]
        [TestCaseSource(nameof(GetConvertCases))]
        public void Convert_WithValidValueAndDutchCultureAndTargetTypeIsObject_ReturnsExpectedString(double value, string expectedValue)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            // Call 
            object result = converter.Convert(value, typeof(object), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCaseSource(nameof(GetNonDoubleValueObject))]
        public void Convert_WithUnsupportedValue_ThrowsNotSupportedException(object unsupportedValue)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

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
            double valueToConvert = random.NextDouble();

            var converter = new NaNToEmptyValueConverter();

            Type unsupportedType = typeof(int);

            // Call 
            TestDelegate call = () => converter.Convert(valueToConvert, unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCase("10.5    ", 10.5)]
        [TestCase("     10.5", 10.5)]
        [TestCase("+10.5", 10.5)]
        [TestCase("-10.5", -10.5)]
        [TestCase("", double.NaN)]
        [TestCase("NaN", double.NaN)]
        [TestCase("3.14", 3.14)]
        [TestCase("3,14.5", 314.5)]
        [TestCase("2.27e-5", 2.27e-5)]
        [TestCase("2.27e5", 2.27e5)]
        [TestCase("Infinity", double.PositiveInfinity)]
        [TestCase("-Infinity", double.NegativeInfinity)]
        public void ConvertBack_WithDoubleValueType_ReturnsExpectedResult(string value, double expectedResult)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            // Call 
            object result = converter.ConvertBack(value, typeof(double), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult).Within(1e-5));
        }

        [Test]
        public void ConvertBack_WithUnsupportedType_ThrowsNotSupportedException()
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            Type unsupportedType = typeof(object);

            // Call 
            TestDelegate call = () => converter.ConvertBack("1", unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCaseSource(nameof(GetNonDoubleValueObject))]
        public void ConvertBack_WithNonDoubleValueType_ReturnsBindingDoNothing(object value)
        {
            // Setup
            var converter = new NaNToEmptyValueConverter();

            // Call 
            object result = converter.ConvertBack(value, typeof(double), null, null);

            // Assert
            Assert.That(result, Is.EqualTo(Binding.DoNothing));
        }

        private static IEnumerable<TestCaseData> GetNonDoubleValueObject()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new object());
        }

        private static IEnumerable<TestCaseData> GetConvertCases()
        {
            yield return new TestCaseData(10.5, "10.5");
            yield return new TestCaseData(1.1, "1.1");
            yield return new TestCaseData(1.1e-2, "0.011");
            yield return new TestCaseData(1.1e-5, "1.1E-05");
            yield return new TestCaseData(1.1e+15, "1.1E+15");
            yield return new TestCaseData(1.1e2, "110");
            yield return new TestCaseData(+10, "10");
            yield return new TestCaseData(-10, "-10");
            yield return new TestCaseData(double.NaN, "");
            yield return new TestCaseData(double.PositiveInfinity, "Infinity");
            yield return new TestCaseData(double.NegativeInfinity, "-Infinity");
        }
    }
}