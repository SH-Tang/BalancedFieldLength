// Copyright (C) 2018 Dennis Tang. All rights reserved.
//
// This file is part of Balanced Field Length.
//
// Balanced Field Length is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using Core.Common.Data;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Components.Integrators
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
            double integratedAngleInRadians = IntegrateState(state.Radians, timeDerivative.Radians, timeStep);
            return Angle.FromRadians(integratedAngleInRadians);
        }
    }
}