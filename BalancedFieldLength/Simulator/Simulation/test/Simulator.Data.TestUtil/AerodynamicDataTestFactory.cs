using System;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Creates valid instances of <see cref="AerodynamicData"/> which can be used for testing.
    /// </summary>
    public static class AerodynamicDataTestFactory
    {
        /// <summary>
        /// Generates an instance of <see cref="AerodynamicData"/> with random values.
        /// </summary>
        /// <returns>An <see cref="AerodynamicData"/> with random values.</returns>
        public static AerodynamicData CreateAerodynamicData()
        {
            var random = new Random(21);

            return new AerodynamicData(random.NextDouble(), random.NextDouble(),
                                       random.NextDouble(), random.NextDouble(),
                                       random.NextDouble(), random.NextDouble(),
                                       random.NextDouble(), random.NextDouble());
        }
    }
}