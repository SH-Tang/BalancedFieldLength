using System;

namespace Calculator.Data
{
    /// <summary>
    /// Class to hold all the relevant properties of an  aircraft.
    /// </summary>
    public class AircraftData
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftData"/>.
        /// </summary>
        /// <param name="nrOfEngines">The number of engines.</param>
        /// <param name="maximumThrustPerEngine">The maximum thrust per engine. [kN]</param>
        /// <param name="takeOffWeight">The takeoff weight. [kN]</param>
        /// <param name="pitchAngleGradient">The rate of pitch angle during rotation. [deg/s]</param>
        /// <param name="maximumPitchAngle">The maximum pitch angle during rotation. [deg]</param>
        /// <param name="rollingResistanceCoefficient">The rolling resistance coefficient. [-]</param>
        /// <param name="brakingResistanceCoefficient">The brake resistance coefficient. [-]</param>
        /// <param name="aerodynamicData">The <see cref="Calculator.Data.AerodynamicData"/>
        /// holding all aerodynamic properties of the aircraft.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicData"/>
        /// is <c>null</c>.</exception>
        public AircraftData(int nrOfEngines, double maximumThrustPerEngine,
                            double takeOffWeight,
                            double pitchAngleGradient, double maximumPitchAngle,
                            double rollingResistanceCoefficient, double brakingResistanceCoefficient,
                            AerodynamicData aerodynamicData)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            NrOfEngines = nrOfEngines;
            MaximumThrustPerEngine = maximumThrustPerEngine;
            TakeOffWeight = takeOffWeight;
            PitchAngleGradient = pitchAngleGradient;
            MaximumPitchAngle = maximumPitchAngle;
            RollingResistanceCoefficient = rollingResistanceCoefficient;
            BrakingResistanceCoefficient = brakingResistanceCoefficient;
            AerodynamicData = aerodynamicData;
        }

        /// <summary>
        /// Gets the number of engines of the aircraft.
        /// </summary>
        public int NrOfEngines { get; }

        /// <summary>
        /// Gets the maximum thrust per engine of the aircraft. [kN]
        /// </summary>
        public double MaximumThrustPerEngine { get; }

        /// <summary>
        /// Gets the take off weight of the aircraft. [kN]
        /// </summary>
        public double TakeOffWeight { get; }

        /// <summary>
        /// Gets the pitch angle gradient during rotation. [deg/s]
        /// </summary>
        /// <remarks>Also denoted as dTheta/ds.</remarks>
        public double PitchAngleGradient { get; }

        /// <summary>
        /// Gets the maximum pitch angle during rotation. [deg]
        /// </summary>
        /// <remarks>Also denoted as theta.</remarks>
        public double MaximumPitchAngle { get; }

        /// <summary>
        /// Gets the rolling resistance coefficient. [-]
        /// </summary>
        /// <remarks>Also denoted as Mu_roll.</remarks>
        public double RollingResistanceCoefficient { get; }

        /// <summary>
        /// Gets the braking resistance coefficient. [-]
        /// </summary>
        /// <remarks>Also denoted as Mu_brake.</remarks>
        public double BrakingResistanceCoefficient { get; }

        /// <summary>
        /// Gets the <see cref="Calculator.Data.AerodynamicData"/> containing all the aerodynamic
        /// data of the aircraft.
        /// </summary>
        public AerodynamicData AerodynamicData { get; }
    }
}