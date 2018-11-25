using Core.Common.Data;

namespace Simulator.Calculator {
    /// <summary>
    /// Class which contains the aircraft states.
    /// </summary>
    public class AircraftState
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftState"/>.
        /// </summary>
        /// <param name="pitchAngle">The pitch angle.</param>
        /// <param name="flightPathAngle">The flight path angle.</param>
        /// <param name="trueAirspeed">The true airspeed. [m/s]</param>
        /// <param name="height">The height. [m]</param>
        public AircraftState(Angle pitchAngle, Angle flightPathAngle, double trueAirspeed, double height)
        {
            PitchAngle = pitchAngle;
            FlightPathAngle = flightPathAngle;
            TrueAirspeed = trueAirspeed;
            Height = height;
        }

        /// <summary>
        /// Gets the pitch angle.
        /// </summary>
        /// <remarks>Also denoted as theta.</remarks>
        public Angle PitchAngle { get; }

        /// <summary>
        /// Gets the flight path angle.
        /// </summary>
        /// <remarks>Also denoted as gamma.</remarks>
        public Angle FlightPathAngle { get; }

        /// <summary>
        /// Gets the true airspeed. [m/s]
        /// </summary>
        /// <remarks>Also denoted as Vtas.</remarks>
        public double TrueAirspeed { get; }

        /// <summary>
        /// Gets the height. [m]
        /// </summary>
        /// <remarks>Also denoted as h.</remarks>
        public double Height { get; }
    }
}