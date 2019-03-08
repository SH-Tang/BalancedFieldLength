using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Geometry;
using Simulator.Calculator.AggregatedDistanceCalculator;

namespace Simulator.Calculator.Helpers
{
    /// <summary>
    /// Helper class which contain functions determining the intersection between the
    /// distances covered of the rejected and the continued take off.
    /// </summary>
    public static class AggregatedDistanceCalculatorHelper
    {
        /// <summary>
        /// Determines the intersection between the distances covered by the rejected
        /// and the continued take off.
        /// </summary>
        /// <param name="outputs">The collection of <see cref="AggregatedDistanceOutput"/>
        /// to determine the intersection for.</param>
        /// <returns>An <see cref="AggregatedDistanceOutput"/> containing the information at which speed the distances intersect
        /// and at which distance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="outputs"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="outputs"/>
        /// <list type="bullet">
        /// <item>contains a duplicate definition for a certain failure speed,</item>
        /// <item>is empty,</item>
        /// <item>contains only one element.</item>
        /// </list></exception>
        /// <remarks>This method will sort <paramref name="outputs"/> in an ascending order based on the failure speed.
        /// The intersection is determined based on this sorted list.</remarks>
        public static AggregatedDistanceOutput DetermineCrossing(IEnumerable<AggregatedDistanceOutput> outputs)
        {
            if (outputs == null)
            {
                throw new ArgumentNullException(nameof(outputs));
            }

            IEnumerable<AggregatedDistanceOutput> sortedOutputs = SortOutputs(outputs);
            if (sortedOutputs.Count() <= 1)
            {
                throw new ArgumentException("Cannot determine crossing from a collection containing 0 or 1 item.");
            }

            AggregatedDistanceOutput firstOutput = sortedOutputs.First();
            for (int i = 1; i < sortedOutputs.Count(); i++)
            {
                AggregatedDistanceOutput currentOutput = sortedOutputs.ElementAt(i);

                // Create line segments
                var continuedTakeOffSegment = new LineSegment(new Point2D(firstOutput.FailureSpeed, firstOutput.ContinuedTakeOffDistance),
                                                              new Point2D(currentOutput.FailureSpeed, currentOutput.ContinuedTakeOffDistance));

                var abortedTakeOffSegment = new LineSegment(new Point2D(firstOutput.FailureSpeed, firstOutput.AbortedTakeOffDistance),
                                                            new Point2D(currentOutput.FailureSpeed, currentOutput.AbortedTakeOffDistance));

                // Determine whether lines cross
                Point2D crossingPoint = Geometry2DHelper.DetermineLineIntersection(continuedTakeOffSegment, abortedTakeOffSegment);

                // Determine if the result is valid
                if (!double.IsNaN(crossingPoint.X) && !double.IsNaN(crossingPoint.Y))
                {
                    double intersectionDistance = crossingPoint.Y;
                    return new AggregatedDistanceOutput(crossingPoint.X, intersectionDistance, intersectionDistance);
                }
            }

            return new AggregatedDistanceOutput(double.NaN, double.NaN, double.NaN);
        }

        /// <summary>
        /// Creates a sorted list based on the failure speeds of the outputs.
        /// </summary>
        /// <param name="outputs">The collection of <see cref="AggregatedDistanceOutput"/> to create a sorted list for.</param>
        /// <returns>A  sorted collection of <see cref="AggregatedDistanceOutput"/> in ascending order of the failure speed.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="outputs"/> contains duplicate definitions for
        /// a certain failure speed.</exception>
        private static IEnumerable<AggregatedDistanceOutput> SortOutputs(IEnumerable<AggregatedDistanceOutput> outputs)
        {
            SortedList<double, AggregatedDistanceOutput> list = new SortedList<double, AggregatedDistanceOutput>();
            foreach (AggregatedDistanceOutput output in outputs)
            {
                double failureSpeed = output.FailureSpeed;
                if (list.ContainsKey(failureSpeed))
                {
                    throw new ArgumentException($"Outputs cannot contain duplicate definitions for failure speed {failureSpeed}.");
                }

                list.Add(failureSpeed, output);
            }

            return list.Values.ToArray();
        }
    }
}