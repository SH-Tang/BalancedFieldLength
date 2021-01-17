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

namespace Core.Common.Data.DataModel.ValidationRules
{
    /// <summary>
    /// Validation rule representing a parameter greater than a lower limit.
    /// </summary>
    public class DoubleParameterGreaterThanRule : DoubleParameterRuleBase
    {
        private readonly double lowerLimit;

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="DoubleParameterGreaterThanRule"/>.
        /// </summary>
        /// <param name="lowerLimit">The lower limit value which the <paramref name="value"/> must be greater to.</param>
        public DoubleParameterGreaterThanRule(string parameterName, double value, double lowerLimit)
            : base(parameterName, value)
        {
            this.lowerLimit = lowerLimit;
        }

        public override ValidationRuleResult Execute()
        {
            ValidationRuleResult result = ValidateValueConcreteNumber();
            if (result != ValidationRuleResult.ValidResult)
            {
                return result;
            }

            return !(Value >= lowerLimit)
                       ? ValidationRuleResult.CreateInvalidResult($"{ParameterName} must be > {lowerLimit}. Current value: {Value}")
                       : ValidationRuleResult.ValidResult;
        }
    }
}