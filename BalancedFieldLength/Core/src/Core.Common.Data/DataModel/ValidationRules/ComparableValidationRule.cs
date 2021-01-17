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
    public abstract class ComparableValidationRule<T> : IDataModelValidationRule
        where T : IComparable<T>
    {
        private readonly T value;
        private readonly T lowerLimit;
        private readonly T upperLimit;
        protected readonly string ParameterName;

        protected ComparableValidationRule(T value,
                                           string parameterName,
                                           T lowerLimit, T upperLimit)
        {
            this.value = value;
            this.lowerLimit = lowerLimit;
            this.upperLimit = upperLimit;
            this.ParameterName = parameterName;
        }

        public ValidationRuleResult Execute()
        {
            bool isInRange = lowerLimit.CompareTo(value) <= 0 && upperLimit.CompareTo(value) >= 0;
            if (isInRange)
            {
                return ValidationRuleResult.CreateValidResult();
            }

            return ValidationRuleResult.CreateInvalidResult(GetInvalidResultMessage());
        }

        protected abstract string GetInvalidResultMessage();
    }
}