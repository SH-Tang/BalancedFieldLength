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
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;
using WPF.Core.Converters;

namespace WPF.Core.Test.Converters
{
    [TestFixture]
    public class AngleValueConverterTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var converter = new AngleValueConverter();

            // Assert
            Assert.That(converter, Is.InstanceOf<IValueConverter>());
            Assert.That(converter.IsDegrees, Is.False);
        }

        [Test]
        [TestCaseSource(nameof(GetNonAngleValueObject))]
        public void Convert_WithUnsupportedValue_ThrowsNotSupportedException(object unsupportedValue)
        {
            // Setup
            var converter = new AngleValueConverter();

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
            Angle valueToConvert = random.NextAngle();

            var converter = new AngleValueConverter();

            Type unsupportedType = typeof(int);

            // Call 
            TestDelegate call = () => converter.Convert(valueToConvert, unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCaseSource(nameof(GetConvertBackTestCases))]
        public void ConvertBackIsDegreesFalse_WithAngleValueType_ReturnsExpectedResult(string value, double expectedResult)
        {
            // Setup
            var converter = new AngleValueConverter();

            // Call 
            object result = converter.ConvertBack(value, typeof(Angle), null, null);

            // Assert
            var convertedResult = (Angle) result;
            Assert.That(convertedResult.Radians, Is.EqualTo(expectedResult).Within(1e-5));
        }

        [Test]
        [TestCaseSource(nameof(GetConvertBackTestCases))]
        public void ConvertBackIsDegreesTrue_WithAngleValueType_ReturnsExpectedResult(string value, double expectedResult)
        {
            // Setup
            var converter = new AngleValueConverter
            {
                IsDegrees = true
            };

            // Call 
            object result = converter.ConvertBack(value, typeof(Angle), null, null);

            // Assert
            var convertedResult = (Angle) result;
            Assert.That(convertedResult.Degrees, Is.EqualTo(expectedResult).Within(1e-5));
        }

        [Test]
        public void ConvertBack_WithUnsupportedType_ThrowsNotSupportedException()
        {
            // Setup
            var converter = new AngleValueConverter();
            Type unsupportedType = typeof(object);

            // Call 
            TestDelegate call = () => converter.ConvertBack("1", unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCaseSource(nameof(GetNonAngleValueObject))]
        public void ConvertBack_WithNonDoubleValueType_ReturnsNull(object value)
        {
            // Setup
            var converter = new AngleValueConverter();

            // Call 
            object result = converter.ConvertBack(value, typeof(Angle), null, null);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestFixture]
        public class TestConvertToString : AngleConvertTestBase
        {
            public TestConvertToString() : base(typeof(string)) {}
        }

        [TestFixture]
        public class TestConvertToObject : AngleConvertTestBase
        {
            public TestConvertToObject() : base(typeof(object)) {}
        }

        private static IEnumerable<TestCaseData> GetNonAngleValueObject()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new object());
        }

        private static IEnumerable<TestCaseData> GetConvertBackTestCases()
        {
            yield return new TestCaseData("10.5    ", 10.5);
            yield return new TestCaseData("     10.5", 10.5);
            yield return new TestCaseData("+10.5", 10.5);
            yield return new TestCaseData("-10.5", -10.5);
            yield return new TestCaseData("", double.NaN);
            yield return new TestCaseData("NaN", double.NaN);
            yield return new TestCaseData("3.14", 3.14);
            yield return new TestCaseData("3,14.5", 314.5);
            yield return new TestCaseData("2.27e-5", 2.27e-5);
            yield return new TestCaseData("2.27e5", 2.27e5);
            yield return new TestCaseData("Infinity", double.PositiveInfinity);
            yield return new TestCaseData("-Infinity", double.NegativeInfinity);
        }

        /// <summary>
        /// Base class to test the convert method from the <see cref="AngleValueConverter"/>.
        /// </summary>
        [TestFixture]
        public abstract class AngleConvertTestBase
        {
            private readonly Type targetType;

            [Test]
            [SetCulture("en-US")]
            [TestCaseSource(typeof(AngleConvertTestBase), nameof(GetConvertCases))]
            public void ConvertIsDegreesFalse_WithValidValueAndEnglishCulture_ReturnsExpectedTargetValue(double value, string expectedValue)
            {
                // Setup
                Angle angleToConvert = Angle.FromRadians(value);
                var converter = new AngleValueConverter();

                // Call 
                object result = converter.Convert(angleToConvert, targetType, null, null);

                // Assert
                Assert.That(result, Is.EqualTo(expectedValue));
            }

            [Test]
            [SetCulture("en-US")]
            [TestCaseSource(typeof(AngleConvertTestBase), nameof(GetConvertCases))]
            public void ConvertIsDegreesTrue_WithValidValueAndEnglishCulture_ReturnsExpectedTargetValue(double value, string expectedValue)
            {
                // Setup
                Angle angleToConvert = Angle.FromDegrees(value);
                var converter = new AngleValueConverter
                {
                    IsDegrees = true
                };

                // Call 
                object result = converter.Convert(angleToConvert, targetType, null, null);

                // Assert
                Assert.That(result, Is.EqualTo(expectedValue));
            }

            [Test]
            [SetCulture("nl-NL")]
            [TestCaseSource(typeof(AngleConvertTestBase), nameof(GetConvertCases))]
            public void ConvertIsDegreesFalse_WithValidValueAndDutchCulture_ReturnsExpectedTargetValue(double value, string expectedValue)
            {
                // Setup
                Angle angleToConvert = Angle.FromRadians(value);
                var converter = new AngleValueConverter();

                // Call 
                object result = converter.Convert(angleToConvert, targetType, null, null);

                // Assert
                Assert.That(result, Is.EqualTo(expectedValue));
            }

            [Test]
            [SetCulture("nl-NL")]
            [TestCaseSource(typeof(AngleConvertTestBase), nameof(GetConvertCases))]
            public void ConvertIsDegreesTrue_WithValidValueAndDutchCulture_ReturnsExpectedTargetValue(double value, string expectedValue)
            {
                // Setup
                Angle angleToConvert = Angle.FromDegrees(value);
                var converter = new AngleValueConverter
                {
                    IsDegrees = true
                };

                // Call 
                object result = converter.Convert(angleToConvert, targetType, null, null);

                // Assert
                Assert.That(result, Is.EqualTo(expectedValue));
            }

            /// <summary>
            /// Creates a new instance of <see cref="AngleConvertTestBase"/>.
            /// </summary>
            /// <param name="targetType">The <see cref="Type"/> to convert to.</param>
            protected AngleConvertTestBase(Type targetType)
            {
                this.targetType = targetType;
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
}