using System;
using Core.Common.Data;
using Simulator.Data;
using Simulator.Data.Helpers;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class which describes the calculation of the aircraft dynamics
    /// when there is no engine failure.
    /// </summary>
    public class NormalTakeOffDynamicsCalculator : AircraftDynamicsCalculatorBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="NormalTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="aircraftData">Tee <see cref="AircraftData"/> which holds
        /// all the information of the aircraft to simulate.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration g0. [m/s^2]</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        public NormalTakeOffDynamicsCalculator(AircraftData aircraftData, double density, double gravitationalAcceleration)
            : base(aircraftData, density, gravitationalAcceleration) {}

        protected override double CalculateRollDrag(AircraftState state)
        {
            return AircraftData.RollingResistanceCoefficient * CalculateNormalForce(state);
        }

        protected override double CalculateThrust()
        {
            return AircraftData.NrOfEngines * GetNewton(AircraftData.MaximumThrustPerEngine);
        }

        protected override double CalculateDragForce(AircraftState state)
        {
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(AerodynamicsData,
                                                                                 CalculateAngleOfAttack(state));
            return AerodynamicsHelper.CalculateDragWithoutEngineFailure(AerodynamicsData,
                                                                        liftCoefficient,
                                                                        Density,
                                                                        state.TrueAirspeed);
        }

        protected override Angle CalculatePitchRate(AircraftState state)
        {
            return ShouldRotate(state) ? AircraftData.PitchAngleGradient : new Angle();
        }

        private bool ShouldRotate(AircraftState aircraftState)
        {
            double rotationSpeed = 1.2 * AerodynamicsHelper.CalculateStallSpeed(AerodynamicsData,
                                                                                GetNewton(AircraftData.TakeOffWeight),
                                                                                Density);

            return aircraftState.TrueAirspeed >= rotationSpeed
                   && aircraftState.PitchAngle < AircraftData.MaximumPitchAngle;
        }
    }
}