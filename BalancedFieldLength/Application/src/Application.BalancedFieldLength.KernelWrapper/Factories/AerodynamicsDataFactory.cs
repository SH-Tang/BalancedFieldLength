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
using KernelAerodynamicsData = Simulator.Data.AerodynamicsData;

namespace Application.BalancedFieldLength.KernelWrapper.Factories
{
    /// <summary>
    /// Factory for creating <see cref="KernelAerodynamicsData"/>.
    /// </summary>
    public static class AerodynamicsDataFactory
    {
        /// <summary>
        /// Creates an <see cref="KernelAerodynamicsData"/> based on <see cref="AircraftData"/>.
        /// </summary>
        /// <param name="aircraftData">The <see cref="AircraftData"/> to create an
        /// <see cref="KernelAerodynamicsData"/> for.</param>
        /// <returns>An <see cref="KernelAerodynamicsData"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aircraftData"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="CreateKernelDataException">Thrown when the <see cref="KernelAerodynamicsData"/>
        /// could not be created.</exception>
        public static KernelAerodynamicsData Create(AircraftData aircraftData)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            try
            {
                return new KernelAerodynamicsData(aircraftData.AspectRatio, aircraftData.WingSurfaceArea,
                                                  aircraftData.ZeroLiftAngleOfAttack, aircraftData.LiftCoefficientGradient,
                                                  aircraftData.MaximumLiftCoefficient, aircraftData.RestDragCoefficient,
                                                  aircraftData.RestDragCoefficientWithEngineFailure, aircraftData.OswaldFactor);
            }
            catch (Exception e) when(e is ArgumentException)
            {
                throw new CreateKernelDataException(e.Message, e);
            }
        }
    }
}