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
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.Helpers;

namespace Simulator.Components.TakeOffDynamics
{
    /// <summary>
    /// Class which describes the calculation of the aircraft dynamics
    /// when the take off is continued after engine failure.
    /// </summary>
    public class ContinuedTakeOffDynamicsCalculator : TakeOffDynamicsCalculatorBase, IFailureTakeOffDynamicsCalculator
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

        #region Calculate Forces

        protected override double CalculateAerodynamicDragForce(AircraftState state)
        {
            double liftCoefficient = AerodynamicsHelper.CalculateLiftCoefficient(AerodynamicsData,
                                                                                 CalculateAngleOfAttack(state));
            return AerodynamicsHelper.CalculateDragWithEngineFailure(AerodynamicsData,
                                                                     liftCoefficient,
                                                                     density,
                                                                     state.TrueAirspeed);
        }

        protected override double GetFrictionCoefficient()
        {
            return aircraftData.RollingResistanceCoefficient;
        }

        protected override double CalculateThrustForce()
        {
            return (aircraftData.NrOfEngines - numberOfFailedEngines) * GetNewton(aircraftData.MaximumThrustPerEngine);
        }

        #endregion
    }
}