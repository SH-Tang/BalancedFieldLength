using System;

namespace Simulator.Calculator.TestUtil
{
    /// <summary>
    /// Factory to create valid instance of <see cref="DistanceCalculatorSettings"/>
    /// which can be used for testing
    /// </summary>
    public static class DistanceCalculatorSettingsTestFactory
    {
        /// <summary>
        /// Creates a <see cref="DistanceCalculatorSettings"/> with valid values.
        /// </summary>
        /// <returns>A <see cref="DistanceCalculatorSettings"/>.</returns>
        public static DistanceCalculatorSettings CreateDistanceCalculatorSettings()
        {
            var random = new Random(21);
            return new DistanceCalculatorSettings(random.Next(),
                random.Next(),
                random.NextDouble());
        }
    }
}
