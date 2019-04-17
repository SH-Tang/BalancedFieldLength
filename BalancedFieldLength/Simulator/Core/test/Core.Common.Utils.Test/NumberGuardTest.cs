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
        public void ArgumentIsLargerThanZeroDouble_InvalidValues_ThrowsArgumentOutOfRangeException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => invalidValue.ArgumentIsLargerThanZero(propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCase(1e-1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void ArgumentIsLargerThanZeroDouble_ValidValues_DoesNotThrowException(double validValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => validValue.ArgumentIsLargerThanZero(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void ArgumentIsLargerThanZeroInteger_InvalidValues_ThrowsArgumentOutOfRangeException(int invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => invalidValue.ArgumentIsLargerThanZero(propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void ArgumentIsLargerThanZeroInteger_ValidValues_ThrowsArgumentOutOfRangeException(int invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => invalidValue.ArgumentIsLargerThanZero(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        public void ArgumentIsLargerThanZeroDouble_ValidValues_DoesNotThrowException()
        {
            // Setup
            const string propertyName = "name";
            const int validValue = 1;

            // Call
            TestDelegate call = () => validValue.ArgumentIsLargerThanZero(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(double.NegativeInfinity)]
        public void ArgumentIsLargerThanZeroAngle_InvalidValues_ThrowsArgumentOutOfRangeException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";
            Angle angle = Angle.FromRadians(invalidValue);

            // Call
            TestDelegate call = () => angle.ArgumentIsLargerThanZero(propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger than 0.");
        }

        [Test]
        [TestCase(1e-1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void ArgumentIsLargerThanZeroAngle_ValidValues_DoesNotThrowException(double validValue)
        {
            // Setup
            const string propertyName = "name";
            Angle angle = Angle.FromDegrees(validValue);

            // Call
            TestDelegate call = () => angle.ArgumentIsLargerThanZero(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(-1e-1)]
        [TestCase(double.NegativeInfinity)]
        public void ArgumentIsLargerOrEqualToZero_InvalidValues_ThrowsArgumentOutOfRangeException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => invalidValue.ArgumentIsLargerOrEqualToZero(propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, $"{propertyName} must be larger or equal to 0.");
        }

        [Test]
        [TestCase(1e-1)]
        [TestCase(0)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void ArgumentIsLargerOrEqualToZero_ValidValues_DoesNotThrowException(double validValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => validValue.ArgumentIsLargerOrEqualToZero(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void ArgumentIsConcreteNumberDouble_InvalidValues_ThrowsArgumentException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";

            // Call
            TestDelegate call = () => invalidValue.ArgumentIsConcreteNumber(propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
        }

        [Test]
        public void ArgumentIsConcreteNumberDouble_ValidValues_DoesNotThrowException()
        {
            // Setup
            const string propertyName = "name";

            var random = new Random(21);
            double validValue = random.NextDouble();

            // Call
            TestDelegate call = () => validValue.ArgumentIsConcreteNumber(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void ArgumentIsConcreteNumberAngle_InvalidValues_ThrowsArgumentException(double invalidValue)
        {
            // Setup
            const string propertyName = "name";
            Angle angle = Angle.FromRadians(invalidValue);

            // Call
            TestDelegate call = () => angle.ArgumentIsConcreteNumber(propertyName);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
        }

        [Test]
        public void ArgumentIsConcreteNumberAngle_ValidValues_DoesNotThrowException()
        {
            // Setup
            const string propertyName = "name";

            var random = new Random(21);
            Angle validValue = random.NextAngle();

            // Call
            TestDelegate call = () => validValue.ArgumentIsConcreteNumber(propertyName);

            // Assert
            Assert.DoesNotThrow(call);
        }
    }
}