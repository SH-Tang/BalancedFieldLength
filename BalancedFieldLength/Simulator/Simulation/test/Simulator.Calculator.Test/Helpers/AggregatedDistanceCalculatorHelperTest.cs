using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.TestUtil;
using NUnit.Framework;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.Helpers;

namespace Simulator.Calculator.Test.Helpers
{
    [TestFixture]
    public class AggregatedDistanceCalculatorHelperTest
    {
        [Test]
        public void DetermineCrossing_OutputsNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => AggregatedDistanceCalculatorHelper.DetermineCrossing(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("outputs", exception.ParamName);
        }

        [Test]
        public void DetermineCrossing_OutputsEmpty_ThrowsArgumentException()
        {
            // Call 
            TestDelegate call = () => AggregatedDistanceCalculatorHelper.DetermineCrossing(Enumerable.Empty<AggregatedDistanceOutput>());

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "Cannot determine crossing from a collection containing 0 or 1 item.");
        }

        [Test]
        public void DetermineCrossing_OutputHasOneEntry_ThrowsArgumentException()
        {
            // Setup
            var random = new Random(21);
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(random.NextDouble(), random.NextDouble(), random.NextDouble())
            };

            // Call 
            TestDelegate call = () => AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "Cannot determine crossing from a collection containing 0 or 1 item.");
        }

        [Test]
        [TestCaseSource(nameof(GetCollectionsWithDuplicateEntries))]
        public void DetermineCrossing_DuplicateOutputs_ThrowsArgumentException(IEnumerable<AggregatedDistanceOutput> outputs,
                                                                               double failureSpeed)
        {
            // Setup

            // Call 
            TestDelegate call = () => AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"Outputs cannot contain duplicate definitions for failure speed {failureSpeed}.");
        }

        [Test]
        public void DetermineCrossing_OutputsNeverIntersect_ReturnsNaN()
        {
            // Setup
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(10, 10, 11),
                new AggregatedDistanceOutput(11, 5, 15),
                new AggregatedDistanceOutput(12, 0, 20)
            };

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.IsNaN(output.FailureSpeed);
            Assert.IsNaN(output.AbortedTakeOffDistance);
            Assert.IsNaN(output.ContinuedTakeOffDistance);
        }

        [Test]
        public void DetermineCrossing_OutputsOnTop_ReturnsNaN()
        {
            // Setup
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(10, 10, 10),
                new AggregatedDistanceOutput(11, 10, 10),
                new AggregatedDistanceOutput(12, 10, 10)
            };

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.IsNaN(output.FailureSpeed);
            Assert.IsNaN(output.AbortedTakeOffDistance);
            Assert.IsNaN(output.ContinuedTakeOffDistance);
        }

        [Test]
        public void DetermineCrossing_OutputsParallel_ReturnsNaN()
        {
            // Setup
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(10, 10, 20),
                new AggregatedDistanceOutput(11, 10, 20),
                new AggregatedDistanceOutput(12, 10, 20)
            };

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.IsNaN(output.FailureSpeed);
            Assert.IsNaN(output.AbortedTakeOffDistance);
            Assert.IsNaN(output.ContinuedTakeOffDistance);
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void DetermineCrossing_VariousConfigurationsWithIntersection_ReturnsExpectedIntersection(IEnumerable<AggregatedDistanceOutput> outputs,
                                                                                                        double expectedFailureSpeed,
                                                                                                        double expectedDistance)
        {
            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(outputs);

            // Assert
            Assert.AreEqual(expectedFailureSpeed, output.FailureSpeed);
            Assert.AreEqual(expectedDistance, output.AbortedTakeOffDistance);
            Assert.AreEqual(expectedDistance, output.ContinuedTakeOffDistance);
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void DetermineCrossing_VariousConfigurationsWithIntersectionInRandomOrder_ReturnsExpectedIntersection(IEnumerable<AggregatedDistanceOutput> outputs,
                                                                                                                     double expectedFailureSpeed,
                                                                                                                     double expectedDistance)
        {
            // Setup
            var random = new Random(21);
            IOrderedEnumerable<AggregatedDistanceOutput> randomSortedOutputs = outputs.OrderBy(x => random.Next());

            // Call
            AggregatedDistanceOutput output = AggregatedDistanceCalculatorHelper.DetermineCrossing(randomSortedOutputs);

            // Assert
            Assert.AreEqual(expectedFailureSpeed, output.FailureSpeed);
            Assert.AreEqual(expectedDistance, output.AbortedTakeOffDistance);
            Assert.AreEqual(expectedDistance, output.ContinuedTakeOffDistance);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new[]
                                          {
                                              new AggregatedDistanceOutput(10, 10, 30),
                                              new AggregatedDistanceOutput(11, 20, 20),
                                              new AggregatedDistanceOutput(12, 30, 10)
                                          }, 11, 20)
                .SetName("AbortedDistance Increasing, ContinuedDistance Decreasing, Defined Intersection Point");

            yield return new TestCaseData(new[]
                                          {
                                              new AggregatedDistanceOutput(10, 30, 10),
                                              new AggregatedDistanceOutput(11, 20, 20),
                                              new AggregatedDistanceOutput(12, 10, 30)
                                          }, 11, 20)
                .SetName("AbortedDistance Decreasing, ContinuedDistance Increasing, Defined Intersection Point");

            yield return new TestCaseData(new[]
                                          {
                                              new AggregatedDistanceOutput(10, 10, 30),
                                              new AggregatedDistanceOutput(12, 30, 10)
                                          }, 11, 20)
                .SetName("AbortedDistance Increasing, ContinuedDistance Decreasing, UnDefine Intersection Point");

            yield return new TestCaseData(new[]
                                          {
                                              new AggregatedDistanceOutput(10, 30, 10),
                                              new AggregatedDistanceOutput(12, 10, 30)
                                          }, 11, 20)
                .SetName("AbortedDistance Decreasing, ContinuedDistance Increasing, Undefined Intersection Point");
        }

        private static IEnumerable<TestCaseData> GetCollectionsWithDuplicateEntries()
        {
            yield return new TestCaseData(new[]
                                          {
                                              new AggregatedDistanceOutput(10, 10, 30),
                                              new AggregatedDistanceOutput(10, 10, 30),
                                              new AggregatedDistanceOutput(12, 30, 10)
                                          }, 10)
                .SetName("Duplicate in sequence");
            yield return new TestCaseData(new[]
                                          {
                                              new AggregatedDistanceOutput(10, 10, 30),
                                              new AggregatedDistanceOutput(12, 30, 10),
                                              new AggregatedDistanceOutput(10, 10, 30)
                                          }, 10)
                .SetName("Duplicate out of order");
        }
    }
}