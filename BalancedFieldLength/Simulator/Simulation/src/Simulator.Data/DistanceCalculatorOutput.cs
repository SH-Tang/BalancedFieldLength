namespace Simulator.Data
{
    /// <summary>
    /// Class to hold the calculated distance for a given failure speed.
    /// </summary>
    public class DistanceCalculatorOutput
    {
        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculatorOutput"/>.
        /// </summary>
        /// <param name="failureSpeed">The speed at which the failure occurred. [m/s]</param>
        /// <param name="distance">The distance covered before the stopping criteria was satisfied. [m]</param>
        public DistanceCalculatorOutput(int failureSpeed, double distance)
        {
            FailureSpeed = failureSpeed;
            Distance = distance;
        }

        /// <summary>
        /// Gets the speed at which the failure occurred. [m/s]
        /// </summary>
        public int FailureSpeed { get; }

        /// <summary>
        /// Gets the distance that was covered. [m]
        /// </summary>
        public double Distance { get; }
    }
}