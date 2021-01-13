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
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using Core.Common.TestUtil;
using NUnit.Framework;
using WPF.Components.MessageView;

namespace WPF.Components.Test.MessageView
{
    [TestFixture]
    public class MessageTypeConverterTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var converter = new MessageTypeConverter();

            // Assert
            Assert.That(converter, Is.InstanceOf<IValueConverter>());
        }

        [Test]
        [TestCaseSource(nameof(GetNonMessageTypes))]
        public void Convert_WithInvalidValueTypes_ThrowsNotSupportedException(object unsupportedValue)
        {
            // Setup
            var converter = new MessageTypeConverter();

            // Call 
            TestDelegate call = () => converter.Convert(unsupportedValue, typeof(Brush), null, null);

            // Assert
            string expectedMessage = $"Conversion from {unsupportedValue?.GetType().Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        public void Convert_WithInvalidTargetType_ThrowsNotSupportedException()
        {
            // Setup
            var random = new Random(21);
            var converter = new MessageTypeConverter();

            Type unsupportedType = typeof(int);

            // Call
            TestDelegate call = () => converter.Convert(random.NextEnum<MessageType>(), unsupportedType, null, null);

            // Assert
            string expectedMessage = $"Conversion to {unsupportedType.Name} is not supported.";
            Assert.That(call, Throws.Exception.InstanceOf<NotSupportedException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        [TestCaseSource(nameof(GetConvertCases))]
        public void Convert_WithValidMessageTypeAndTargetTypeObject_ReturnsExpectedValue(
            MessageType valueToConvert, Brush expectedValue)
        {
            // Setup
            var converter = new MessageTypeConverter();

            // Call
            object convertedValue = converter.Convert(valueToConvert, typeof(object), null, null);

            // Assert
            Assert.That(convertedValue, Is.InstanceOf<Brush>());
            Assert.That(convertedValue, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCaseSource(nameof(GetConvertCases))]
        public void Convert_WithValidMessageTypeAndTargetTypeBrush_ReturnsExpectedValue(
            MessageType valueToConvert, Brush expectedValue)
        {
            // Setup
            var converter = new MessageTypeConverter();

            // Call
            object convertedValue = converter.Convert(valueToConvert, typeof(Brush), null, null);

            // Assert
            Assert.That(convertedValue, Is.InstanceOf<Brush>());
            Assert.That(convertedValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public void Convert_WithInvalidMessageType_ThrowsNotSupportedException()
        {
            // Setup
            const MessageType invalidMessageType = (MessageType) 1001;
            
            var converter = new MessageTypeConverter();

            // Call
            TestDelegate call = () => converter.Convert(invalidMessageType, typeof(object), null, null);

            // Assert
            string expectedMessage = $"The value of argument 'value' ({(int)invalidMessageType}) is invalid for Enum type '{typeof(MessageType).Name}'." +
                                     $"{Environment.NewLine}Parameter name: value";
            Assert.That(call, Throws.Exception.InstanceOf<InvalidEnumArgumentException>()
                                    .And.Message.EqualTo(expectedMessage));
        }

        [Test]
        public void ConvertBack_Always_ThrowsNotSupportedException()
        {
            // Setup
            var converter = new MessageTypeConverter();

            // Call
            TestDelegate call = () => converter.ConvertBack(null, null, null, null);

            // Assert
            Assert.That(call, Throws.Exception.TypeOf<NotSupportedException>()
                                    .With.Message.EqualTo("ConvertBack operation is not supported."));
        }

        private static IEnumerable<TestCaseData> GetConvertCases()
        {
            yield return new TestCaseData(MessageType.Info, Brushes.CornflowerBlue);
            yield return new TestCaseData(MessageType.Error, Brushes.Crimson);
            yield return new TestCaseData(MessageType.Warning, Brushes.Gold);
        }

        private static IEnumerable<TestCaseData> GetNonMessageTypes()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new object());
        }
    }
}