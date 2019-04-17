namespace Simulator.Calculator.BalancedFieldLengthCalculator
{
    /// <summary>
    /// Class representing the balanced field length output.
    /// </summary>
    public class BalancedFieldLength
    {
        /// <summary>
        /// Creates a new instance of <see cref="BalancedFieldLength"/>.
        /// </summary>
        /// <param name="velocity">The velocity at which the balanced field length occurs. [m/s]</param>
        /// <param name="distance">The distance at which the balanced field length occurs. [m]</param>
        internal BalancedFieldLength(double velocity, double distance)
        {
            Velocity = velocity;
            Distance = distance;
        }

        /// <summary>
        /// Gets the velocity at which the balanced field length occurs. [m/s]
        /// </summary>
        public double Velocity { get; }

        /// <summary>
        /// Gets the distance covered at the balanced field length. [m]
        /// </summary>
        public double Distance { get; }
    }
}