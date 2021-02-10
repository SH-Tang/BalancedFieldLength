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
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.Properties;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;

namespace Application.BalancedFieldLength.ValidationRuleProviders
{
    /// <summary>
    /// A <see cref="IDataModelRuleProvider"/> to provide validation rules for instances of <see cref="EngineData"/>.
    /// </summary>
    public class EngineDataRuleProvider : IDataModelRuleProvider
    {
        private readonly EngineData data;

        /// <summary>
        /// Creates an instance of <see cref="EngineDataRuleProvider"/>.
        /// </summary>
        /// <param name="data">The <see cref="EngineData"/> to create the provider for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is <c>null</c>.</exception>
        public EngineDataRuleProvider(EngineData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.data = data;
        }

        public IEnumerable<IDataModelValidationRule> GetDataModelValidationRules()
        {
            yield return new DoubleParameterConcreteValueRule(
                ParameterNameExtractor.ExtractParameterName(Resources.EngineData_ThrustPerEngine_DisplayName),
                data.ThrustPerEngine);
            yield return new ComparableParameterGreaterThanRule<double>(
                ParameterNameExtractor.ExtractParameterName(Resources.EngineData_ThrustPerEngine_DisplayName),
                data.ThrustPerEngine, 0);
            yield return new ComparableParameterGreaterThanRule<int>(
                ParameterNameExtractor.ExtractParameterName(Resources.EngineData_NumberOfEngines_DisplayName),
                data.NrOfEngines, 0);
            yield return new ComparableParameterGreaterThanRule<int>(
                ParameterNameExtractor.ExtractParameterName(Resources.EngineData_NrOfFailedEngines_DisplayName),
                data.NrOfFailedEngines, 0);
            yield return new ComparableParameterGreaterThanRule<int>(
                ParameterNameExtractor.ExtractParameterName(Resources.EngineData_NumberOfEngines_DisplayName),
                data.NrOfEngines, data.NrOfFailedEngines);
        }
    }
}