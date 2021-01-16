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
            TestDelegate call = () => DataModelValidator.Validate(Enumerable.Empty<IHasDataModelValidation>());

            // Assert
            const string expectedMessage = "dataModels cannot be empty.";
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, expectedMessage);
        }

        [Test]
        public void Validate_WithValidDataModels_ThenExecutesAllRulesAndReturnsValidResult()
        {
            // Setup
            var validationRuleOne = Substitute.For<IDataModelValidationRule>();
            validationRuleOne.Execute().Returns(ValidationRuleResult.CreateValidResult());

            var validModelOne = Substitute.For<IHasDataModelValidation>();
            validModelOne.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleOne
            });

            var validationRuleTwo = Substitute.For<IDataModelValidationRule>();
            validationRuleTwo.Execute().Returns(ValidationRuleResult.CreateValidResult());

            var validModelTwo = Substitute.For<IHasDataModelValidation>();
            validModelTwo.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleTwo
            });

            IHasDataModelValidation[] models =
            {
                validModelOne,
                validModelTwo
            };

            // Call
            DataModelValidatorResult result = DataModelValidator.Validate(models);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.ValidationMessages, Is.Empty);

            validModelOne.Received(1).GetDataModelValidationRules();
            validationRuleOne.Received(1).Execute();

            validModelTwo.Received(1).GetDataModelValidationRules();
            validationRuleTwo.Received(1).Execute();
        }

        [Test]
        public void Validate_WithOneValidAndOneInvalidDataModel_ExecutesAllRulesAndReturnsInvalidResult()
        {
            // Setup
            var validationRuleOne = Substitute.For<IDataModelValidationRule>();
            validationRuleOne.Execute().Returns(ValidationRuleResult.CreateValidResult());

            var validModelOne = Substitute.For<IHasDataModelValidation>();
            validModelOne.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleOne
            });

            var validationRuleTwo = Substitute.For<IDataModelValidationRule>();
            validationRuleTwo.Execute().Returns(ValidationRuleResult.CreateValidResult());

            const string validationError = "Validation Error";
            var invalidValidationRule = Substitute.For<IDataModelValidationRule>();
            invalidValidationRule.Execute().Returns(ValidationRuleResult.CreateInvalidResult(validationError));

            var invalidModelTwo = Substitute.For<IHasDataModelValidation>();
            invalidModelTwo.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleTwo,
                invalidValidationRule
            });

            IHasDataModelValidation[] models =
            {
                validModelOne,
                invalidModelTwo
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
            validationRuleOne.Execute().Returns(ValidationRuleResult.CreateValidResult());
            
            const string validationErrorOne = "Validation Error One";
            var invalidValidationRuleOne = Substitute.For<IDataModelValidationRule>();
            invalidValidationRuleOne.Execute().Returns(ValidationRuleResult.CreateInvalidResult(validationErrorOne));

            var invalidModelOne = Substitute.For<IHasDataModelValidation>();
            invalidModelOne.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleOne,
                invalidValidationRuleOne
            });

            var validationRuleTwo = Substitute.For<IDataModelValidationRule>();
            validationRuleTwo.Execute().Returns(ValidationRuleResult.CreateValidResult());

            const string validationErrorTwo = "Validation Error Two";
            var invalidValidationRuleTwo = Substitute.For<IDataModelValidationRule>();
            invalidValidationRuleTwo.Execute().Returns(ValidationRuleResult.CreateInvalidResult(validationErrorTwo));

            var invalidModelTwo = Substitute.For<IHasDataModelValidation>();
            invalidModelTwo.GetDataModelValidationRules().Returns(new[]
            {
                validationRuleTwo,
                invalidValidationRuleTwo
            });

            IHasDataModelValidation[] models =
            {
                invalidModelOne,
                invalidModelTwo
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