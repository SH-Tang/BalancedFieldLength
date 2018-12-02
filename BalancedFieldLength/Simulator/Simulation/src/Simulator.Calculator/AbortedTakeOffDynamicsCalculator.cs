using System;
using Core.Common.Data;
using Simulator.Data;
using Simulator.Data.Helpers;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class which describes the calculation of the aircraft dynamics
    /// when the take off is aborted after engine failure.
    /// </summary>
    public class AbortedTakeOffDynamicsCalculator : AircraftDynamicsCalculatorBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="ContinuedTakeOffDynamicsCalculator"/>.
        /// </summary>
        /// <param name="aircraftData">Tee <see cref="AircraftData"/> which holds
        /// all the information of the aircraft to simulate.</param>
        /// <param name="density">The air density. [kg/m3]</param>
        /// <param name="gravitationalAcceleration">The gravitational acceleration g0. [m/s^2]</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        public AbortedTakeOffDynamicsCalculator(AircraftData aircraftData, double density, double gravitationalAcceleration)
            : base(aircraftData, density, gravitationalAcceleration) {}

        protected override double GetFrictionCoefficient()
        {
            return AircraftData.BrakingResistanceCoefficient;
        }

        protected override double CalculateThrustForce()
        {
            return 0;
        }

        protected override double CalculateAerodynamicDragForce(AircraftState state)
        {
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(AerodynamicsData,
                                                                                 CalculateAngleOfAttack(state));
            return AerodynamicsHelper.CalculateDragWithEngineFailure(AerodynamicsData,
                                                                     liftCoefficient,
                                                                     Density,
                                                                     state.TrueAirspeed);
        }

        protected override Angle CalculatePitchRate(AircraftState state)
        {
            return new Angle();
        }
    }
}