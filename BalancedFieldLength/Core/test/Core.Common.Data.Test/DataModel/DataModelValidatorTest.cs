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
using Core.Common.Data.DataModel;
using Core.Common.TestUtil;
using NSubstitute;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel
{
    [TestFixture]
    public class DataModelValidatorTest
    {
        [Test]
        public void Validate_DataModelsNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => DataModelValidator.Validate(null);

            // Assert
            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("dataModels"));
        }

        [Test]
        public void Validate_DataModelsEmpty_ThrowsArgumentException()
        {
            // Call
            TestDelegate call = () => DataModelValidator.Validate(Enumerable.Empty<IDataModelRuleProvider>());

            // Assert
            const string expectedMessage = "dataModels cannot be empty.";
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, expectedMessage);
        }

        [Test]
        public void Validate_WithValidDataModels_ThenExecutesAllRulesAndReturnsValidResult()
        {
            // Setup
            var validationRuleOne = Substitute.For<IDataModelValidationRule>();
            validationRuleOne.Execute().Returns(ValidationRuleResult.ValidResult);

            var validProviderOne = Substitute.For<IDataModelRuleProvider>();
            validProviderOne.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleOne
            });

            var validationRuleTwo = Substitute.For<IDataModelValidationRule>();
            validationRuleTwo.Execute().Returns(ValidationRuleResult.ValidResult);

            var validProviderTwo = Substitute.For<IDataModelRuleProvider>();
            validProviderTwo.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleTwo
            });

            IDataModelRuleProvider[] models =
            {
                validProviderOne,
                validProviderTwo
            };

            // Call
            DataModelValidatorResult result = DataModelValidator.Validate(models);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.ValidationMessages, Is.Empty);

            validProviderOne.Received(1).GetDataModelValidationRules();
            validationRuleOne.Received(1).Execute();

            validProviderTwo.Received(1).GetDataModelValidationRules();
            validationRuleTwo.Received(1).Execute();
        }

        [Test]
        public void Validate_WithOneValidAndOneInvalidDataModel_ExecutesAllRulesAndReturnsInvalidResult()
        {
            // Setup
            var validationRuleOne = Substitute.For<IDataModelValidationRule>();
            validationRuleOne.Execute().Returns(ValidationRuleResult.ValidResult);

            var validProvider = Substitute.For<IDataModelRuleProvider>();
            validProvider.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleOne
            });

            var validationRuleTwo = Substitute.For<IDataModelValidationRule>();
            validationRuleTwo.Execute().Returns(ValidationRuleResult.ValidResult);

            const string validationError = "Validation Error";
            var invalidValidationRule = Substitute.For<IDataModelValidationRule>();
            invalidValidationRule.Execute().Returns(ValidationRuleResult.CreateInvalidResult(validationError));

            var invalidProvider = Substitute.For<IDataModelRuleProvider>();
            invalidProvider.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleTwo,
                invalidValidationRule
            });

            IDataModelRuleProvider[] models =
            {
                validProvider,
                invalidProvider
            };

            // Call
            DataModelValidatorResult result = DataModelValidator.Validate(models);

            // Assert
            Assert.That(result.IsValid, Is.False);
            CollectionAssert.AreEqual(new[]
            {
                validationError
            }, result.ValidationMessages);
        }

        [Test]
        public void Validate_WithTwoInvalidModels_ExecutesAllRulesAndReturnsInvalidResult()
        {
            // Setup
            var validationRuleOne = Substitute.For<IDataModelValidationRule>();
            validationRuleOne.Execute().Returns(ValidationRuleResult.ValidResult);
            
            const string validationErrorOne = "Validation Error One";
            var invalidValidationRuleOne = Substitute.For<IDataModelValidationRule>();
            invalidValidationRuleOne.Execute().Returns(ValidationRuleResult.CreateInvalidResult(validationErrorOne));

            var invalidProviderOne = Substitute.For<IDataModelRuleProvider>();
            invalidProviderOne.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleOne,
                invalidValidationRuleOne
            });

            var validationRuleTwo = Substitute.For<IDataModelValidationRule>();
            validationRuleTwo.Execute().Returns(ValidationRuleResult.ValidResult);

            const string validationErrorTwo = "Validation Error Two";
            var invalidValidationRuleTwo = Substitute.For<IDataModelValidationRule>();
            invalidValidationRuleTwo.Execute().Returns(ValidationRuleResult.CreateInvalidResult(validationErrorTwo));

            var invalidProviderTwo = Substitute.For<IDataModelRuleProvider>();
            invalidProviderTwo.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleTwo,
                invalidValidationRuleTwo
            });

            IDataModelRuleProvider[] models =
            {
                invalidProviderOne,
                invalidProviderTwo
            };

            // Call
            DataModelValidatorResult result = DataModelValidator.Validate(models);

            // Assert
            Assert.That(result.IsValid, Is.False);
            CollectionAssert.AreEquivalent(new[]
            {
                validationErrorOne,
                validationErrorTwo
            }, result.ValidationMessages);

        }
    }
}