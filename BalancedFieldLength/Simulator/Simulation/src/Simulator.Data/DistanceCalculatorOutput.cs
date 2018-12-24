namespace Simulator.Data
{
    /// <summary>
    /// Class to hold the calculated distance for a given failure speed.
    /// </summary>
    public class DistanceCalculatorOutput
    {
        /// <summary>
        /// Gets the speed at which the failure occurred. [m/s]
        /// </summary>
        public int FailureSpeed { get; }

        /// <summary>
        /// Gets the distance that was covered. [m]
        /// </summary>
        public double Distance { get; }

        /// <summary>
        /// Indicates whether the result was obtained before failure.
        /// </summary>
        public bool ConvergenceBeforeFailure { get; }

        /// <summary>
        /// Indicates whether the result was converged.
        /// </summary>
        public bool CalculationConverged { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculatorOutput"/>.
        /// </summary>
        /// <param name="failureSpeed">The speed at which the failure occurred. [m/s]</param>
        /// <param name="distance">The distance covered before the stopping criteria was satisfied. [m]</param>
        /// <param name="convergenceBeforeFailure">Indicates whether the result was obtained
        /// before the aircraft reached a failed state.</param>
        /// <param name="calculationConverged">Indicates whether the result was converged.</param>
        public DistanceCalculatorOutput(int failureSpeed, double distance, bool convergenceBeforeFailure,
            bool calculationConverged)
        {
            FailureSpeed = failureSpeed;
            Distance = distance;
            ConvergenceBeforeFailure = convergenceBeforeFailure;
            CalculationConverged = calculationConverged;
        }
    }
}