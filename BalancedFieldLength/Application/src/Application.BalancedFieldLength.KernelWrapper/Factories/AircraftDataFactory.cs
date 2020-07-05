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
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using Simulator.Data;
using AircraftData = Application.BalancedFieldLength.Data.AircraftData;
using KernelAircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper.Factories
{
    /// <summary>
    /// Factory for creating <see cref="Simulator.Data.AircraftData"/>.
    /// </summary>
    public static class AircraftDataFactory
    {
        /// <summary>
        /// Creates an <see cref="KernelAircraftData"/> based on its input arguments.
        /// </summary>
        /// <param name="aircraftData">The <see cref="Data.AircraftData"/> to create an
        /// <see cref="KernelAircraftData"/> for.</param>
        /// <param name="engineData">The <see cref="EngineData"/> to create the <see cref="KernelAircraftData"/>
        /// for.</param>
        /// <returns>An <see cref="KernelAircraftData"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any argument is <c>null</c>.</exception>
        /// <exception cref="CreateKernelDataException">Thrown when the <see cref="KernelAircraftData"/>
        /// could not be created.</exception>
        public static KernelAircraftData Create(AircraftData aircraftData, EngineData engineData)
        {
            if (engineData == null)
            {
                throw new ArgumentNullException(nameof(engineData));
            }

            AerodynamicsData aerodynamicsData = AerodynamicsDataFactory.Create(aircraftData);

            try
            {
                return new KernelAircraftData(engineData.NrOfEngines, engineData.ThrustPerEngine, aircraftData.TakeOffWeight,
                                              aircraftData.PitchGradient, aircraftData.MaximumPitchAngle,
                                              aircraftData.RollResistanceCoefficient, aircraftData.RollResistanceWithBrakesCoefficient,
                                              aerodynamicsData);
            }
            catch (ArgumentException e)
            {
                throw new CreateKernelDataException(e.Message, e);
            }
        }
    }
}