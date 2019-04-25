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
using Core.Common.Geometry;
using Simulator.Calculator.AggregatedDistanceCalculator;

namespace Simulator.Calculator.BalancedFieldLengthCalculator
{
    /// <summary>
    /// Calculator which calculates the balanced field length based distances on the
    /// covered of the rejected and the continued take off.
    /// </summary>
    public static class BalancedFieldLengthCalculator
    {
        /// <summary>
        /// Calculates the balanced field length by determining the intersection between the distances covered by the rejected
        /// and the continued take off.
        /// </summary>
        /// <param name="outputs">The collection of <see cref="AggregatedDistanceOutput"/> to determine the balanced field length for.</param>
        /// <returns>An <see cref="BalancedFieldLength"/> containing the information at which balanced field length occurs.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="outputs"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="outputs"/>
        /// <list type="bullet">
        /// <item>contains a duplicate definition for a certain failure speed,</item>
        /// <item>is empty,</item>
        /// <item>contains only one element.</item>
        /// </list></exception>
        /// <remarks>This method will sort <paramref name="outputs"/> in an ascending order based on the failure speed.
        /// The intersection is determined based on this sorted list.</remarks>
        public static BalancedFieldLength CalculateBalancedFieldLength(IEnumerable<AggregatedDistanceOutput> outputs)
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

            AggregatedDistanceOutput previousOutput = sortedOutputs.First();
            for (var i = 1; i < sortedOutputs.Count(); i++)
            {
                AggregatedDistanceOutput currentOutput = sortedOutputs.ElementAt(i);

                // Create line segments
                var continuedTakeOffSegment = new LineSegment(new Point2D(previousOutput.FailureSpeed, previousOutput.ContinuedTakeOffDistance),
                                                              new Point2D(currentOutput.FailureSpeed, currentOutput.ContinuedTakeOffDistance));

                var abortedTakeOffSegment = new LineSegment(new Point2D(previousOutput.FailureSpeed, previousOutput.AbortedTakeOffDistance),
                                                            new Point2D(currentOutput.FailureSpeed, currentOutput.AbortedTakeOffDistance));

                // Determine whether lines cross
                Point2D crossingPoint = Geometry2DHelper.DetermineLineIntersection(continuedTakeOffSegment, abortedTakeOffSegment);

                // Determine if the result is valid
                if (!double.IsNaN(crossingPoint.X) && !double.IsNaN(crossingPoint.Y))
                {
                    double intersectionDistance = crossingPoint.Y;
                    return new BalancedFieldLength(crossingPoint.X, intersectionDistance);
                }

                previousOutput = currentOutput;
            }

            return new BalancedFieldLength(double.NaN, double.NaN);
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
            var list = new SortedList<double, AggregatedDistanceOutput>();
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