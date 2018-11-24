namespace Simulator.Data.TestUtil
{
    /// <summary>
    /// Class which contains constants which can be used for testing.
    /// </summary>
    public static class SimulationConstants
    {
        /// <summary>
        /// Represents the tolerance of double deviations due to rounding.
        /// </summary>
        public const double Tolerance = 10e-6;

        /// <summary>
        /// Represents the gravitational acceleration. m/s^2
        /// </summary>
        public const double GravitationalAcceleration = 9.81;

        /// <summary>
        /// Represents the air density. [kg/m^3] 
        /// </summary>
        public const double Density = 1.225;
    }
}