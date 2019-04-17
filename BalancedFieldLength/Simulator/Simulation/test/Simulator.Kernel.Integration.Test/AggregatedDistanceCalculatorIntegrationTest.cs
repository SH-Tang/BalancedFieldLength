using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Common.Data;
using Core.Common.TestUtil;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.BalancedFieldLengthCalculator;
using Simulator.Components.Integrators;
using Simulator.Data;

namespace Simulator.Kernel.Integration.Test
{
    [TestFixture]
    public class AggregatedDistanceCalculatorIntegrationTest
    {
        private const double gravitationalAcceleration = 9.81;
        private const double density = 1.225;

        private const int maximumTimeSteps = 10000;
        private const double timeStep = 0.1;

        private const double tolerance = 1e-2;

        [Test]
        [TestCaseSource(nameof(GetAircraftData))]
        public void GivenKernel_WhenCalculationsAreMadeForVelocityRange_ThenReturnsExpectedOutputsAndBalancedFieldLength(AircraftData aircraftData,
                                                                                                                         IntegrationReferenceData integrationReferenceData)
        {
            // Given
            var integrator = new EulerIntegrator();
            var calculationKernel = new AggregatedDistanceCalculatorKernel();

            // When 
            List<AggregatedDistanceOutput> outputs = new List<AggregatedDistanceOutput>();
            for (int i = 0; i < 90; i++)
            {
                var calculationSettings = new CalculationSettings(i, maximumTimeSteps, timeStep);
                AggregatedDistanceOutput result = calculationKernel.Calculate(aircraftData,
                                                                              integrator,
                                                                              1,
                                                                              density,
                                                                              gravitationalAcceleration,
                                                                              calculationSettings);
                outputs.Add(result);
            }

            BalancedFieldLength balancedFieldLength = BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Then
            IEnumerable<ReferenceOutput> referenceOutputs = GetReferenceOutputs(integrationReferenceData.FileName);
            int expectedLength = referenceOutputs.Count();
            Assert.AreEqual(expectedLength, outputs.Count, "Number of reference data entries do not match with actual number of entries");

            int velocity = 0;
            foreach (ReferenceOutput referenceOutput in referenceOutputs)
            {
                Assert.AreEqual(referenceOutput.Velocity, outputs[velocity].FailureSpeed);
                Assert.AreEqual(referenceOutput.ContinuedTakeOffDistance, outputs[velocity].ContinuedTakeOffDistance, tolerance);
                Assert.AreEqual(referenceOutput.AbortedTakeOffDistance, outputs[velocity].AbortedTakeOffDistance, tolerance);

                velocity++;
            }

            Assert.AreEqual(integrationReferenceData.Velocity, balancedFieldLength.Velocity, tolerance);
            Assert.AreEqual(integrationReferenceData.Distance, balancedFieldLength.Distance, tolerance);
        }

        private static IEnumerable<ReferenceOutput> GetReferenceOutputs(string fileName)
        {
            string solutionPath = TestHelper.GetSolutionRootPath();
            string referenceFilePath = Path.Combine(solutionPath, @"Simulator\Simulation\test\Simulator.Kernel.Integration.Test\test-data", fileName);

            List<ReferenceOutput> referenceOutputs = new List<ReferenceOutput>();
            using (var parser = new TextFieldParser(referenceFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                bool firstLine = true;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }

                    int velocity = int.Parse(fields[0]);
                    double continuedTakeOffDistance = double.Parse(fields[1]);
                    double abortedTakeOffDistance = double.Parse(fields[2]);
                    referenceOutputs.Add(new ReferenceOutput(velocity, continuedTakeOffDistance, abortedTakeOffDistance));
                }
            }

            return referenceOutputs;
        }

        private static IEnumerable<TestCaseData> GetAircraftData()
        {
            var twoEngineReferenceData = new IntegrationReferenceData("Two Power Jet Engine.csv", 72.03, 2343.09);
            yield return new TestCaseData(new AircraftData(2, 75, 500, Angle.FromDegrees(6), Angle.FromDegrees(16), 0.02, 0.2,
                                                           new AerodynamicsData(15, 100, Angle.FromDegrees(-3), 4.85, 1.60, 0.021, 0.026, 0.85)),
                                          twoEngineReferenceData)
                .SetName("Two Jet Power Engine");

            var threeEngineReferenceData = new IntegrationReferenceData("Three Power Jet Engine.csv", 78.10, 2779.96);
            yield return new TestCaseData(new AircraftData(3, 120, 1200, Angle.FromDegrees(5), Angle.FromDegrees(15), 0.02, 0.2,
                                                           new AerodynamicsData(14, 200, Angle.FromDegrees(-4), 4.32, 1.45, 0.024, 0.028, 0.80)),
                                          threeEngineReferenceData)
                .SetName("Three Jet Power Engine");

            var fourEngineReferenceData = new IntegrationReferenceData("Four Power Jet Engine.csv", 81.40, 2865.53);
            yield return new TestCaseData(new AircraftData(4, 300, 3500, Angle.FromDegrees(4), Angle.FromDegrees(14), 0.02, 0.2,
                                                           new AerodynamicsData(12, 500, Angle.FromDegrees(-5), 3.95, 1.40, 0.026, 0.029, 0.82)),
                                          fourEngineReferenceData)
                .SetName("Four Jet Power Engine");
        }

        /// <summary>
        /// Class holding all the reference data information.
        /// </summary>
        public class IntegrationReferenceData
        {
            /// <summary>
            /// Creates a new instance of <see cref="IntegrationReferenceData"/>.
            /// </summary>
            /// <param name="fileName">The file name to read the reference data from.</param>
            /// <param name="velocity">The velocity at which the balanced field length occurs. [m/s]</param>
            /// <param name="distance">The distance at which the balanced field length occurs. [m]</param>
            public IntegrationReferenceData(string fileName, double velocity, double distance)
            {
                FileName = fileName;
                Velocity = velocity;
                Distance = distance;
            }

            /// <summary>
            /// Gets the file name with the reference data.
            /// </summary>
            public string FileName { get; }

            /// <summary>
            /// Gets the velocity of which the balanced field length occurs. [m/s]
            /// </summary>
            public double Velocity { get; }

            /// <summary>
            /// Gets the distance of the balanced field length. [m]
            /// </summary>
            public double Distance { get; }
        }

        /// <summary>
        /// Represents the reference output generated by the calculator for each velocity.
        /// </summary>
        private class ReferenceOutput
        {
            /// <summary>
            /// Creates a new instance of <see cref="ReferenceOutput"/>.
            /// </summary>
            /// <param name="velocity">The velocity at which the failure occurred. [m/s]</param>
            /// <param name="continuedTakeOffDistance">The continued take-off distance until screenheight was reached. [m]</param>
            /// <param name="abortedTakeOffDistance">The aborted take-off distance until velocity was 0. [m]</param>
            public ReferenceOutput(int velocity, double continuedTakeOffDistance, double abortedTakeOffDistance)
            {
                Velocity = velocity;
                ContinuedTakeOffDistance = continuedTakeOffDistance;
                AbortedTakeOffDistance = abortedTakeOffDistance;
            }

            /// <summary>
            /// Gets the velocity at which the failure occurred. [m/s]
            /// </summary>
            public int Velocity { get; }

            /// <summary>
            /// Gets the covered distance until reaching screenheight when the take-off was continued. [m]
            /// </summary>
            public double ContinuedTakeOffDistance { get; }

            /// <summary>
            /// Gets the covered distance until velocity is 0 when the take-off was aborted. [m]
            /// </summary>
            public double AbortedTakeOffDistance { get; }
        }
    }
}