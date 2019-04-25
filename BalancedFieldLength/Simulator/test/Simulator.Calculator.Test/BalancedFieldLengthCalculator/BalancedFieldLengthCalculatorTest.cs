// Copyright (C) 2018 Dennis Tang. All rights reserved.
//
// This file is part of Balanced Field Length.
//
// Balanced Field Length is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.TestUtil;
using NUnit.Framework;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.BalancedFieldLengthCalculator;

namespace Simulator.Calculator.Test.BalancedFieldLengthCalculator
{
    [TestFixture]
    public class BalancedFieldLengthCalculatorTest
    {
        [Test]
        public void CalculateBalancedFieldLength_OutputsNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("outputs", exception.ParamName);
        }

        [Test]
        public void CalculateBalancedFieldLength_OutputsEmpty_ThrowsArgumentException()
        {
            // Call 
            TestDelegate call = () => Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(Enumerable.Empty<AggregatedDistanceOutput>());

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "Cannot determine crossing from a collection containing 0 or 1 item.");
        }

        [Test]
        public void CalculateBalancedFieldLength_OutputHasOneEntry_ThrowsArgumentException()
        {
            // Setup
            var random = new Random(21);
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(random.NextDouble(), random.NextDouble(), random.NextDouble())
            };

            // Call 
            TestDelegate call = () => Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "Cannot determine crossing from a collection containing 0 or 1 item.");
        }

        [Test]
        [TestCaseSource(nameof(GetCollectionsWithDuplicateEntries))]
        public void CalculateBalancedFieldLength_DuplicateOutputs_ThrowsArgumentException(IEnumerable<AggregatedDistanceOutput> outputs,
                                                                                          double failureSpeed)
        {
            // Setup

            // Call 
            TestDelegate call = () => Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, $"Outputs cannot contain duplicate definitions for failure speed {failureSpeed}.");
        }

        [Test]
        public void CalculateBalancedFieldLength_OutputsNeverIntersect_ReturnsNaN()
        {
            // Setup
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(10, 10, 11),
                new AggregatedDistanceOutput(11, 5, 15),
                new AggregatedDistanceOutput(12, 0, 20)
            };

            // Call
            BalancedFieldLength output = Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Assert
            Assert.IsNaN(output.Velocity);
            Assert.IsNaN(output.Distance);
        }

        [Test]
        public void CalculateBalancedFieldLength_OutputsOnTop_ReturnsNaN()
        {
            // Setup
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(10, 10, 10),
                new AggregatedDistanceOutput(11, 10, 10),
                new AggregatedDistanceOutput(12, 10, 10)
            };

            // Call
            BalancedFieldLength output = Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Assert
            Assert.IsNaN(output.Velocity);
            Assert.IsNaN(output.Distance);
        }

        [Test]
        public void CalculateBalancedFieldLength_OutputsParallel_ReturnsNaN()
        {
            // Setup
            AggregatedDistanceOutput[] outputs =
            {
                new AggregatedDistanceOutput(10, 10, 20),
                new AggregatedDistanceOutput(11, 10, 20),
                new AggregatedDistanceOutput(12, 10, 20)
            };

            // Call
            BalancedFieldLength output = Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Assert
            Assert.IsNaN(output.Velocity);
            Assert.IsNaN(output.Distance);
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void CalculateBalancedFieldLength_VariousConfigurationsWithIntersection_ReturnsExpectedIntersection(IEnumerable<AggregatedDistanceOutput> outputs,
                                                                                                                   double expectedFailureSpeed,
                                                                                                                   double expectedDistance)
        {
            // Call
            BalancedFieldLength output = Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(outputs);

            // Assert
            Assert.AreEqual(expectedFailureSpeed, output.Velocity);
            Assert.AreEqual(expectedDistance, output.Distance);
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases))]
        public void CalculateBalancedFieldLength_VariousConfigurationsWithIntersectionInRandomOrder_ReturnsExpectedIntersection(IEnumerable<AggregatedDistanceOutput> outputs,
                                                                                                                                double expectedFailureSpeed,
                                                                                                                                double expectedDistance)
        {
            // Setup
            var random = new Random(21);
            IOrderedEnumerable<AggregatedDistanceOutput> randomSortedOutputs = outputs.OrderBy(x => random.Next());

            // Call
            BalancedFieldLength output = Calculator.BalancedFieldLengthCalculator.BalancedFieldLengthCalculator.CalculateBalancedFieldLength(randomSortedOutputs);

            // Assert
            Assert.AreEqual(expectedFailureSpeed, output.Velocity);
            Assert.AreEqual(expectedDistance, output.Distance);
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
                .SetName("AbortedDistance Increasing, ContinuedDistance Decreasing, Undefined Intersection Point");

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