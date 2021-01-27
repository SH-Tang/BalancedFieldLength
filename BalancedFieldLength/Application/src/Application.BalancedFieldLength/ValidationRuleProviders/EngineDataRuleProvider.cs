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
            yield return new DoubleParameterConcreteValueRule("Thrust per engine", data.ThrustPerEngine);
            yield return new ComparableParameterGreaterThanRule<double>("Thrust per engine", data.ThrustPerEngine, 0);
            yield return new ComparableParameterGreaterThanRule<int>("Nr of Engines", data.NrOfEngines, 0);
            yield return new ComparableParameterGreaterThanRule<int>("Nr of Failed Engines", data.NrOfFailedEngines, 0);
            yield return new ComparableParameterGreaterThanRule<int>("Nr of Engines", data.NrOfEngines, data.NrOfFailedEngines);
        }
    }
}