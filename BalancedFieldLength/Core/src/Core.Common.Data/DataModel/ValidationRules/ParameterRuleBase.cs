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
    /// Base class definition for validation rule definitions based on a parameter name.
    /// </summary>
    public abstract class ParameterRuleBase : IDataModelValidationRule
    {
        protected readonly string ParameterName;

        /// <summary>
        /// Creates a new instance of <see cref="ParameterRuleBase"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName"/>
        /// is <c>null</c>, empty or consists of whitespaces.</exception>
        protected ParameterRuleBase(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(parameterName));
            }

            ParameterName = parameterName;
        }

        public abstract ValidationRuleResult Execute();
    }
}