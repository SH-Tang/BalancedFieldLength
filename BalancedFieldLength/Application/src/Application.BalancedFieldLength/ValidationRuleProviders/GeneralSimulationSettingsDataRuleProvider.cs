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
using Application.BalancedFieldLength.Data;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;

namespace Application.BalancedFieldLength.ValidationRuleProviders
{
    /// <summary>
    /// A <see cref="IDataModelRuleProvider"/> to provide validation rules for instances of <see cref="GeneralSimulationSettingsData"/>.
    /// </summary>
    public class GeneralSimulationSettingsDataRuleProvider : IDataModelRuleProvider
    {
        private readonly GeneralSimulationSettingsData data;

        /// <summary>
        /// Creates an instance of <see cref="EngineDataRuleProvider"/>.
        /// </summary>
        /// <param name="data">The <see cref="GeneralSimulationSettingsData"/> to create the provider for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is <c>null</c>.</exception>
        public GeneralSimulationSettingsDataRuleProvider(GeneralSimulationSettingsData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.data = data;
        }

        public IEnumerable<IDataModelValidationRule> GetDataModelValidationRules()
        {
            IEnumerable<IDataModelValidationRule> doubleParameterRules =
                CreateDoubleParameterValidationRules("Density", data.Density)
                    .Concat(CreateDoubleParameterValidationRules("Gravitational acceleration", data.GravitationalAcceleration))
                    .Concat(CreateDoubleParameterValidationRules("Time step", data.TimeStep));
            foreach (IDataModelValidationRule rule in doubleParameterRules)
            {
                yield return rule;
            }

            yield return new ComparableParameterGreaterThanRule<int>("Maximum number of iterations", data.MaximumNrOfIterations, 0);
            yield return new ComparableParameterGreaterThanRule<int>("End failure velocity", data.EndFailureVelocity, 0);
        }

        private static IEnumerable<IDataModelValidationRule> CreateDoubleParameterValidationRules(string parameterName, double value)
        {
            yield return new DoubleParameterConcreteValueRule(parameterName, value);
            yield return new ComparableParameterGreaterThanRule<double>(parameterName, value, 0);
        }
    }
}