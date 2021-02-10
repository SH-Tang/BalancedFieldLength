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
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Data.DataModel
{
    /// <summary>
    /// Helper class which can be used for validation <see cref="IDataModelRuleProvider"/>.
    /// </summary>
    public static class DataModelValidator
    {
        /// <summary>
        /// Validates the collection of <see cref="IDataModelRuleProvider"/>.
        /// </summary>
        /// <param name="dataModels">The collection of <see cref="IDataModelRuleProvider"/>
        /// to create the validator for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataModels"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dataModels"/> is <c>empty</c>.</exception>
        public static DataModelValidatorResult Validate(IEnumerable<IDataModelRuleProvider> dataModels)
        {
            if (dataModels == null)
            {
                throw new ArgumentNullException(nameof(dataModels));
            }

            if (!dataModels.Any())
            {
                throw new ArgumentException($"{nameof(dataModels)} cannot be empty.");
            }

            IEnumerable<IDataModelValidationRule> validationRules = GatherDataModelValidationRules(dataModels);
            IEnumerable<ValidationRuleResult> results = GatherValidationResults(validationRules);

            IEnumerable<ValidationRuleResult> validationFailures = results.Where(r => !r.IsValid);
            return validationFailures.Any() 
                       ? DataModelValidatorResult.CreateInvalidResult(validationFailures.Select(f => f.ValidationMessage))
                       : DataModelValidatorResult.ValidResult;
        }

        private static IEnumerable<ValidationRuleResult> GatherValidationResults(IEnumerable<IDataModelValidationRule> validationRules)
        {
            return validationRules.Select(rule => rule.Execute()).ToArray();
        }

        private static IEnumerable<IDataModelValidationRule> GatherDataModelValidationRules(IEnumerable<IDataModelRuleProvider> dataModels)
        {
            var validationRules = new List<IDataModelValidationRule>();
            foreach (IDataModelRuleProvider hasDataModelValidation in dataModels)
            {
                validationRules.AddRange(hasDataModelValidation.GetDataModelValidationRules());
            }

            return validationRules;
        }
    }
}