using System;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel.ValidationRules
{
    [TestFixture]
    public class AngleParameterRuleBaseTest
    {
        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            Angle value = random.NextAngle();

            // Call
            var rule = new TestAngleParameterRuleBase(parameterName, value);

            // Assert
            Assert.That(rule, Is.InstanceOf<ParameterRuleBase>());
        }

        [Test]
        public void ValidateValueConcreteNumber_WithValidNumbers_ReturnsExpectedValidationResult()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            Angle value = random.NextAngle();

            var rule = new TestAngleParameterRuleBase(parameterName, value);

            // Call 
            ValidationRuleResult result = rule.PublicValidateValueConcreteNumber();

            // Assert
            Assert.That(result, Is.SameAs(ValidationRuleResult.ValidResult));
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void ValidateValueConcreteNumber_WithInvalidValues_ReturnsExpectedValidationResult(double invalidValue)
        {
            // Setup
            const string parameterName = "ParameterName";
            var rule = new TestAngleParameterRuleBase(parameterName, Angle.FromDegrees(invalidValue));

            // Call 
            ValidationRuleResult result = rule.PublicValidateValueConcreteNumber();

            // Assert
            Assert.That(result.IsValid, Is.False);

            var expectedMessage = $"{parameterName} must be a concrete number.";
            Assert.That(result.ValidationMessage, Is.EqualTo(expectedMessage));
        }

        private class TestAngleParameterRuleBase : AngleParameterRuleBase
        {
            public TestAngleParameterRuleBase(string parameterName, Angle value) : base(parameterName, value) {}

            public ValidationRuleResult PublicValidateValueConcreteNumber()
            {
                return ValidateValueConcreteNumber();
            }

            public override ValidationRuleResult Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}