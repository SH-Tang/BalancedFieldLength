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

using Core.Common.Data.Properties;

namespace Core.Common.Data.DataModel.ValidationRules
{
    /// <summary>
    /// Base definition of a validation rule based on parameters representing <see cref="Angle"/> values.
    /// </summary>
    public abstract class AngleParameterRuleBase : ParameterRuleBase
    {
        private readonly Angle value;

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="AngleParameterRuleBase"/>.
        /// </summary>
        /// <param name="value">The value to create the rule for.</param>
        protected AngleParameterRuleBase(string parameterName, Angle value) : base(parameterName)
        {
            this.value = value;
        }

        protected ValidationRuleResult ValidateValueConcreteNumber()
        {
            return !value.IsConcreteAngle()
                       ? ValidationRuleResult.CreateInvalidResult(
                           string.Format(Resources.NumberParameterRuleBase_Value_0_must_be_a_concrete_number, ParameterName))
                       : ValidationRuleResult.ValidResult;
        }
    }
}