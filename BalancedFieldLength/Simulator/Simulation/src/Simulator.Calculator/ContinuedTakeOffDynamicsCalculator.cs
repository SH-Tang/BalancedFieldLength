using System;
using Calculator.Data;
using Core.Common.Data;
using Simulator.Data;
using Simulator.Data.Helpers;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class which describes the calculation of the aircraft dynamics
    /// when the take off is continued after engine failure.
    /// </summary>
    public class ContinuedTakeOffDynamicsCalculator
    {
        private readonly AircraftData aircraftData;
        private readonly int numberOfFailedEngines;
        private readonly double density;
        private readonly double gravitationalAcceleration;
        private readonly AerodynamicsData aerodynamicsData;

        /// <summary>
        /// Creates a new instance of <see cref="ContinuedTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="aircraftData">Tee <see cref="AircraftData"/> which holds
        /// all the information of the aircraft to simulate.</param>
        /// <param name="numberOfFailedEngines">The number of engines which failed during takeoff.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration g0. [m/s^2]</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        public ContinuedTakeOffDynamicsCalculator(AircraftData aircraftData, int numberOfFailedEngines, double density, double gravitationalAcceleration)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            this.aircraftData = aircraftData;
            this.numberOfFailedEngines = numberOfFailedEngines;
            this.density = density;
            this.gravitationalAcceleration = gravitationalAcceleration;
            aerodynamicsData = aircraftData.AerodynamicsData;
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

        private static double GetNewton(double kiloNewton)
        {
            return kiloNewton * 1000;
        }

        #region Calculate Rates

        private static double CalculateClimbRate(AircraftState aircraftState)
        {
            return aircraftState.TrueAirspeed * Math.Sin(aircraftState.FlightPathAngle.Radians);
        }

        private Angle CalculatePitchRate(AircraftState aircraftState)
        {
            return ShouldRotate(aircraftState) ? Angle.FromDegrees(aircraftData.PitchAngleGradient) : new Angle();
        }

        private bool ShouldRotate(AircraftState aircraftState)
        {
            double rotationSpeed = 1.2 * AerodynamicsHelper.CalculateStallSpeed(aerodynamicsData,
                                                                                GetNewton(aircraftData.TakeOffWeight),
                                                                                density);

            return aircraftState.TrueAirspeed >= rotationSpeed
                   && aircraftState.PitchAngle.Degrees < aircraftData.MaximumPitchAngle;
        }

        private double CalculateTrueAirSpeedRate(AircraftState aircraftState)
        {
            return (gravitationalAcceleration * (CalculateThrust()
                                                 - CalculateDragForce(aircraftState) - CalculateRollDrag(aircraftState)
                                                 - GetNewton(aircraftData.TakeOffWeight) * Math.Sin(aircraftState.FlightPathAngle.Radians)))
                   / GetNewton(aircraftData.TakeOffWeight);
        }

        private Angle CalculateFlightPathAngleRate(AircraftState state)
        {
            if (state.TrueAirspeed < 1)
            {
                return new Angle();
            }

            double acceleration = (gravitationalAcceleration * (CalculateLift(state) - GetNewton(aircraftData.TakeOffWeight) + CalculateNormalForce(state)))
                                  / (GetNewton(aircraftData.TakeOffWeight) * state.TrueAirspeed);
            return Angle.FromRadians(acceleration);
        }

        #endregion

        #region Calculate Forces

        private double CalculateLift(AircraftState state)
        {
            return AerodynamicsHelper.CalculateLift(aircraftData.AerodynamicsData,
                                                    CalculateAngleOfAttack(state),
                                                    density,
                                                    state.TrueAirspeed);
        }

        private static Angle CalculateAngleOfAttack(AircraftState state)
        {
            return state.PitchAngle - state.FlightPathAngle;
        }

        private double CalculateDragForce(AircraftState state)
        {
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(aerodynamicsData,
                                                                                 CalculateAngleOfAttack(state));
            return AerodynamicsHelper.CalculateDragWithEngineFailure(aerodynamicsData,
                                                                     liftCoefficient,
                                                                     density,
                                                                     state.TrueAirspeed);
        }

        private double CalculateRollDrag(AircraftState state)
        {
            return aircraftData.RollingResistanceCoefficient * CalculateNormalForce(state);
        }

        private double CalculateNormalForce(AircraftState state)
        {
            double normalForce = GetNewton(aircraftData.TakeOffWeight) - CalculateLift(state);
            if (state.Height >= 0.01 || normalForce < 0)
            {
                return 0;
            }

            return normalForce;
        }

        private double CalculateThrust()
        {
            return (aircraftData.NrOfEngines - numberOfFailedEngines) * GetNewton(aircraftData.MaximumThrustPerEngine);
        }

        #endregion
    }
}