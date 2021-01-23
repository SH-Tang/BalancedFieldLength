using Core.Common.Data.Properties;

namespace Core.Common.Data.DataModel.ValidationRules
{
    /// <summary>
    /// Validation rule representing a <see cref="Angle"/> being a concrete number. (e.g. not <see cref="double.NaN"/>,
    /// <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.
    /// </summary>
    public class AngleParameterConcreteValueRule : ParameterRuleBase
    {
        private readonly Angle value;

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="AngleParameterConcreteValueRule"/>.
        /// </summary>
        /// <param name="value">The value to create the rule for.</param>
        public AngleParameterConcreteValueRule(string parameterName, Angle value) : base(parameterName)
        {
            this.value = value;
        }

        public override ValidationRuleResult Execute()
        {
            return !value.IsConcreteAngle()
                       ? ValidationRuleResult.CreateInvalidResult(
                           string.Format(Resources.NumberParameterRuleBase_Value_0_must_be_a_concrete_number, ParameterName))
                       : ValidationRuleResult.ValidResult;
        }
    }
}