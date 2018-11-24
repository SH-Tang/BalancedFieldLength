namespace Simulator.Calculator {
    /// <summary>
    /// Class which contains the time derivatives of the aircraft states.
    /// </summary>
    public class AircraftAccelerations
    {
        /// <summary>
        /// Gets the pitch rate. [deg/s]
        /// </summary>
        /// <remarks>Also denoted as dTheta/dt.</remarks>
        public double PitchRate { get; set; }

        /// <summary>
        /// Gets the climb rate. [m/s]
        /// </summary>
        /// <remarks>Also denoted as dh/dt.</remarks>
        public double ClimbRate { get; set; }

        /// <summary>
        /// Gets the true airspeed rate. [m/s^2]
        /// </summary>
        /// <remarks>Also denoted as dVtas/dt.</remarks>
        public double TrueAirSpeedRate { get; set; }

        /// <summary>
        /// Gets the flight path angle rate. [rad/s]
        /// </summary>
        /// <remarks>Also  denoted as dGamma/dt.</remarks>
        public double FlightPathRate { get; set; }
    }
}