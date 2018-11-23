using System.Collections.Generic;
using System.Linq;
using Calculator.Data;
using NUnit.Framework;

namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Class which can be used to provide real test data with aircraft properties.
    /// </summary>
    public static class AircraftTestData
    {
        /// <summary>
        /// Gets the test cases containing real aircraft data.
        /// </summary>
        /// <returns>The test cases with actual aircraft data.</returns>
        /// <remarks>This test case data can be used as:
        /// <code>
        /// [Test]
        /// [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAircraftData))]
        /// public static void SomeTest(AircraftData aircraftData)
        /// {
        ///     // Test
        /// }
        /// </code>
        /// </remarks>
        public static IEnumerable<TestCaseData> GetAircraftData()
        {
            TestAerodynamicData[] aerodynamicTestData = GetAerodynamicData();
            yield return new TestCaseData(new AircraftData(2, 75, 500, 6, 16, 0.02, 0.2, aerodynamicTestData[0].Data))
                .SetName(aerodynamicTestData[0].TestName);
            yield return new TestCaseData(new AircraftData(3, 120, 1200, 5, 15, 0.02, 0.2, aerodynamicTestData[1].Data))
                .SetName(aerodynamicTestData[1].TestName);
            yield return new TestCaseData(new AircraftData(4, 300, 3500, 4, 14, 0.02, 0.2, aerodynamicTestData[2].Data))
                .SetName(aerodynamicTestData[2].TestName);

        }

        /// <summary>
        /// Gets the test cases containing real aerodynamic data.
        /// </summary>
        /// <returns>The test cases with aerodynamic data.</returns>
        /// <remarks>This test case data can be used as:
        /// <code>
        /// [Test]
        /// [TestCaseSource(typeof(AircraftTestData), nameof(AircraftTestData.GetAerodynamicDataTestCases))]
        /// public static void SomeTest(AerodynamicData aerodynamicData)
        /// {
        ///     // Test
        /// }
        /// </code>
        /// </remarks>
        public static IEnumerable<TestCaseData> GetAerodynamicDataTestCases()
        {
            return GetAerodynamicData().Select(testData => new TestCaseData(testData.Data)
                                                   .SetName(testData.TestName));
        }

        private static TestAerodynamicData[] GetAerodynamicData()
        {
            return new[]
                   {
                       new TestAerodynamicData(new AerodynamicData(15, 100, -3, 4.85, 1.60, 0.021, 0.026, 0.85), "Two Jet Power Engine"),
                       new TestAerodynamicData(new AerodynamicData(14, 200, -4, 4.32, 1.45, 0.024, 0.028, 0.80), "Three Jet Power Engine"),
                       new TestAerodynamicData(new AerodynamicData(12, 500, -5, 3.95, 1.40, 0.026, 0.029, 0.82), "Four Jet Power Engine")
                   };
        }

        private class TestAerodynamicData
        {
            public TestAerodynamicData(AerodynamicData data, string testName)
            {
                Data = data;
                TestName = testName;
            }

            public AerodynamicData Data { get; }
            public string TestName { get; }
        }
    }
}