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
using Application.BalancedFieldLength.KernelWrapper.TestUtils;
using Core.Common.TestUtil;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using Simulator.Kernel;
using KernelAircraftData = Simulator.Data.AircraftData;

namespace Application.BalancedFieldLength.KernelWrapper.Test
{
    [TestFixture]
    public class BalancedFieldLengthCalculatorTest
    {
        [Test]
        public void Validate_CalculationNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => BalancedFieldLengthCalculationModule.Validate(null);

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
                // Call
                IEnumerable<string> messages = BalancedFieldLengthCalculationModule.Validate(calculation);

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

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(null,
                            double.NaN,
                            double.NaN,
                            0).ReturnsForAnyArgs(KernelValidationError.None);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                // Call
                BalancedFieldLengthCalculationModule.Validate(calculation);

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
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);

            calculation.SimulationSettings.Density = density;
            calculation.EngineData.NrOfFailedEngines = nrOfFailedEngines;
            calculation.SimulationSettings.GravitationalAcceleration = gravitationalAcceleration;

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(Arg.Any<KernelAircraftData>(),
                            Arg.Any<double>(),
                            Arg.Any<double>(),
                            Arg.Any<int>()).ReturnsForAnyArgs(KernelValidationError.None);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                // Call
                IEnumerable<string> messages = BalancedFieldLengthCalculationModule.Validate(calculation);

                // Assert
                CollectionAssert.IsEmpty(messages);
            }
        }

        [Test]
        [TestCase(KernelValidationError.InvalidDensity, "Density is invalid.")]
        [TestCase(KernelValidationError.InvalidNrOfFailedEngines, "Number of failed engines is invalid.")]
        [TestCase(KernelValidationError.InvalidGravitationalAcceleration, "Gravitational acceleration is invalid.")]
        public void Validate_WithArgumentAndErrorOccurs_ReturnsErrorMessages(KernelValidationError error,
                                                                             string expectedMessage)
        {
            // Setup
            var random = new Random(21);
            int nrOfFailedEngines = random.Next();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            var calculation = new BalancedFieldLengthCalculation();
            SetValidAircraftData(calculation.AircraftData);
            SetEngineData(calculation.EngineData);

            calculation.SimulationSettings.Density = density;
            calculation.EngineData.NrOfFailedEngines = nrOfFailedEngines;
            calculation.SimulationSettings.GravitationalAcceleration = gravitationalAcceleration;

            var kernel = Substitute.For<IAggregatedDistanceCalculatorKernel>();
            kernel.Validate(Arg.Any<KernelAircraftData>(),
                            Arg.Any<double>(),
                            Arg.Any<double>(),
                            Arg.Any<int>()).ReturnsForAnyArgs(error);

            var testFactory = new TestKernelFactory(kernel);
            using (new BalancedFieldLengthKernelFactoryConfig(testFactory))
            {
                // Call
                IEnumerable<string> messages = BalancedFieldLengthCalculationModule.Validate(calculation);

                // Assert
                CollectionAssert.AreEqual(new[]
                {
                    expectedMessage
                }, messages);
            }
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
        }

        /// <summary>
        /// Mockup class for creating instances of <see cref="IAggregatedDistanceCalculatorKernel"/>
        /// which can be used for testing in combination with the <see cref="BalancedFieldLengthKernelFactoryConfig"/>.
        /// </summary>
        private class TestKernelFactory : IBalancedFieldLengthKernelFactory
        {
            private readonly IAggregatedDistanceCalculatorKernel testKernel;

            /// <summary>
            /// Creates a new instance of <see cref="TestKernelFactory"/>.
            /// </summary>
            /// <param name="testKernel">The <see cref="IAggregatedDistanceCalculatorKernel"/>
            /// to run the factory with.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="testKernel"/>
            /// is <c>null</c>.</exception>
            public TestKernelFactory(IAggregatedDistanceCalculatorKernel testKernel)
            {
                if (testKernel == null)
                {
                    throw new ArgumentNullException(nameof(testKernel));
                }

                this.testKernel = testKernel;
            }

            public IAggregatedDistanceCalculatorKernel CreateDistanceCalculatorKernel()
            {
                return testKernel;
            }
        }
    }
}