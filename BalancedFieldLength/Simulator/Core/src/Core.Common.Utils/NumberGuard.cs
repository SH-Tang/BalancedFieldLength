using System;
using Core.Common.Data;

namespace Core.Common.Utils
{
    /// <summary>
    /// Class which can be used to guard numerical data.
    /// </summary>
    public static class NumberGuard
    {
        /// <summary>
        /// Guards whether the argument is larger than 0.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is less or equal to 0.</exception>
        public static void ArgumentIsLargerThanZero(this double argument, string propertyName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Guards whether the argument is larger than 0.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is less or equal to 0.</exception>
        public static void ArgumentIsLargerThanZero(this int argument, string propertyName)
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Guards whether the argument is larger than 0.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is less or equal to 0.</exception>
        public static void ArgumentIsLargerThanZero(this Angle argument, string propertyName)
        {
            if (argument <= Angle.FromRadians(0))
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Guards whether the argument is larger or equal to 0.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is less than 0.</exception>
        public static void ArgumentIsLargerOrEqualToZero(this double argument, string propertyName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger or equal to 0.");
            }
        }

        /// <summary>
        /// Guards whether the argument is not <see cref="double.NaN"/> or Infinity.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is <see cref="double.NaN"/>, <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.</exception>
        public static void ArgumentIsConcreteNumber(this double argument, string propertyName)
        {
            if (double.IsNaN(argument) || double.IsInfinity(argument))
            {
                throw new ArgumentException($"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
            }
        }

        /// <summary>
        /// Guards whether the argument is not <see cref="double.NaN"/> or Infinity.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is <see cref="double.NaN"/>, <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.</exception>
        public static void ArgumentIsConcreteNumber(this Angle argument, string propertyName)
        {
            double radians = argument.Radians;
            if (double.IsNaN(radians) || double.IsInfinity(radians))
            {
                throw new ArgumentException($"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
            }
        }
    }
}