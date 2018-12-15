using System;
using Core.Common.Data;
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

        /// <summary>
        /// Generates an instance of <see cref="AircraftData"/> with random values
        /// and a set value for the zero lift angle of attack.
        /// </summary>
        /// <returns>An <see cref="AircraftData"/> with random values.</returns>
        public static AircraftData CreateRandomAircraftData(Angle zeroLiftAngleOfAttack)
        {
            var random = new Random(21);
            return new AircraftData(random.Next(), random.NextDouble(),
                                    random.NextDouble(), random.NextAngle(),
                                    random.NextAngle(), random.NextDouble(),
                                    random.NextDouble(),
                                    GenerateAerodynamicsData(random, zeroLiftAngleOfAttack));
        }

        private static AerodynamicsData GenerateAerodynamicsData(Random random, Angle zeroLiftAngleOfAttack)
        {
            return new AerodynamicsData(random.NextDouble(), random.NextDouble(),
                                        zeroLiftAngleOfAttack, random.NextDouble(),
                                        random.NextDouble(), random.NextDouble(),
                                        random.NextDouble(), random.NextDouble());
        }
    }
}