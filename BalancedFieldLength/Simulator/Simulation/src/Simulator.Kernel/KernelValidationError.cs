namespace Simulator.Kernel
{
    /// <summary>
    /// Enum representing the kernel validation errors.
    /// </summary>
    public enum KernelValidationError
    {
        /// <summary>
        /// Represents an invalid density.
        /// </summary>
        InvalidDensity,

        /// <summary>
        /// Represents an invalid gravitational acceleration.
        /// </summary>
        InvalidGravitationalAcceleration,

        /// <summary>
        /// Indicates an invalid number of failed engines
        /// </summary>
        InvalidNrOfFailedEngines
    }
}