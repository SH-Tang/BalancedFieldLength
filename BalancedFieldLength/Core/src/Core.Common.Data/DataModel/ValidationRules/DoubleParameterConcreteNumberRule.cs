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
    /// Validation rule representing a double being a concrete number. (e.g. not <see cref="double.NaN"/>,
    /// <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.
    /// </summary>
    public class DoubleParameterConcreteNumberRule : DoubleParameterRuleBase
    {
        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="DoubleParameterConcreteNumberRule"/>.
        /// </summary>
        public DoubleParameterConcreteNumberRule(string parameterName, double value) : base(parameterName, value) {}

        public override ValidationRuleResult Execute()
        {
            return ValidateValueConcreteNumber();
        }
    }
}