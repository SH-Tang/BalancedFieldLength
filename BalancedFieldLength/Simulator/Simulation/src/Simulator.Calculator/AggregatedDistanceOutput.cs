namespace Simulator.Calculator
{
    /// <summary>
    /// Class to hold the aggregated output of the calculated distances of a continued
    /// and a rejected take off for a given failure speed.
    /// </summary>
    public class AggregatedDistanceOutput
    {
        /// <summary>
        /// Creates a new instance of <see cref="AggregatedDistanceOutput"/>.
        /// </summary>
        /// <param name="failureSpeed">The failure speed for which the output was calculated. [m/s]</param>
        /// <param name="abortedTakeOffDistance">The distance belonging to an aborted take off. [m]</param>
        /// <param name="continuedTakeOffDistance">The distance belonging to a continued take off. [m]</param>
        public AggregatedDistanceOutput(int failureSpeed, double abortedTakeOffDistance, double continuedTakeOffDistance)
        {
            FailureSpeed = failureSpeed;
            AbortedTakeOffDistance = abortedTakeOffDistance;
            ContinuedTakeOffDistance = continuedTakeOffDistance;
        }

        /// <summary>
        /// Gets the failure speed. [m/s]
        /// </summary>
        public double FailureSpeed { get; }

        /// <summary>
        /// Gets the distance of an aborted take off. [m]
        /// </summary>
        public double AbortedTakeOffDistance { get; }

        /// <summary>
        /// Gets the distance of a continued take off. [m]
        /// </summary>
        public double ContinuedTakeOffDistance { get; }
    }
}