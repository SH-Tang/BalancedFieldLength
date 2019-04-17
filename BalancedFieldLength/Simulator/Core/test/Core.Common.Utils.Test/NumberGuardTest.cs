using System;
using Core.Common.Data;
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Utils.Test
{
    [TestFixture]
    public class NumberGuardTest
    {
        [Test]
        [TestCase(-1e-1)]
        [TestCase(0)]
        [TestCase(double.NegativeInfinity)]
        public void ValidateParameterLargerThanZeroDouble_InvalidValues_ThrowsArgumentOutOfRangeException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerThanZero(invalidValue, propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCase(1e-1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void ValidateParameterLargerThanZeroDouble_ValidValues_DoesNotThrowException(double validValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerThanZero(validValue, propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void ValidateParameterLargerThanZeroInteger_InvalidValues_ThrowsArgumentOutOfRangeException(int invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerThanZero(invalidValue, propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        public void ValidateParameterLargerThanZeroDouble_ValidValues_DoesNotThrowException()
        {
            // Setup
            const string propertyName = "name";
            const int validValue = 1;

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerThanZero(validValue, propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(double.NegativeInfinity)]
        public void ValidateParameterLargerThanZeroAngle_InvalidValues_ThrowsArgumentOutOfRangeException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";
            Angle angle = Angle.FromRadians(invalidValue);

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerThanZero(angle, propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCase(1e-1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void ValidateParameterLargerThanZerAngle_ValidValues_DoesNotThrowException(double validValue)
        {
            // Setup
            const string propertyName = "name";
            Angle angle = Angle.FromDegrees(validValue);

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerThanZero(angle, propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(-1e-1)]
        [TestCase(double.NegativeInfinity)]
        public void ValidateParameterLargerOrEqualToZero_InvalidValues_ThrowsArgumentOutOfRangeException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerOrEqualToZero(invalidValue, propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger or equal to 0.");
        }

        [Test]
        [TestCase(1e-1)]
        [TestCase(0)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void ValidateParameterLargerOrEqualToZero_ValidValues_DoesNotThrowException(double validValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsLargerOrEqualToZero(validValue, propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void ValidateValueIsConcreteNumberDouble_InvalidValues_ThrowsArgumentException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsConcreteNumber(invalidValue, propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
        }

        [Test]
        public void ValidateValueIsConcreteNumberDouble_ValidValues_DoesNotThrowException()
        {
            // Setup
            const string propertyName = "name";

            var random = new Random(21);
            double validValue = random.NextDouble();

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsConcreteNumber(validValue, propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void ValidateValueIsConcreteNumberAngle_InvalidValues_ThrowsArgumentException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";
            Angle angle = Angle.FromRadians(invalidValue);

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsConcreteNumber(angle, propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
        }

        [Test]
        public void ValidateValueIsConcreteNumberAngle_ValidValues_DoesNotThrowException()
        {
            // Setup
            const string propertyName = "name";

            var random = new Random(21);
            Angle validValue = random.NextAngle();

            // Call
            TestDelegate call = () => NumberGuard.ArgumentIsConcreteNumber(validValue, propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }
    }
}