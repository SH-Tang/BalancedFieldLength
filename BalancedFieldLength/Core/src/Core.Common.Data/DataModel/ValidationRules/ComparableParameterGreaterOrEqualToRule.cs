using System;
using Core.Common.Data.Properties;

namespace Core.Common.Data.DataModel.ValidationRules
{
    /// <summary>
    /// Validation rule representing a parameter greater or equal to a lower limit.
    /// </summary>
    /// <typeparam name="TComparable">The type of <see cref="IComparable{T}"/>.</typeparam>
    public class ComparableParameterGreaterOrEqualToRule<TComparable> : ParameterRuleBase
        where TComparable : IComparable, IComparable<TComparable>
    {
        private readonly TComparable value;
        private readonly TComparable lowerLimit;

        /// <inheritdoc/>
        /// <summary>
        /// Creates a new instance of <see cref="ComparableParameterGreaterOrEqualToRule"/>.
        /// </summary>
        /// <param name="value">The <see cref="IComparable{T}"/> value to construct the rule for.</param>
        /// <param name="lowerLimit">The lower limit value which the <paramref name="value"/> must be greater or equal to.</param>
        public ComparableParameterGreaterOrEqualToRule(string parameterName, TComparable value, TComparable lowerLimit)
            : base(parameterName)
        {
            this.value = value;
            this.lowerLimit = lowerLimit;
        }

        public override ValidationRuleResult Execute()
        {
            return lowerLimit.CompareTo(value) > 0
                       ? ValidationRuleResult.CreateInvalidResult(
                           string.Format(Resources.NumericRule_Value_0_must_be_greater_or_equal_to_LowerLimit_1_Current_value_2,
                                         ParameterName, lowerLimit, value))
                       : ValidationRuleResult.ValidResult;
        }
    }
}