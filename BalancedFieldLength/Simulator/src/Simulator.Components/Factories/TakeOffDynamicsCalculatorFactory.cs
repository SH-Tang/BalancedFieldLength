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

using Simulator.Calculator.TakeOffDynamics;
using Simulator.Components.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Components.Factories
{
    /// <summary>
    /// Factory to create take off dynamics calculators.
    /// </summary>
    public class TakeOffDynamicsCalculatorFactory : ITakeOffDynamicsCalculatorFactory
    {
        public INormalTakeOffDynamicsCalculator CreateNormalTakeOffDynamics(AircraftData data,
                                                                            double density,
                                                                            double gravitationalAcceleration)
        {
            return new NormalTakeOffDynamicsCalculator(data, density, gravitationalAcceleration);
        }

        public IFailureTakeOffDynamicsCalculator CreateAbortedTakeOffDynamics(AircraftData data,
                                                                              double density,
                                                                              double gravitationalAcceleration)
        {
            return new AbortedTakeOffDynamicsCalculator(data, density, gravitationalAcceleration);
        }

        public IFailureTakeOffDynamicsCalculator CreateContinuedTakeOffDynamicsCalculator(AircraftData data,
                                                                                          int nrOfFailedEngines,
                                                                                          double density,
                                                                                          double gravitationalAcceleration)
        {
            return new ContinuedTakeOffDynamicsCalculator(data, nrOfFailedEngines, density, gravitationalAcceleration);
        }
    }
}