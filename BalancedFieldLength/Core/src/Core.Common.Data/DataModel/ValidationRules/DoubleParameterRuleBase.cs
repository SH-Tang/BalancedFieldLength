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
    /// Base definition of a validation rule based on parameters representing double values.
    /// </summary>
    public abstract class DoubleParameterRuleBase : ParameterRuleBase
    {
        protected readonly double Value;

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="DoubleParameterRuleBase"/>.
        /// </summary>
        /// <param name="value">The value to create the rule for.</param>
        protected DoubleParameterRuleBase(string parameterName, double value) : base(parameterName)
        {
            Value = value;
        }

        protected ValidationRuleResult ValidateValueConcreteNumber()
        {
            if (double.IsNaN(Value) || double.IsInfinity(Value))
            {
                return ValidationRuleResult.CreateInvalidResult($"{ParameterName} must be a concrete number.");
            }

            return ValidationRuleResult.ValidResult;
        }
    }
}