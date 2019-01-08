using System;
using Core.Common.Data;

namespace Core.Common.Utils
{
    /// <summary>
    /// Class which can be used to validate numerical data.
    /// </summary>
    public static class NumberValidator
    {
        /// <summary>
        /// Validates whether a value is larger than 0.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is less or equal to 0.</exception>
        public static void ValidateParameterLargerThanZero(double value, string propertyName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Validates whether a value is larger than 0.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is less or equal to 0.</exception>
        public static void ValidateParameterLargerThanZero(int value, string propertyName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Validates whether a value is larger than 0.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is less or equal to 0.</exception>
        public static void ValidateParameterLargerThanZero(Angle value, string propertyName)
        {
            if (value <= Angle.FromRadians(0))
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Validates whether a value is larger or equal to 0.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is less than 0.</exception>
        public static void ValidateParameterLargerOrEqualToZero(double value, string propertyName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger or equal to 0.");
            }
        }

        /// <summary>
        /// Validates whether a value is not <see cref="double.NaN"/> or Infinity.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is <see cref="double.NaN"/>, <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.</exception>
        public static void ValidateValueIsConcreteNumber(double value, string propertyName)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException($"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
            }
        }

        /// <summary>
        /// Validates whether a value is not <see cref="double.NaN"/> or Infinity.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is <see cref="double.NaN"/>, <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.</exception>
        public static void ValidateValueIsConcreteNumber(Angle value, string propertyName)
        {
            double radians = value.Radians;
            if (double.IsNaN(radians) || double.IsInfinity(radians))
            {
                throw new ArgumentException($"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
            }
        }
    }
}