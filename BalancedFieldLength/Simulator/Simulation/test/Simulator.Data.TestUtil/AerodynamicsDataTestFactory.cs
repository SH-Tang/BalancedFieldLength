using System;
using Core.Common.Data;

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
                                        Angle.FromDegrees(random.NextDouble()), random.NextDouble(),
                                        random.NextDouble(), random.NextDouble(),
                                        random.NextDouble(), random.NextDouble());
        }
    }
}