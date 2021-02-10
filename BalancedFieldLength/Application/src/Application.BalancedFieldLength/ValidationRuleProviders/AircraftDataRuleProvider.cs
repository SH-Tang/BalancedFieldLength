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
using Application.BalancedFieldLength.Properties;
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
                CreateParameterValidationRules(
                        ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_TakeOffWeight_DisplayName),
                        data.TakeOffWeight)
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_PitchAngleGradient_DisplayName),
                                data.PitchGradient))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_MaximumPitchAngle_DisplayName),
                                data.MaximumPitchAngle))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_WingSurfaceArea_DisplayName),
                                data.WingSurfaceArea))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_AspectRatio_DisplayName),
                                data.AspectRatio))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_OswaldFactor_DisplayName),
                                data.OswaldFactor))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_MaximumLiftCoefficient_DisplayName), 
                                data.MaximumLiftCoefficient))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_LiftCoefficientGradient_DisplayName), 
                                data.LiftCoefficientGradient))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_RestDragCoefficient_DisplayName), 
                                data.RestDragCoefficient))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_RestDragCoefficientWithEngineFailure_DisplayName), 
                                data.RestDragCoefficientWithEngineFailure))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_RollResistanceCoefficient_DisplayName), 
                                data.RollResistanceCoefficient))
                    .Concat(CreateParameterValidationRules(
                                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_RollResistanceWithBrakesCoefficient_DisplayName),
                                data.RollResistanceWithBrakesCoefficient));
            foreach (IDataModelValidationRule rule in parameterRules)
            {
                yield return rule;
            }

            yield return new AngleParameterConcreteValueRule(
                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_ZeroLiftAngleOfAttack_DisplayName),
                data.ZeroLiftAngleOfAttack);
            yield return new ComparableParameterGreaterThanRule<double>(
                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_RestDragCoefficientWithEngineFailure_DisplayName),
                data.RestDragCoefficientWithEngineFailure, data.RestDragCoefficient);
            yield return new ComparableParameterGreaterThanRule<double>(
                ParameterNameExtractor.ExtractParameterName(Resources.AircraftData_RollResistanceWithBrakesCoefficient_DisplayName),
                data.RollResistanceWithBrakesCoefficient, data.RollResistanceCoefficient);
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