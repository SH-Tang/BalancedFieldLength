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
using Core.Common.Data;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;

namespace Application.BalancedFieldLength.ValidationRuleProviders
{
    /// <summary>
    /// A <see cref="IDataModelRuleProvider"/> to provide validation rules for instances of <see cref="AircraftData"/>.
    /// </summary>
    public class AircraftDataRuleProvider : IDataModelRuleProvider
    {
        private readonly AircraftData data;

        /// <summary>
        /// Creates an instance of <see cref="AircraftDataRuleProvider"/>.
        /// </summary>
        /// <param name="data">The <see cref="AircraftData"/> to create the provider for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is <c>null</c>.</exception>
        public AircraftDataRuleProvider(AircraftData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.data = data;
        }

        public IEnumerable<IDataModelValidationRule> GetDataModelValidationRules()
        {
            IEnumerable<IDataModelValidationRule> parameterRules =
                CreateParameterValidationRules("Take off Weight", data.TakeOffWeight)
                    .Concat(CreateParameterValidationRules("Pitch gradient", data.PitchGradient))
                    .Concat(CreateParameterValidationRules("Maximum pitch angle", data.MaximumPitchAngle))
                    .Concat(CreateParameterValidationRules("Wing surface area", data.WingSurfaceArea))
                    .Concat(CreateParameterValidationRules("Aspect ratio", data.AspectRatio))
                    .Concat(CreateParameterValidationRules("Oswald factor", data.OswaldFactor))
                    .Concat(CreateParameterValidationRules("Maximum lift coefficient", data.MaximumLiftCoefficient))
                    .Concat(CreateParameterValidationRules("Lift coefficient gradient", data.LiftCoefficientGradient))
                    .Concat(CreateParameterValidationRules("Rest drag coefficient", data.RestDragCoefficient))
                    .Concat(CreateParameterValidationRules("Rest drag coefficient with engine failure", data.RestDragCoefficientWithEngineFailure))
                    .Concat(CreateParameterValidationRules("Roll resistance coefficient", data.RollResistanceCoefficient))
                    .Concat(CreateParameterValidationRules("Roll resistance with brakes coefficient", data.RollResistanceWithBrakesCoefficient));
            foreach (IDataModelValidationRule rule in parameterRules)
            {
                yield return rule;
            }

            yield return new AngleParameterConcreteValueRule("Zero angle", data.ZeroLiftAngleOfAttack);
            yield return new ComparableParameterGreaterThanRule<double>("Rest drag coefficient with engine failure", data.RestDragCoefficientWithEngineFailure, data.RestDragCoefficient);
            yield return new ComparableParameterGreaterThanRule<double>("Roll resistance with brakes coefficient", data.RollResistanceWithBrakesCoefficient, data.RollResistanceCoefficient);
        }

        private static IEnumerable<IDataModelValidationRule> CreateParameterValidationRules(string parameterName, double value)
        {
            yield return new DoubleParameterConcreteValueRule(parameterName, value);
            yield return new ComparableParameterGreaterThanRule<double>(parameterName, value, 0);
        }

        private static IEnumerable<IDataModelValidationRule> CreateParameterValidationRules(string parameterName, Angle value)
        {
            yield return new AngleParameterConcreteValueRule(parameterName, value);
            yield return new ComparableParameterGreaterThanRule<Angle>(parameterName, value, new Angle());
        }
    }
}