using System;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel.ValidationRules
{
    [TestFixture]
    public class AngleParameterConcreteValueRuleTest
    {
        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            Angle value = random.NextAngle();

            // Call
            var rule = new AngleParameterConcreteValueRule(parameterName, value);

            // Assert
            Assert.That(rule, Is.InstanceOf<ParameterRuleBase>());
        }

        [Test]
        public void Execute_WithValidValue_ReturnsValidResult()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            Angle value = random.NextAngle();

            var rule = new AngleParameterConcreteValueRule(parameterName, value);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result, Is.SameAs(ValidationRuleResult.ValidResult));
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void Execute_WithNonConcreteValues_ReturnsExpectedValidationResult(double invalidValue)
        {
            // Setup
            const string parameterName = "ParameterName";
            Angle invalidAngle = Angle.FromDegrees(invalidValue);
            var rule = new AngleParameterConcreteValueRule(parameterName, invalidAngle);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result.IsValid, Is.False);

            var expectedMessage = $"{parameterName} must be a concrete number.";
            Assert.That(result.ValidationMessage, Is.EqualTo(expectedMessage));
        }
    }
}