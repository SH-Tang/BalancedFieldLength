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
using System.Linq;
using Application.BalancedFieldLength.Data;
using Application.BalancedFieldLength.KernelWrapper.Exceptions;
using Application.BalancedFieldLength.KernelWrapper.TestUtils;
using Core.Common.TestUtil;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Components.Integrators;
using Simulator.Data;
using Simulator.Data.Exceptions;
using Simulator.Kernel;
using AircraftData = Application.BalancedFieldLength.Data.AircraftData;
using KernelAircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper.Test
{
    [TestFixture]
    public class BalancedFieldLengthCalculationModuleTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var calculationModule = new BalancedFieldLengthCalculationModule();

            // Assert
            Assert.That(calculationModule, Is.InstanceOf<IBalancedFieldLengthCalculationModule>());
        }

        [Test]
        public void Validate_CalculationNull_ThrowsArgumentNullException()
        {
            // Setup 
            var module = new BalancedFieldLengthCalculationModule();

            // Call
            TestDelegate call = () => module.Validate(null);

            Assert.That(call, Throws.ArgumentNullException
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("calculation"));
        }

        [Test]
        public void GivenInvalidAircraftDataCausingException_WhenCallingValidate_ReturnsErrorMessages()
        {
            // Setup
            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);

            calculation.AircraftData.AspectRatio = double.NaN;

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                IEnumerable<string> messages = module.Validate(calculation);

                // Assert
                CollectionAssert.AreEqual(new[]
                {
                    "aspectRatio must be a concrete number and cannot be NaN or Infinity."
                }, messages);
            }
        }

        [Test]
        public void Validate_Always_FactoryReceiveCorrectData()
        {
            // Setup
            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);
            SetSimulationSettingsData(calculation.SimulationSettings);

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(Arg.Any<KernelAircraftData>(),
                            Arg.Any<double>(),
                            Arg.Any<double>(),
                            Arg.Any<int>())
                  .ReturnsForAnyArgs(KernelValidationError.None);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                module.Validate(calculation);

                // Assert
                ICall calls = kernel.ReceivedCalls().Single();
                object[] arguments = calls.GetArguments();
                AircraftDataTestHelper.AssertAircraftData(calculation.AircraftData,
                                                          calculation.EngineData,
                                                          (KernelAircraftData) arguments[0]);

                GeneralSimulationSettingsData simulationSettings = calculation.SimulationSettings;
                Assert.That(arguments[1], Is.EqualTo(simulationSettings.Density));
                Assert.That(arguments[2], Is.EqualTo(simulationSettings.GravitationalAcceleration));
                Assert.That(arguments[3], Is.EqualTo(calculation.EngineData.NrOfFailedEngines));
            }
        }

        [Test]
        public void Validate_WithArgumentAndNoErrorOccurs_ReturnsNoErrorMessages()
        {
            // Setup
            var random = new Random(21);
            int nrOfFailedEngines = random.Next();

            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);
            SetSimulationSettingsData(calculation.SimulationSettings);

            calculation.EngineData.NrOfFailedEngines = nrOfFailedEngines;

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(Arg.Any<KernelAircraftData>(),
                            Arg.Any<double>(),
                            Arg.Any<double>(),
                            Arg.Any<int>())
                  .ReturnsForAnyArgs(KernelValidationError.None);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                IEnumerable<string> messages = module.Validate(calculation);

                // Assert
                CollectionAssert.IsEmpty(messages);
            }
        }

        [Test]
        [TestCase(KernelValidationError.InvalidDensity, "Density is invalid.")]
        [TestCase(KernelValidationError.InvalidNrOfFailedEngines, "Number of failed engines is invalid.")]
        [TestCase(KernelValidationError.InvalidGravitationalAcceleration, "Gravitational acceleration is invalid.")]
        public void Validate_WithArgumentAndKernelValidationErrorOccurs_ReturnsErrorMessages(KernelValidationError error,
                                                                                             string expectedMessage)
        {
            // Setup
            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);
            SetSimulationSettingsData(calculation.SimulationSettings);

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(Arg.Any<KernelAircraftData>(),
                            Arg.Any<double>(),
                            Arg.Any<double>(),
                            Arg.Any<int>())
                  .ReturnsForAnyArgs(error);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                IEnumerable<string> messages = module.Validate(calculation);

                // Assert
                CollectionAssert.AreEqual(new[]
                {
                    expectedMessage
                }, messages);
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_InvalidEndFailureSpeed_ReturnsValidationMessage(int invalidEndFailureSpeed)
        {
            // Setup
            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);
            SetSimulationSettingsData(calculation.SimulationSettings);

            calculation.SimulationSettings.EndFailureVelocity = invalidEndFailureSpeed;

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(Arg.Any<KernelAircraftData>(),
                            Arg.Any<double>(),
                            Arg.Any<double>(),
                            Arg.Any<int>())
                  .ReturnsForAnyArgs(KernelValidationError.None);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                IEnumerable<string> messages = module.Validate(calculation);

                // Assert
                CollectionAssert.AreEqual(new[]
                {
                    "End failure velocity must be larger than 0."
                }, messages);
            }
        }

        [Test]
        public void Calculate_CalculationNull_ThrowsArgumentNullException()
        {
            // Setup
            var module = new BalancedFieldLengthCalculationModule();

            // Call
            TestDelegate call = () => module.Calculate(null);

            // Assert
            Assert.That(call, Throws.TypeOf<ArgumentNullException>()
                                    .With.Property(nameof(ArgumentNullException.ParamName))
                                    .EqualTo("calculation"));
        }

        [Test]
        public void Calculate_WithValidArgumentsAndResult_ReturnsExpectedOutput()
        {
            // Setup
            var random = new Random(21);
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);
            SetSimulationSettingsData(calculation.SimulationSettings);

            calculation.SimulationSettings.Density = density;
            calculation.SimulationSettings.GravitationalAcceleration = gravitationalAcceleration;
            calculation.SimulationSettings.EndFailureVelocity = 3;

            calculation.EngineData.NrOfFailedEngines = nrOfFailedEngines;

            const double expectedVelocity = 11;
            const double expectedDistance = 20;
            var outputs = new[]
            {
                new AggregatedDistanceOutput(10, 10, 30),
                new AggregatedDistanceOutput(expectedVelocity, expectedDistance, 20),
                new AggregatedDistanceOutput(12, 30, 10)
            };

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Calculate(Arg.Any<KernelAircraftData>(),
                             Arg.Any<EulerIntegrator>(),
                             Arg.Any<int>(),
                             Arg.Any<double>(),
                             Arg.Any<double>(),
                             Arg.Any<CalculationSettings>())
                  .Returns(outputs[0], outputs[1], outputs[2]);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                BalancedFieldLengthOutput output = module.Calculate(calculation);

                // Assert
                Assert.That(output.Velocity, Is.EqualTo(expectedVelocity));
                Assert.That(output.Distance, Is.EqualTo(expectedDistance));

                ICall[] calls = kernel.ReceivedCalls().ToArray();
                Assert.That(calls.Length, Is.EqualTo(3));
                AssertCalculateCall(calls[0], calculation, 0);
                AssertCalculateCall(calls[1], calculation, 1);
                AssertCalculateCall(calls[2], calculation, 2);
            }
        }

        [Test]
        public void Calculate_CalculatorThrowsCalculateException_ThrowsKernelCalculateException()
        {
            // Setup
            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);
            SetSimulationSettingsData(calculation.SimulationSettings);

            calculation.SimulationSettings.EndFailureVelocity = 1;

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            var calculatorException = new CalculatorException("Can't calculate this.");
            kernel.Calculate(Arg.Any<KernelAircraftData>(),
                             Arg.Any<EulerIntegrator>(),
                             Arg.Any<int>(),
                             Arg.Any<double>(),
                             Arg.Any<double>(),
                             Arg.Any<CalculationSettings>())
                  .ThrowsForAnyArgs(calculatorException);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                var module = new BalancedFieldLengthCalculationModule();

                // Call
                TestDelegate call = () => module.Calculate(calculation);

                // Assert
                var exception = Assert.Throws<KernelCalculationException>(call);
                Assert.That(exception.InnerException, Is.SameAs(calculatorException));
                Assert.That(exception.Message, Is.EqualTo(calculatorException.Message));
            }
        }

        private static void AssertCalculateCall(ICall call,
                                                BalancedFieldLengthCalculation calculation,
                                                int failureVelocity)
        {
            object[] arguments = call.GetArguments();
            AircraftDataTestHelper.AssertAircraftData(calculation.AircraftData,
                                                      calculation.EngineData,
                                                      (KernelAircraftData) arguments[0]);

            Assert.That(arguments[1], Is.TypeOf<EulerIntegrator>());

            GeneralSimulationSettingsData simulationSettings = calculation.SimulationSettings;
            Assert.That(arguments[2], Is.EqualTo(calculation.EngineData.NrOfFailedEngines));
            Assert.That(arguments[3], Is.EqualTo(simulationSettings.Density));
            Assert.That(arguments[4], Is.EqualTo(simulationSettings.GravitationalAcceleration));

            CalculationSettingsTestHelper.AssertCalculationSettings(simulationSettings, failureVelocity,
                                                                    (CalculationSettings) arguments[5]);
        }

        private static void SetValidAircraftData(AircraftData data)
        {
            var random = new Random(21);
            data.WingSurfaceArea = random.NextDouble();
            data.AspectRatio = random.NextDouble();
            data.OswaldFactor = random.NextDouble();
            data.MaximumLiftCoefficient = random.NextDouble();
            data.LiftCoefficientGradient = random.NextDouble();
            data.ZeroLiftAngleOfAttack = random.NextAngle();
            data.RestDragCoefficient = random.NextDouble();
            data.RestDragCoefficientWithEngineFailure = random.NextDouble();
            data.MaximumPitchAngle = random.NextAngle();
            data.PitchGradient = random.NextAngle();
            data.RollResistanceCoefficient = random.NextDouble();
            data.RollResistanceWithBrakesCoefficient = random.NextDouble();
            data.TakeOffWeight = random.NextDouble();
        }

        private static void SetEngineData(EngineData data)
        {
            var random = new Random(21);
            data.NrOfEngines = random.Next();
            data.ThrustPerEngine = random.NextDouble();
            data.NrOfFailedEngines = data.NrOfEngines - 1;
        }

        private static void SetSimulationSettingsData(GeneralSimulationSettingsData data)
        {
            var random = new Random(21);
            data.TimeStep = random.NextDouble();
            data.MaximumNrOfIterations = random.Next();
            data.Density = random.NextDouble();
            data.GravitationalAcceleration = random.NextDouble();
            data.EndFailureVelocity = random.Next();
        }
    }
}