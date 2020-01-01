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

namespace Application.BalancedFieldLength.KernelWrapper.Factories
{
    /// <summary>
    /// Factory to create <see cref="CalculationSettings"/>.
    /// </summary>
    public static class CalculationSettingsFactory
    {
        /// <summary>
        /// Creates a <see cref="CalculationSettings"/> based on <see cref="GeneralSimulationSettingsData"/>.
        /// </summary>
        /// <param name="settings">The <see cref="GeneralSimulationSettingsData"/> to create
        /// a <see cref="CalculationSettings"/> for.</param>
        /// <param name="failureSpeed">The failure speed to create the settings for. [m/s]</param>
        /// <returns>A <see cref="CalculationSettings"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="CreateKernelDataException">Thrown when <see cref="CalculationSettings"/>
        /// could not be successfully created.</exception>
        public static CalculationSettings Create(GeneralSimulationSettingsData settings, int failureSpeed)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            try
            {
                return new CalculationSettings(failureSpeed, settings.MaximumNrOfIterations, settings.TimeStep);
            }
            catch (ArgumentException e)
            {
                throw new CreateKernelDataException(e.Message, e);
            }
        }
    }
}