using System;
using Core.Common.TestUtil;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Creates valid instances of <see cref="AircraftData"/> which can be used for testing.
    /// </summary>
    public static class AircraftDataTestFactory
    {
        /// <summary>
        /// Generates an instance of <see cref="AircraftData"/> with random values.
        /// </summary>
        /// <returns>An <see cref="AircraftData"/> with random values.</returns>
        public static AircraftData CreateRandomAircraftData()
        {
            var random = new Random(21);
            return new AircraftData(random.Next(), random.NextDouble(),
                                    random.NextDouble(), random.NextAngle(),
                                    random.NextAngle(), random.NextDouble(),
                                    random.NextDouble(), AerodynamicsDataTestFactory.CreateAerodynamicsData());
        }
    }
}