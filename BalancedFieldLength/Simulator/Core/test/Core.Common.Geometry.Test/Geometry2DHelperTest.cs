using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Core.Common.Geometry.Test
{
    [TestFixture]
    public class Geometry2DHelperTest
    {
        [Test]
        public void DetermineCrossing_Line1Null_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            var lineSegment = new LineSegment(new Point2D(random.NextDouble(), random.NextDouble()),
                                              new Point2D(random.NextDouble(), random.NextDouble()));

            // Call
            TestDelegate call = () => Geometry2DHelper.DetermineLineIntersection(null, lineSegment);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("line1", exception.ParamName);
        }

        [Test]
        public void DetermineCrossing_Line2Null_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            var lineSegment = new LineSegment(new Point2D(random.NextDouble(), random.NextDouble()),
                                              new Point2D(random.NextDouble(), random.NextDouble()));

            // Call
            TestDelegate call = () => Geometry2DHelper.DetermineLineIntersection(lineSegment, null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("line2", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetLinesOnTopScenarios))]
        public void DetermineCrossing_LinesOnTop_ReturnsNaN(LineSegment line1, LineSegment line2)
        {
            // Call 
            Point2D result = Geometry2DHelper.DetermineLineIntersection(line1, line2);

            // Assert
            Assert.IsNaN(result.X);
            Assert.IsNaN(result.Y);
        }

        [Test]
        [TestCaseSource(nameof(GetCrossingLinesScenarios))]
        public void DetermineCrossing_VariousScenariosWithCrossing_ReturnsExpectedResult(LineSegment line1,
                                                                                         LineSegment line2,
                                                                                         Point2D expectedResult)
        {
            // Call 
            Point2D result = Geometry2DHelper.DetermineLineIntersection(line1, line2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCaseSource(nameof(GetParallelLinesScenarios))]
        public void DetermineCrossing_ParallelLines_ReturnsNaN(LineSegment line1, LineSegment line2)
        {
            // Call 
            Point2D result = Geometry2DHelper.DetermineLineIntersection(line1, line2);

            // Assert
            Assert.IsNaN(result.X);
            Assert.IsNaN(result.Y);
        }

        #region Test Data

        private static IEnumerable<TestCaseData> GetCrossingLinesScenarios()
        {
            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(10, 20),
                                                          new Point2D(20, 10)),
                                          new Point2D(15, 15))
                .SetName("Line 1 Increasing, Line 2 Decreasing, Intermediate point intersect");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 20),
                                                          new Point2D(20, 10)),
                                          new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new Point2D(15, 15))
                .SetName("Line 1 Decreasing, Line 2 Increasing, Intermediate point intersect");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(10, 20)),
                                          new LineSegment(new Point2D(5, 15),
                                                          new Point2D(15, 15)),
                                          new Point2D(10, 15))
                .SetName("Line 1 Vertical, Line 2 Horizontal, Intermediate point intersect");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(10, 20)),
                                          new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new Point2D(10, 10))
                .SetName("Line 1 Start Point, Line 2 Start Point intersect");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(10, 20)),
                                          new LineSegment(new Point2D(20, 20),
                                                          new Point2D(10, 10)),
                                          new Point2D(10, 10))
                .SetName("Line 1 Start Point, Line 2 End Point intersect");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 20),
                                                          new Point2D(10, 10)),
                                          new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new Point2D(10, 10))
                .SetName("Line 1 End Point, Line 2 Start Point intersect");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 20),
                                                          new Point2D(10, 10)),
                                          new LineSegment(new Point2D(20, 20),
                                                          new Point2D(10, 10)),
                                          new Point2D(10, 10))
                .SetName("Line 1 End Point, Line 2 End Point intersect");
        }

        private static IEnumerable<TestCaseData> GetLinesOnTopScenarios()
        {
            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)))
                .SetName("Full overlap, same direction");
            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(20, 20),
                                                          new Point2D(10, 10)))
                .SetName("Full overlap, reversed direction");

            yield return new TestCaseData(new LineSegment(new Point2D(5, 20),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(10, 20),
                                                          new Point2D(20, 20)))
                .SetName("Partial overlap, same direction");
            yield return new TestCaseData(new LineSegment(new Point2D(20, 20),
                                                          new Point2D(5, 20)),
                                          new LineSegment(new Point2D(10, 20),
                                                          new Point2D(20, 20)))
                .SetName("Partial overlap, reversed direction");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(20, 20),
                                                          new Point2D(30, 30)))
                .SetName("Line 1 End Point, Line 2 Start Point coincide");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(-10, -10),
                                                          new Point2D(10, 10)))
                .SetName("Line 1 Start Point, Line 2 End Point coincide");
        }

        private static IEnumerable<TestCaseData> GetParallelLinesScenarios()
        {
            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 10)),
                                          new LineSegment(new Point2D(10, 20),
                                                          new Point2D(20, 20)))
                .SetName("Horizontally parallel");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(10, 20)),
                                          new LineSegment(new Point2D(20, 10),
                                                          new Point2D(20, 20)))
                .SetName("Vertically parallel");

            yield return new TestCaseData(new LineSegment(new Point2D(10, 10),
                                                          new Point2D(20, 20)),
                                          new LineSegment(new Point2D(10, 20),
                                                          new Point2D(20, 30)))
                .SetName("Diagonally parallel");
        }

        #endregion
    }
}