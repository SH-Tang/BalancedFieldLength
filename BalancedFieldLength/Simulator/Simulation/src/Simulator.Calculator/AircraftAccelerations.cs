namespace Simulator.Calculator {
    /// <summary>
    /// Class which contains the time derivatives of the aircraft states.
    /// </summary>
    public class AircraftAccelerations
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftAccelerations"/>.
        /// </summary>
        /// <param name="pitchRate">The pitch rate. [deg/s]</param>
        /// <param name="climbRate">The climb rate. [m/s]</param>
        /// <param name="trueAirSpeedRate">The true airspeed rate. [m/s^2]</param>
        /// <param name="flightPathRate">The flight path angle rate. [rad/s]</param>
        public AircraftAccelerations(double pitchRate, double climbRate, double trueAirSpeedRate, double flightPathRate)
        {
            PitchRate = pitchRate;
            ClimbRate = climbRate;
            TrueAirSpeedRate = trueAirSpeedRate;
            FlightPathRate = flightPathRate;
        }

        /// <summary>
        /// Gets the pitch rate. [deg/s]
        /// </summary>
        /// <remarks>Also denoted as dTheta/dt.</remarks>
        public double PitchRate { get; }

        /// <summary>
        /// Gets the climb rate. [m/s]
        /// </summary>
        /// <remarks>Also denoted as dh/dt.</remarks>
        public double ClimbRate { get; }

        /// <summary>
        /// Gets the true airspeed rate. [m/s^2]
        /// </summary>
        /// <remarks>Also denoted as dVtas/dt.</remarks>
        public double TrueAirSpeedRate { get; }

        /// <summary>
        /// Gets the flight path angle rate. [rad/s]
        /// </summary>
        /// <remarks>Also  denoted as dGamma/dt.</remarks>
        public double FlightPathRate { get; }
    }
}