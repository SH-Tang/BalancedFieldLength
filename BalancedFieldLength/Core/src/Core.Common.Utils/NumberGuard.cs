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
using Core.Common.Data;
using Core.Common.Utils.Properties;

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
                string message = string.Format(Resources.PropertyName_0_must_be_larger_than_Zero, propertyName);
                throw new ArgumentOutOfRangeException(propertyName, message);
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
                string message = string.Format(Resources.PropertyName_0_must_be_larger_than_Zero, propertyName);
                throw new ArgumentOutOfRangeException(propertyName, message);
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
                string message = string.Format(Resources.PropertyName_0_must_be_larger_than_Zero, propertyName);
                throw new ArgumentOutOfRangeException(propertyName, message);
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
                string message = string.Format(Resources.PropertyName_0_must_be_larger_or_equal_to_Zero, propertyName);
                throw new ArgumentOutOfRangeException(propertyName, message);
            }
        }

        /// <summary>
        /// Guards whether the argument is larger or equal to 0.
        /// </summary>
        /// <param name="argument">The argument to guard.</param>
        /// <param name="propertyName">The name of the property which is being guarded.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="argument"/>
        /// is less than 0.</exception>
        public static void ArgumentIsLargerOrEqualToZero(this int argument, string propertyName)
        {
            if (argument < 0)
            {
                string message = string.Format(Resources.PropertyName_0_must_be_larger_or_equal_to_Zero, propertyName);
                throw new ArgumentOutOfRangeException(propertyName, message);
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
                string message = string.Format(Resources.PropertyName_0_must_be_a_concrete_number_and_cannot_be_NaN_or_Infinity, propertyName);
                throw new ArgumentException(message);
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
                string message = string.Format(Resources.PropertyName_0_must_be_a_concrete_number_and_cannot_be_NaN_or_Infinity, propertyName);
                throw new ArgumentException(message);
            }
        }
    }
}