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
using NSubstitute;
using NUnit.Framework;
using Simulator.Calculator;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Components.Factories;
using Simulator.Data;
using Simulator.Data.TestUtil;

namespace Simulator.Components.Test.Factories
{
    [TestFixture]
    public class DistanceCalculatorFactoryTest
    {
        [Test]
        public void Constructor_TakeOffDynamicsCalculatorFactoryNull_ThrowsArgumentNullException()
        {
            // Call
            TestDelegate call = () => new DistanceCalculatorFactory(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("takeOffDynamicsCalculatorFactory", exception.ParamName);
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var dynamicsFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();

            // Call
            var factory = new DistanceCalculatorFactory(dynamicsFactory);

            // Assert
            Assert.IsInstanceOf<IDistanceCalculatorFactory>(factory);
        }

        [Test]
        public void CreateContinuedTakeOffDistanceCalculator_WithValidArguments_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            int nrOfFailedEngines = random.Next();

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();

            var takeOffDynamicsCalculatorFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();
            takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(aircraftData, density, gravitationalAcceleration)
                                            .Returns(normalTakeOffDynamicsCalculator);
            takeOffDynamicsCalculatorFactory.CreateContinuedTakeOffDynamicsCalculator(aircraftData, nrOfFailedEngines, density, gravitationalAcceleration)
                                            .Returns(failureTakeOffDynamicsCalculator);

            var integrator = Substitute.For<IIntegrator>();

            IDistanceCalculatorFactory factory = new DistanceCalculatorFactory(takeOffDynamicsCalculatorFactory);

            // Call
            IDistanceCalculator calculator = factory.CreateContinuedTakeOffDistanceCalculator(aircraftData,
                                                                                              integrator,
                                                                                              nrOfFailedEngines,
                                                                                              density,
                                                                                              gravitationalAcceleration,
                                                                                              CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            takeOffDynamicsCalculatorFactory.DidNotReceiveWithAnyArgs().CreateAbortedTakeOffDynamics(Arg.Any<AircraftData>(),
                                                                                                     Arg.Any<double>(),
                                                                                                     Arg.Any<double>());
            takeOffDynamicsCalculatorFactory.Received(1).CreateNormalTakeOffDynamics(Arg.Any<AircraftData>(),
                                                                                     Arg.Any<double>(),
                                                                                     Arg.Any<double>());
            takeOffDynamicsCalculatorFactory.Received(1).CreateContinuedTakeOffDynamicsCalculator(Arg.Any<AircraftData>(),
                                                                                                  Arg.Any<int>(),
                                                                                                  Arg.Any<double>(),
                                                                                                  Arg.Any<double>());

            Assert.IsInstanceOf<DistanceCalculator>(calculator);
        }

        [Test]
        public void CreateAbortedTakeOffDistanceCalculator_WithValidArguments_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();

            var takeOffDynamicsCalculatorFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();
            takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(aircraftData, density, gravitationalAcceleration)
                                            .Returns(normalTakeOffDynamicsCalculator);
            takeOffDynamicsCalculatorFactory.CreateAbortedTakeOffDynamics(aircraftData, density, gravitationalAcceleration)
                                            .Returns(failureTakeOffDynamicsCalculator);

            var integrator = Substitute.For<IIntegrator>();

            IDistanceCalculatorFactory factory = new DistanceCalculatorFactory(takeOffDynamicsCalculatorFactory);

            // Call
            IDistanceCalculator calculator = factory.CreateAbortedTakeOffDistanceCalculator(aircraftData,
                                                                                            integrator,
                                                                                            density,
                                                                                            gravitationalAcceleration,
                                                                                            CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            takeOffDynamicsCalculatorFactory.DidNotReceiveWithAnyArgs().CreateContinuedTakeOffDynamicsCalculator(Arg.Any<AircraftData>(),
                                                                                                                 Arg.Any<int>(),
                                                                                                                 Arg.Any<double>(),
                                                                                                                 Arg.Any<double>());
            takeOffDynamicsCalculatorFactory.Received(1).CreateNormalTakeOffDynamics(Arg.Any<AircraftData>(),
                                                                                     Arg.Any<double>(),
                                                                                     Arg.Any<double>());
            takeOffDynamicsCalculatorFactory.Received(1).CreateAbortedTakeOffDynamics(Arg.Any<AircraftData>(),
                                                                                      Arg.Any<double>(),
                                                                                      Arg.Any<double>());

            Assert.IsInstanceOf<DistanceCalculator>(calculator);
        }
    }
}