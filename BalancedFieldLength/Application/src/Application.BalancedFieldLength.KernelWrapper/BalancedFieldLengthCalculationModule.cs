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
using Application.BalancedFieldLength.KernelWrapper.Properties;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.Integrators;
using Simulator.Components.Integrators;
using Simulator.Data;
using Simulator.Data.Exceptions;
using Simulator.Kernel;
using AircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper
{
    /// <summary>
    /// Module to validate and calculate the balanced field length calculation.
    /// </summary>
    public class BalancedFieldLengthCalculationModule : IBalancedFieldLengthCalculationModule
    {
        private readonly IAggregatedDistanceCalculatorKernel kernel;

        /// <summary>
        /// Creates a new instance of <see cref="BalancedFieldLengthCalculationModule"/>.
        /// </summary>
        public BalancedFieldLengthCalculationModule()
        {
            kernel = BalancedFieldLengthKernelFactory.Instance.CreateDistanceCalculatorKernel();
        }

        /// <summary>
        /// Validates a <see cref="BalancedFieldLengthCalculation"/>.
        /// </summary>
        /// <param name="calculation">The calculation to validate.</param>
        /// <returns>A collection of validation messages if the validation failed, empty otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculation"/>
        /// is <c>null</c>.</exception>
        public IEnumerable<string> Validate(BalancedFieldLengthCalculation calculation)
        {
            if (calculation == null)
            {
                throw new ArgumentNullException(nameof(calculation));
            }

            try
            {
                EngineData engineData = calculation.EngineData;
                GeneralSimulationSettingsData generalSimulationSettings = calculation.SimulationSettings;

                AircraftData aircraftData = AircraftDataFactory.Create(calculation.AircraftData, engineData);
                KernelValidationError validationResult = kernel.Validate(aircraftData,
                                                                         generalSimulationSettings.Density,
                                                                         generalSimulationSettings.GravitationalAcceleration,
                                                                         engineData.NrOfFailedEngines);

                var messages = new List<string>();
                if (generalSimulationSettings.EndFailureVelocity < 1)
                {
                    messages.Add(Resources.BalancedFieldLengthCalculationModule_ValidationMessage_End_failure_velocity_must_be_larger_than_Zero);
                }

                if (validationResult.HasFlag(KernelValidationError.InvalidDensity))
                {
                    messages.Add(Resources.BalancedFieldLengthCalculationModule_ValidationMessage_Invalid_Density);
                }

                if (validationResult.HasFlag(KernelValidationError.InvalidGravitationalAcceleration))
                {
                    messages.Add(Resources.BalancedFieldLengthCalculationModule_ValidationMessage_Invalid_GravitationalAcceleration);
                }

                if (validationResult.HasFlag(KernelValidationError.InvalidNrOfFailedEngines))
                {
                    messages.Add(Resources.BalancedFieldLengthCalculationModule_ValidationMessage_Invalid_NrOfFailedEngines);
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

        /// <summary>
        /// Calculates the <see cref="BalancedFieldLengthOutput"/> based on <paramref name="calculation"/>.
        /// </summary>
        /// <param name="calculation">The <see cref="BalancedFieldLengthCalculation"/> to calculate for.</param>
        /// <returns>A <see cref="BalancedFieldLengthOutput"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="calculation"/> is <c>null</c>.</exception>
        /// <exception cref="CreateKernelDataException">Thrown when the calculation input
        /// could not be created for the kernel.</exception>
        /// <exception cref="KernelCalculationException">Thrown when <see cref="AggregatedDistanceOutput"/>
        /// could not be calculated.</exception>
        public BalancedFieldLengthOutput Calculate(BalancedFieldLengthCalculation calculation)
        {
            if (calculation == null)
            {
                throw new ArgumentNullException(nameof(calculation));
            }

            GeneralSimulationSettingsData generalSimulationSettings = calculation.SimulationSettings;
            double density = generalSimulationSettings.Density;
            int endVelocity = generalSimulationSettings.EndFailureVelocity;
            double gravitationalAcceleration = generalSimulationSettings.GravitationalAcceleration;

            EngineData engineData = calculation.EngineData;
            int nrOfFailedEngines = engineData.NrOfFailedEngines;
            AircraftData aircraftData = AircraftDataFactory.Create(calculation.AircraftData, engineData);

            var integrator = new EulerIntegrator();

            var outputs = new List<AggregatedDistanceOutput>();
            for (var i = 0; i < endVelocity; i++)
            {
                var calculationInput = new CalculationInput(generalSimulationSettings,
                                                            i,
                                                            aircraftData,
                                                            integrator,
                                                            nrOfFailedEngines,
                                                            density,
                                                            gravitationalAcceleration);
                AggregatedDistanceOutput output = CalculateDistances(calculationInput);
                outputs.Add(output);
            }

            return BalancedFieldLengthOutputFactory.Create(outputs);
        }

        /// <summary>
        /// Calculates the <see cref="AggregatedDistanceOutput"/> based on the calculation input.
        /// </summary>
        /// <param name="calculationInput">The calculation input to calculate for.</param>
        /// <returns>A <see cref="AggregatedDistanceOutput"/>.</returns>
        /// <exception cref="CreateKernelDataException">Thrown when the calculation input
        /// could not be created for the kernel.</exception>
        /// <exception cref="KernelCalculationException">Thrown when <see cref="AggregatedDistanceOutput"/>
        /// could not be calculated.</exception>
        private AggregatedDistanceOutput CalculateDistances(CalculationInput calculationInput)
        {
            try
            {
                CalculationSettings simulationSettings = CalculationSettingsFactory.Create(calculationInput.GeneralSimulationSettings,
                                                                                           calculationInput.FailureVelocity);
                return kernel.Calculate(calculationInput.AircraftData,
                                        calculationInput.Integrator,
                                        calculationInput.NrOfFailedEngines,
                                        calculationInput.Density,
                                        calculationInput.GravitationalAcceleration,
                                        simulationSettings);
            }
            catch (CalculatorException e)
            {
                throw new KernelCalculationException(e.Message, e);
            }
        }

        private class CalculationInput
        {
            public CalculationInput(GeneralSimulationSettingsData generalSimulationSettings,
                                    int failureVelocity,
                                    AircraftData aircraftData,
                                    IIntegrator integrator,
                                    int nrOfFailedEngines,
                                    double density,
                                    double gravitationalAcceleration)
            {
                GeneralSimulationSettings = generalSimulationSettings;
                FailureVelocity = failureVelocity;
                AircraftData = aircraftData;
                Integrator = integrator;
                NrOfFailedEngines = nrOfFailedEngines;
                Density = density;
                GravitationalAcceleration = gravitationalAcceleration;
            }

            public GeneralSimulationSettingsData GeneralSimulationSettings { get; }
            public int FailureVelocity { get; }
            public AircraftData AircraftData { get; }
            public IIntegrator Integrator { get; }
            public int NrOfFailedEngines { get; }
            public double Density { get; }
            public double GravitationalAcceleration { get; }
        }
    }
}