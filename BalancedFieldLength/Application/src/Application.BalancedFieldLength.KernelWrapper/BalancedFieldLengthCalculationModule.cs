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
using System.Collections.Generic;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using Application.BalancedFieldLength.KernelWrapper.Factories;
using Simulator.Kernel;
using AircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper
{
    /// <summary>
    /// Class which performs the balanced field length calculation.
    /// </summary>
    public static class BalancedFieldLengthCalculationModule
    {
        /// <summary>
        /// Validates a <see cref="BalancedFieldLengthCalculation"/>.
        /// </summary>
        /// <param name="calculation">The calculation to validate.</param>
        /// <returns>A collection of validation messages if the validation failed, empty otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculation"/>
        /// is <c>null</c>.</exception>
        public static IEnumerable<string> Validate(BalancedFieldLengthCalculation calculation)
        {
            if (calculation == null)
            {
                throw new ArgumentNullException(nameof(calculation));
            }

            try
            {
                IAggregatedDistanceCalculatorKernel kernel = BalancedFieldLengthKernelFactory.Instance.CreateDistanceCalculatorKernel();
                EngineData engineData = calculation.EngineData;
                GeneralSimulationSettingsData generalSimulationSettings = calculation.SimulationSettings;

                AircraftData aircraftData = AircraftDataFactory.Create(calculation.AircraftData, engineData);
                KernelValidationError validationResult = kernel.Validate(aircraftData,
                                                                         generalSimulationSettings.Density,
                                                                         generalSimulationSettings.GravitationalAcceleration,
                                                                         engineData.NrOfFailedEngines);

                var messages = new List<string>();
                if (validationResult.HasFlag(KernelValidationError.InvalidDensity))
                {
                    messages.Add("Density is invalid.");
                }

                if (validationResult.HasFlag(KernelValidationError.InvalidGravitationalAcceleration))
                {
                    messages.Add("Gravitational acceleration is invalid.");
                }

                if (validationResult.HasFlag(KernelValidationError.InvalidNrOfFailedEngines))
                {
                    messages.Add("Number of failed engines is invalid.");
                }

                return messages;
            }
            catch (CreateKernelDataException e)
            {
                return new[]
                {
                    e.Message
                };
            }
        }
    }
}