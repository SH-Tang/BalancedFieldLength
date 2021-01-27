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
using System.Linq;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.ValidationRuleProviders;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.ValidationRuleProviders
{
    [TestFixture]
    public class GeneralSettingsDataRuleProviderTest
    {
        [Test]
        public void Constructor_DataNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new GeneralSimulationSettingsDataRuleProvider(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("data"));
        }

        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            var data = new GeneralSimulationSettingsData();

            // Call
            var provider = new GeneralSimulationSettingsDataRuleProvider(data);

            // Assert
            Assert.That(provider, Is.InstanceOf<IDataModelRuleProvider>());
        }

        [Test]
        public void GetDataModelValidationRules_Always_ReturnsExpectedValidationRules()
        {
            // Setup
            var data = new GeneralSimulationSettingsData();
            var provider = new GeneralSimulationSettingsDataRuleProvider(data);

            // Call 
            IDataModelValidationRule[] rules = provider.GetDataModelValidationRules().ToArray();

            // Assert
            Assert.That(rules, Has.Length.EqualTo(8));
            Assert.That(rules[0], Is.TypeOf<DoubleParameterConcreteValueRule>());
            Assert.That(rules[1], Is.TypeOf<ComparableParameterGreaterThanRule<double>>());
            Assert.That(rules[2], Is.TypeOf<DoubleParameterConcreteValueRule>());
            Assert.That(rules[3], Is.TypeOf<ComparableParameterGreaterThanRule<double>>());
            Assert.That(rules[4], Is.TypeOf<DoubleParameterConcreteValueRule>());
            Assert.That(rules[5], Is.TypeOf<ComparableParameterGreaterThanRule<double>>());
            
            Assert.That(rules[6], Is.TypeOf<ComparableParameterGreaterThanRule<int>>());
            Assert.That(rules[7], Is.TypeOf<ComparableParameterGreaterThanRule<int>>());
        }
    }
}