using System;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Factory to create valid instance of <see cref="CalculationSettings"/>
    /// which can be used for testing
    /// </summary>
    public static class CalculationSettingsTestFactory
    {
        /// <summary>
        /// Creates a <see cref="CalculationSettings"/> with valid values.
        /// </summary>
        /// <returns>A <see cref="CalculationSettings"/>.</returns>
        public static CalculationSettings CreateDistanceCalculatorSettings()
        {
            var random = new Random(21);
            return new CalculationSettings(random.Next(),
                                           random.Next(),
                                           random.NextDouble());
        }
    }
}