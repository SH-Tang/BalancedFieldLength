using System;
using Core.Common.Data.DataModel;
using Core.Common.Data.DataModel.ValidationRules;
using NUnit.Framework;

namespace Core.Common.Data.Test.DataModel.ValidationRules
{
    [TestFixture]
    public abstract class DoubleParameterRuleBaseTestFixture<T> where T : DoubleParameterRuleBase
    {
        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            const string parameterName = "ParameterName";

            var random = new Random(21);
            double value = random.NextDouble();

            // Call
            T rule = CreateRule(parameterName, value);

            // Assert
            Assert.That(rule, Is.InstanceOf<DoubleParameterRuleBase>());
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void Execute_WithNonConcreteValues_ReturnsExpectedValidationResult(double invalidValue)
        {
            // Setup
            const string parameterName = "ParameterName";
            T rule = CreateRule(parameterName, invalidValue);

            // Call 
            ValidationRuleResult result = rule.Execute();

            // Assert
            Assert.That(result.IsValid, Is.False);

            var expectedMessage = $"{parameterName} must be a concrete number.";
            Assert.That(result.ValidationMessage, Is.EqualTo(expectedMessage));
        }

        protected abstract T CreateRule(string parameterName, double value);
    }
}