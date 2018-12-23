using Core.Common.Data;

namespace Simulator.Data
{
    /// <summary>
    /// Class which contains the aircraft states.
    /// </summary>
    public class AircraftState
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftState"/>
        /// with zero values for the states.
        /// </summary>
        public AircraftState() {}

        /// <summary>
        /// Creates a new instance of <see cref="AircraftState"/>.
        /// </summary>
        /// <param name="pitchAngle">The pitch angle.</param>
        /// <param name="flightPathAngle">The flight path angle.</param>
        /// <param name="trueAirspeed">The true airspeed. [m/s]</param>
        /// <param name="height">The height. [m]</param>
        /// <param name="distance">The horizontal distance from the starting point. [m]</param>
        public AircraftState(Angle pitchAngle, Angle flightPathAngle, double trueAirspeed, double height, double distance)
        {
            PitchAngle = pitchAngle;
            FlightPathAngle = flightPathAngle;
            TrueAirspeed = trueAirspeed;
            Height = height;
            Distance = distance;
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

        /// <summary>
        /// Gets the horizontal distance. [m]
        /// </summary>
        /// <remarks>Also denoted as x or d.</remarks>
        public double Distance { get; }
    }
}