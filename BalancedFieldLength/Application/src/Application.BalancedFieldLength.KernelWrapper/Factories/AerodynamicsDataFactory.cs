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
using KernelAerodynamicData = Simulator.Data.AerodynamicsData;

namespace Application.BalancedFieldLength.KernelWrapper.Factories
{
    /// <summary>
    /// Factory for creating <see cref="Simulator.Data.AerodynamicsData"/>.
    /// </summary>
    public static class AerodynamicsDataFactory
    {
        /// <summary>
        /// Creates an <see cref="Simulator.Data.AerodynamicsData"/> based on <see cref="AircraftData"/>.
        /// </summary>
        /// <param name="aircraftData">The <see cref="AircraftData"/> to create an
        /// <see cref="Simulator.Data.AerodynamicsData"/> for.</param>
        /// <returns>An <see cref="Simulator.Data.AerodynamicsData"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        public static KernelAerodynamicData Create(AircraftData aircraftData)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            return new KernelAerodynamicData(aircraftData.AspectRatio, aircraftData.WingSurfaceArea,
                                             aircraftData.ZeroLiftAngleOfAttack, aircraftData.LiftCoefficientGradient,
                                             aircraftData.MaximumLiftCoefficient, aircraftData.RestDragCoefficient,
                                             aircraftData.RestDragCoefficientWithEngineFailure, aircraftData.OswaldFactor);
        }
    }
}