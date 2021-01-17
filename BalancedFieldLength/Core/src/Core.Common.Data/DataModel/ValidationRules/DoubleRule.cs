namespace Core.Common.Data.DataModel.ValidationRules
{
    public abstract class DoubleRule : ComparableValidationRule<double>
    {
        protected DoubleRule(double value, string parameterName, double lowerLimit, double upperLimit)
            : base(value, parameterName, lowerLimit, upperLimit) {}

        public static DoubleRule GreaterThan(double value, string parameterName, double lowerLimit)
        {
            return new GreaterThanRule(value, parameterName, lowerLimit);
        }

        public static DoubleRule GreaterOrEqualTo(double value, string parameterName, double lowerLimit)
        {
            return new GreaterOrEqualToRule(value, parameterName, lowerLimit);
        }

        private class GreaterOrEqualToRule : DoubleRule
        {
            public GreaterOrEqualToRule(double value, string parameterName,
                                        double lowerLimit)
                : base(value, parameterName, lowerLimit, double.MaxValue) {}

            protected override string GetInvalidResultMessage()
            {
                throw new System.NotImplementedException();
            }
        }

        private class GreaterThanRule : DoubleRule
        {
            public GreaterThanRule(double value, string parameterName,
                                   double lowerLimit)
                : base(value, parameterName, lowerLimit + double.Epsilon, double.MaxValue) {}

            protected override string GetInvalidResultMessage()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}