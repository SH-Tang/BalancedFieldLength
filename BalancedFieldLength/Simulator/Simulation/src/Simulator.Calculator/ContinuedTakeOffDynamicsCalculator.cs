using System;
using Core.Common.Data;
using Simulator.Data;
using Simulator.Data.Helpers;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class which describes the calculation of the aircraft dynamics
    /// when the take off is continued after engine failure.
    /// </summary>
    public class ContinuedTakeOffDynamicsCalculator : AircraftDynamicsCalculatorBase
    {
        private readonly AircraftData aircraftData;
        private readonly int numberOfFailedEngines;
        private readonly double density;

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
            : base(aircraftData, density, gravitationalAcceleration)
        {
            this.aircraftData = aircraftData;
            this.numberOfFailedEngines = numberOfFailedEngines;
            this.density = density;
        }

        #region Calculate Rates

        protected override Angle CalculatePitchRate(AircraftState state)
        {
            return ShouldRotate(state) ? aircraftData.PitchAngleGradient : new Angle();
        }

        private bool ShouldRotate(AircraftState aircraftState)
        {
            double rotationSpeed = 1.2 * AerodynamicsHelper.CalculateStallSpeed(AerodynamicsData,
                                                                                GetNewton(aircraftData.TakeOffWeight),
                                                                                density);

            return aircraftState.TrueAirspeed >= rotationSpeed
                   && aircraftState.PitchAngle < aircraftData.MaximumPitchAngle;
        }

        #endregion

        #region Calculate Forces

        protected override double CalculateDragForce(AircraftState state)
        {
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(AerodynamicsData,
                                                                                 CalculateAngleOfAttack(state));
            return AerodynamicsHelper.CalculateDragWithEngineFailure(AerodynamicsData,
                                                                     liftCoefficient,
                                                                     density,
                                                                     state.TrueAirspeed);
        }

        protected override double CalculateRollDrag(AircraftState state)
        {
            return aircraftData.RollingResistanceCoefficient * CalculateNormalForce(state);
        }

        protected override double CalculateThrust()
        {
            return (aircraftData.NrOfEngines - numberOfFailedEngines) * GetNewton(aircraftData.MaximumThrustPerEngine);
        }

        #endregion
    }
}