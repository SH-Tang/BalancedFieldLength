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

namespace Core.Common.Data.DataModel.ValidationRules
{
    /// <summary>
    /// Helper class that can be used to create numeric validation rules.
    /// </summary>
    public static class NumericRule
    {
        /// <summary>
        /// Creates a <see cref="IDataModelValidationRule"/> to verify whether the
        /// <paramref name="value"/> is greater than <paramref name="lowerLimit"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to make the rule for.</param>
        /// <param name="value">The value to verify the rule with.</param>
        /// <param name="lowerLimit">The lower limit of the value</param>
        /// <returns>An <see cref="IDataModelValidationRule"/> which verifies whether the <paramref name="value"/>
        /// is greater than <paramref name="lowerLimit"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName"/>
        /// is <c>null</c>, empty or consists of whitespaces.</exception>
        public static IDataModelValidationRule DoubleGreaterThan(string parameterName,
                                                                 double value,
                                                                 double lowerLimit)
        {
            return new DoubleParameterGreaterThanRule(parameterName, value, lowerLimit);
        }

        /// <summary>
        /// Creates a <see cref="IDataModelValidationRule"/> to verify whether the
        /// <paramref name="value"/> is greater or equal to <paramref name="lowerLimit"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to make the rule for.</param>
        /// <param name="value">The value to verify the rule with.</param>
        /// <param name="lowerLimit">The lower limit of the value</param>
        /// <returns>An <see cref="IDataModelValidationRule"/> which verifies whether the <paramref name="value"/>
        /// is greater or equal to <paramref name="lowerLimit"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName"/>
        /// is <c>null</c>, empty or consists of whitespaces.</exception>
        public static IDataModelValidationRule DoubleGreaterOrEqualTo(string parameterName,
                                                                      double value,
                                                                      double lowerLimit)
        {
            return new DoubleParameterGreaterOrEqualToRule(parameterName, value, lowerLimit);
        }
    }
}