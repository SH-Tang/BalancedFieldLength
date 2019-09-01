namespace Application.BalancedFieldLength.Controls
{
    /// <summary>
    /// View model for displaying the output.
    /// </summary>
    public class OutputViewModel
    {
        /// <summary>
        /// Creates a new instance of <see cref="OutputViewModel"/>.
        /// </summary>
        public OutputViewModel()
        {
            BalancedFieldLengthDistance = double.NaN;
            BalancedFieldLengthVelocity = double.NaN;
        }

        /// <summary>
        /// Gets the balanced field length distance. [m]
        /// </summary>
        public double BalancedFieldLengthDistance { get; }

        /// <summary>
        /// Gets the velocity at which the balanced field length
        /// distance occurs. [m/s]
        /// </summary>
        public double BalancedFieldLengthVelocity { get; }
    }
}