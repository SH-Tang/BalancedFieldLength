using System;
using Core.Common.Data;
using Simulator.Data;
using Simulator.Data.Helpers;

namespace Simulator.Calculator
{
    /// <summary>
    /// Base class which calculates the standard aircraft dynamics.
    /// </summary>
    public abstract class AircraftDynamicsCalculatorBase
    {
        private readonly double gravitationalAcceleration;

        /// <summary>
        /// Creates a new instance of <see cref="ContinuedTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="aircraftData">Tee <see cref="AircraftData"/> which holds
        /// all the information of the aircraft to simulate.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration g0. [m/s^2]</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        protected AircraftDynamicsCalculatorBase(AircraftData aircraftData,
                                                 double density,
                                                 double gravitationalAcceleration)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            AircraftData = aircraftData;
            AerodynamicsData = aircraftData.AerodynamicsData;
            Density = density;
            this.gravitationalAcceleration = gravitationalAcceleration;
        }

        /// <summary>
        /// Calculates the accelerations acting on the aircraft based
        /// on <see cref="AircraftState"/>.
        /// </summary>
        /// <param name="aircraftState">The <see cref="AircraftState"/>
        /// the aircraft is currently in.</param>
        /// <returns>The <see cref="AircraftAccelerations"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftState"/>
        /// is <c>null</c>.</exception>
        public AircraftAccelerations Calculate(AircraftState aircraftState)
        {
            if (aircraftState == null)
            {
                throw new ArgumentNullException(nameof(aircraftState));
            }

            return new AircraftAccelerations(CalculatePitchRate(aircraftState),
                                             CalculateClimbRate(aircraftState),
                                             CalculateTrueAirSpeedRate(aircraftState),
                                             CalculateFlightPathAngleRate(aircraftState));
        }

        /// <summary>
        /// Gets the <see cref="Data.AircraftData"/> from this calculator.
        /// </summary>
        protected AircraftData AircraftData { get; }

        /// <summary>
        /// Gets the <see cref="Data.AerodynamicsData"/> from this calculator.
        /// </summary>
        protected AerodynamicsData AerodynamicsData { get; }

        /// <summary>
        /// Gets the density. [kg/m^3]
        /// </summary>
        protected double Density { get; }

        /// <summary>
        /// Calculates the roll drag that's acting on the aircraft. [N]
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/>
        /// the aircraft is currently in.</param>
        /// <returns>The roll drag.</returns>
        protected abstract double CalculateRollDrag(AircraftState state);

        /// <summary>
        /// Calculates the thrust force. [N]
        /// </summary>
        /// <returns>The thrust force.</returns>
        protected abstract double CalculateThrust();

        /// <summary>
        /// Calculates the drag force that is acting on the aircraft. [N]
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/>
        /// the aircraft is currently in.</param>
        /// <returns>The drag force.</returns>
        protected abstract double CalculateDragForce(AircraftState state);

        /// <summary>
        /// Calculates the normal force. [N]
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/> the aircraft
        /// is currently in.</param>
        /// <returns>The normal force.</returns>
        protected double CalculateNormalForce(AircraftState state)
        {
            double normalForce = GetNewton(AircraftData.TakeOffWeight) - CalculateLift(state);
            if (state.Height >= 0.01 || normalForce < 0)
            {
                return 0;
            }

            return normalForce;
        }

        /// <summary>
        /// Calculates the pitch rate based on <paramref name="state"/>.
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/> the aircraft
        /// is currently in.</param>
        /// <returns>The pitch rate.</returns>
        protected abstract Angle CalculatePitchRate(AircraftState state);

        /// <summary>
        /// Calculates the angle of attack based on <paramref name="state"/>.
        /// </summary>
        /// <param name="state">The <see cref="AircraftState"/> the aircraft
        /// is currently in.</param>
        /// <returns>The angle of attack.</returns>
        protected static Angle CalculateAngleOfAttack(AircraftState state)
        {
            return state.PitchAngle - state.FlightPathAngle;
        }

        private static double GetNewton(double kiloNewton)
        {
            return kiloNewton * 1000;
        }

        private static double CalculateClimbRate(AircraftState aircraftState)
        {
            return aircraftState.TrueAirspeed * Math.Sin(aircraftState.FlightPathAngle.Radians);
        }

        private double CalculateTrueAirSpeedRate(AircraftState aircraftState)
        {
            return (gravitationalAcceleration * (CalculateThrust()
                                                 - CalculateDragForce(aircraftState) - CalculateRollDrag(aircraftState)
                                                 - GetNewton(AircraftData.TakeOffWeight) * Math.Sin(aircraftState.FlightPathAngle.Radians)))
                   / GetNewton(AircraftData.TakeOffWeight);
        }

        private Angle CalculateFlightPathAngleRate(AircraftState state)
        {
            if (state.TrueAirspeed < 1)
            {
                return new Angle();
            }

            double acceleration = (gravitationalAcceleration * (CalculateLift(state) - GetNewton(AircraftData.TakeOffWeight) + CalculateNormalForce(state)))
                                  / (GetNewton(AircraftData.TakeOffWeight) * state.TrueAirspeed);
            return Angle.FromRadians(acceleration);
        }

        private double CalculateLift(AircraftState state)
        {
            return AerodynamicsHelper.CalculateLift(AircraftData.AerodynamicsData,
                                                    CalculateAngleOfAttack(state),
                                                    Density,
                                                    state.TrueAirspeed);
        }
    }
}