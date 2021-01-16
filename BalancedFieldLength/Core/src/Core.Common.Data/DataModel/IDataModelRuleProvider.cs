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

using System.Collections.Generic;

namespace Core.Common.Data.DataModel
{
    /// <summary>
    /// Interface describing a provider to provide validation rules for a data model.
    /// </summary>
    public interface IDataModelRuleProvider
    {
        /// <summary>
        /// Gets the collection of <see cref="IDataModelValidationRule"/>.
        /// </summary>
        /// <returns>The collection of <see cref="IDataModelValidationRule"/>.</returns>
        IEnumerable<IDataModelValidationRule> GetDataModelValidationRules();
    }
}