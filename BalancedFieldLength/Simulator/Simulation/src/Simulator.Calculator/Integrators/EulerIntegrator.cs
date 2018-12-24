using System;
using Core.Common.Data;
using Simulator.Data;

namespace Simulator.Calculator.Integrators
{
    /// <summary>
    /// Class which solves the dynamic system by means of Euler integration.
    /// </summary>
    public class EulerIntegrator : IIntegrator
    {
        public AircraftState Integrate(AircraftState state, AircraftAccelerations accelerations, double timeStep)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (accelerations == null)
            {
                throw new ArgumentNullException(nameof(accelerations));
            }
            
            return new AircraftState(IntegrateState(state.PitchAngle, accelerations.PitchRate, timeStep),
                IntegrateState(state.FlightPathAngle, accelerations.FlightPathRate, timeStep),
                IntegrateState(state.TrueAirspeed, accelerations.TrueAirSpeedRate, timeStep),
                IntegrateState(state.Height, accelerations.ClimbRate, timeStep),
                IntegrateState(state.Distance, state.TrueAirspeed, timeStep));
        }

        private static double IntegrateState(double state, double timeDerivative, double timeStep)
        {
            return state + timeDerivative * timeStep;
        }

        private static Angle IntegrateState(Angle state, Angle timeDerivative, double timeStep)
        {
            var integratedAngleInRadians = IntegrateState(state.Radians, timeDerivative.Radians, timeStep);
            return Angle.FromRadians(integratedAngleInRadians);
        }
    }
}