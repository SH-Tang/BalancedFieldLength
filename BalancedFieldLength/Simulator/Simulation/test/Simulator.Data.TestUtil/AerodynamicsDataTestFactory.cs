using System;
using Core.Common.TestUtil;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Creates valid instances of <see cref="AerodynamicsData"/> which can be used for testing.
    /// </summary>
    public static class AerodynamicsDataTestFactory
    {
        /// <summary>
        /// Generates an instance of <see cref="AerodynamicsData"/> with random values.
        /// </summary>
        /// <returns>An <see cref="AerodynamicsData"/> with random values.</returns>
        public static AerodynamicsData CreateAerodynamicsData()
        {
            var random = new Random(21);

            return new AerodynamicsData(random.NextDouble(), random.NextDouble(),
                                        random.NextAngle(), random.NextDouble(),
                                        random.NextDouble(), random.NextDouble(),
                                        random.NextDouble(), random.NextDouble());
        }
    }
}