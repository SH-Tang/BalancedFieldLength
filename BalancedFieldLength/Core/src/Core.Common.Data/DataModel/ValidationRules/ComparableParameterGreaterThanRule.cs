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
using Core.Common.Data.Properties;

namespace Core.Common.Data.DataModel.ValidationRules
{
    /// <summary>
    /// Validation rule representing a parameter greater than a lower limit.
    /// </summary>
    /// <typeparam name="TComparable">The type of <see cref="IComparable{T}"/>.</typeparam>
    public class ComparableParameterGreaterThanRule<TComparable> : ParameterRuleBase 
        where TComparable:IComparable, IComparable<TComparable>
    {
        private readonly TComparable value;
        private readonly TComparable lowerLimit;

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="ComparableParameterGreaterThanRule"/>.
        /// </summary>
        /// <param name="value">The <see cref="IComparable{T}"/> value to construct the rule for.</param>
        /// <param name="lowerLimit">The lower limit value which the <paramref name="value"/> must be greater than.</param>
        public ComparableParameterGreaterThanRule(string parameterName, TComparable value, TComparable lowerLimit) 
            : base(parameterName)
        {
            this.value = value;
            this.lowerLimit = lowerLimit;
        }

        public override ValidationRuleResult Execute()
        {
            return lowerLimit.CompareTo(value) >= 0
                       ? ValidationRuleResult.CreateInvalidResult(
                           string.Format(Resources.NumericRule_Value_0_must_be_greater_or_equal_to_LowerLimit_1_Current_value_2,
                                         ParameterName, lowerLimit, value))
                       : ValidationRuleResult.ValidResult;
        }
    }
}