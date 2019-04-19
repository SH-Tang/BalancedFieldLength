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
using System.Linq;
using Core.Common.Data;
using Core.Common.TestUtil;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.Exceptions;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test
{
    [TestFixture]
    public class DistanceCalculatorTest
    {
        [Test]
        public void Constructor_NormalTakeOffDynamicsCalculatorNull_ThrowsArgumentNullException()
        {
            // Setup
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            // Call
            TestDelegate call = () => new DistanceCalculator(null,
                                                             failureTakeOffDynamicsCalculator,
                                                             integrator,
                                                             CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("normalTakeOffDynamicsCalculator", exception.ParamName);
        }

        [Test]
        public void Constructor_FailureTakeOffDynamicsCalculatorNull_ThrowsArgumentNullException()
        {
            // Setup
            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            // Call
            TestDelegate call = () => new DistanceCalculator(normalTakeOffDynamicsCalculator,
                                                             null,
                                                             integrator,
                                                             CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("failureTakeOffDynamicsCalculator", exception.ParamName);
        }

        [Test]
        public void Constructor_IntegratorNull_ThrowsArgumentNullException()
        {
            // Setup
            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();

            // Call
            TestDelegate call = () => new DistanceCalculator(normalTakeOffDynamicsCalculator,
                                                             failureTakeOffDynamicsCalculator,
                                                             null,
                                                             CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("integrator", exception.ParamName);
        }

        [Test]
        public void Constructor_CalculationSettingsNull_ThrowsArgumentNullException()
        {
            // Setup
            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            // Call
            TestDelegate call = () => new DistanceCalculator(normalTakeOffDynamicsCalculator,
                                                             failureTakeOffDynamicsCalculator,
                                                             integrator,
                                                             null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("calculationSettings", exception.ParamName);
        }

        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Setup
            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            var integrator = Substitute.For<IIntegrator>();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            // Call
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator,
                                                    failureTakeOffDynamicsCalculator,
                                                    integrator,
                                                    calculationSettings);

            // Assert
            Assert.IsInstanceOf<IDistanceCalculator>(calculator);
        }

        [Test]
        public void Calculate_NormalDynamicsCalculatorThrowsException_ThenExceptionRethrown()
        {
            // Setup
            var calculatorException = new CalculatorException();
            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            normalTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                           .Throws(calculatorException);

            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator,
                                                    failureTakeOffDynamicsCalculator,
                                                    integrator,
                                                    CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Call
            TestDelegate call = () => calculator.Calculate();

            // Assert
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreSame(calculatorException, exception);
        }

        [Test]
        public void Calculate_FailureDynamicsCalculatorThrowsException_ThenExceptionRethrown()
        {
            // Setup
            var random = new Random(21);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();

            var calculatorException = new CalculatorException();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            failureTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                            .Throws(calculatorException);

            var integrator = Substitute.For<IIntegrator>();
            integrator.Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep)
                      .Returns(CreateAircraftStateWithVelocity(failureSpeed + 0.1));

            var calculatorSettings = new CalculationSettings(failureSpeed,
                                                             random.Next(),
                                                             timeStep);
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator,
                                                    failureTakeOffDynamicsCalculator,
                                                    integrator,
                                                    calculatorSettings);

            // Call
            TestDelegate call = () => calculator.Calculate();

            // Assert
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreSame(calculatorException, exception);
        }

        [Test]
        public void Calculate_MaximumIterationsHit_ThrowsCalculatorException()
        {
            // Setup
            var random = new Random(21);
            int nrOfTimeSteps = random.Next(1, 10);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();

            var calculatorException = new CalculatorException();
            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            failureTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                            .Throws(calculatorException);

            var integrator = Substitute.For<IIntegrator>();
            integrator.Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep)
                      .Returns(CreateAircraftStateWithVelocity(failureSpeed));

            var calculatorSettings = new CalculationSettings(failureSpeed, nrOfTimeSteps, timeStep);
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator,
                                                    integrator, calculatorSettings);

            // Call
            TestDelegate call = () => calculator.Calculate();

            // Assert
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreEqual("Calculation did not converge.", exception.Message);

            integrator.ReceivedWithAnyArgs(nrOfTimeSteps)
                      .Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithHeightAboveScreenHeightDuringNormalTakeOff_ThenCallsInExpectedOrderAndThrowsCalculatorException()
        {
            // Given
            var random = new Random(21);
            const double screenHeight = 10.7;

            int nrOfTimeSteps = random.Next(2, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            AircraftState[] states =
            {
                CreateAircraftStateWithHeight(screenHeight - 0.1),
                CreateAircraftStateWithHeight(screenHeight + 0.1),
                CreateAircraftStateWithHeight(screenHeight + 0.2)
            };

            AircraftAccelerations[] accelerations =
            {
                CreateAircraftAccelerations(),
                CreateAircraftAccelerations()
            };

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            normalTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                           .Returns(accelerations[0], accelerations[1]);

            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();

            var integrator = Substitute.For<IIntegrator>();
            integrator.Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep)
                      .Returns(states[0], states[1], states[2]);

            var calculatorSettings = new CalculationSettings(failureSpeed, nrOfTimeSteps, timeStep);
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator,
                                                    integrator, calculatorSettings);

            // When
            TestDelegate call = () => calculator.Calculate();

            // Then
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreEqual("Calculation converged before failure occurred.", exception.Message);

            failureTakeOffDynamicsCalculator.DidNotReceiveWithAnyArgs().Calculate(Arg.Any<AircraftState>());
            normalTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(2).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
                             {
                                 normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)));
                                 integrator.Integrate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)), accelerations[0], timeStep);
                                 normalTakeOffDynamicsCalculator.Calculate(states[0]);
                                 integrator.Integrate(states[0], accelerations[1], timeStep);

                                 // Do not expect additional calls after the second state was returned
                             });
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithVelocityZeroDuringNormalTakeOff_ThenCallsInExpectedOrderAnThrowsCalculatorException()
        {
            // Given
            var random = new Random(21);
            int nrOfTimeSteps = random.Next(2, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            AircraftState[] states =
            {
                CreateAircraftStateWithVelocity(failureSpeed - 0.2),
                CreateAircraftStateWithVelocity(0),
                CreateAircraftStateWithVelocity(failureSpeed - 0.1)
            };

            AircraftAccelerations[] accelerations =
            {
                CreateAircraftAccelerations(),
                CreateAircraftAccelerations()
            };

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            normalTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                           .Returns(accelerations[0], accelerations[1]);

            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();

            var integrator = Substitute.For<IIntegrator>();
            integrator.Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep)
                      .Returns(states[0], states[1], states[2]);

            var calculatorSettings = new CalculationSettings(failureSpeed, nrOfTimeSteps, timeStep);
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator,
                                                    integrator, calculatorSettings);

            // When
            TestDelegate call = () => calculator.Calculate();

            // Then
            var exception = Assert.Throws<CalculatorException>(call);
            Assert.AreEqual("Calculation converged before failure occurred.", exception.Message);

            failureTakeOffDynamicsCalculator.DidNotReceiveWithAnyArgs().Calculate(Arg.Any<AircraftState>());
            normalTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(2).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
                             {
                                 normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)));
                                 integrator.Integrate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)), accelerations[0], timeStep);
                                 normalTakeOffDynamicsCalculator.Calculate(states[0]);
                                 integrator.Integrate(states[0], accelerations[1], timeStep);

                                 // Do not expect additional calls after the second state was returned
                             });
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithVelocityZeroAfterFailureDynamics_ThenCallsInExpectedOrderAndOutputReturned()
        {
            // Given
            var random = new Random(21);
            int nrOfTimeSteps = random.Next(3, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            AircraftState[] states =
            {
                CreateAircraftStateWithVelocity(failureSpeed + 0.1),
                CreateAircraftStateWithVelocity(failureSpeed - 0.2),
                CreateAircraftStateWithVelocity(0)
            };

            AircraftAccelerations[] accelerations =
            {
                CreateAircraftAccelerations(),
                CreateAircraftAccelerations(),
                CreateAircraftAccelerations()
            };

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            normalTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                           .Returns(accelerations[0]);

            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            failureTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                            .Returns(accelerations[1], accelerations[2]);

            var integrator = Substitute.For<IIntegrator>();
            integrator.Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep)
                      .Returns(states[0], states[1], states[2]);

            var calculatorSettings = new CalculationSettings(failureSpeed, nrOfTimeSteps, timeStep);
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator,
                                                    integrator, calculatorSettings);

            // Call
            DistanceCalculatorOutput output = calculator.Calculate();

            // Assert
            normalTakeOffDynamicsCalculator.Received(1).Calculate(Arg.Any<AircraftState>());
            failureTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(3).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
                             {
                                 normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)));
                                 integrator.Integrate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)), accelerations[0], timeStep);

                                 failureTakeOffDynamicsCalculator.Calculate(states[0]);
                                 integrator.Integrate(states[0], accelerations[1], timeStep);

                                 failureTakeOffDynamicsCalculator.Calculate(states[1]);
                                 integrator.Integrate(states[1], accelerations[2], timeStep);
                             });

            Assert.AreEqual(states.Last().Distance, output.Distance);
            Assert.AreEqual(failureSpeed, output.FailureSpeed);
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithScreenHeightAfterFailureDynamics_ThenCallsInExpectedOrderAndOutputReturned()
        {
            // Given
            var random = new Random(21);
            const double screenHeight = 10.7;
            int nrOfTimeSteps = random.Next(3, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var failureState = new AircraftState(random.NextAngle(),
                                                 random.NextAngle(),
                                                 failureSpeed + 0.1,
                                                 random.NextDouble(),
                                                 random.NextDouble());

            AircraftState[] states =
            {
                failureState,
                CreateAircraftStateWithHeight(screenHeight - 0.2),
                CreateAircraftStateWithHeight(screenHeight)
            };

            AircraftAccelerations[] accelerations =
            {
                CreateAircraftAccelerations(),
                CreateAircraftAccelerations(),
                CreateAircraftAccelerations()
            };

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            normalTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                           .Returns(accelerations[0]);

            var failureTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            failureTakeOffDynamicsCalculator.Calculate(Arg.Any<AircraftState>())
                                            .Returns(accelerations[1], accelerations[2]);

            var integrator = Substitute.For<IIntegrator>();
            integrator.Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep)
                      .Returns(states[0], states[1], states[2]);

            var calculatorSettings = new CalculationSettings(failureSpeed, nrOfTimeSteps, timeStep);
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator,
                                                    integrator, calculatorSettings);

            // Call
            DistanceCalculatorOutput output = calculator.Calculate();

            // Assert
            normalTakeOffDynamicsCalculator.Received(1).Calculate(Arg.Any<AircraftState>());
            failureTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(3).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
                             {
                                 normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)));
                                 integrator.Integrate(Arg.Is<AircraftState>(state => IsZeroAircraftState(state)), accelerations[0], timeStep);

                                 failureTakeOffDynamicsCalculator.Calculate(states[0]);
                                 integrator.Integrate(states[0], accelerations[1], timeStep);

                                 failureTakeOffDynamicsCalculator.Calculate(states[1]);
                                 integrator.Integrate(states[1], accelerations[2], timeStep);
                             });

            Assert.AreEqual(states.Last().Distance, output.Distance);
            Assert.AreEqual(failureSpeed, output.FailureSpeed);
        }

        private static bool IsZeroAircraftState(AircraftState state)
        {
            return state.FlightPathAngle.Equals(new Angle())
                   && state.PitchAngle.Equals(new Angle())
                   && state.Height == 0
                   && state.TrueAirspeed == 0;
        }

        private static AircraftState CreateAircraftStateWithVelocity(double velocity)
        {
            var random = new Random(21);
            return new AircraftState(random.NextAngle(),
                                     random.NextAngle(),
                                     velocity,
                                     random.NextDouble(),
                                     random.NextDouble());
        }

        private static AircraftState CreateAircraftStateWithHeight(double height)
        {
            var random = new Random(21);
            return new AircraftState(random.NextAngle(),
                                     random.NextAngle(),
                                     random.NextDouble(),
                                     height,
                                     random.NextDouble());
        }

        private static AircraftAccelerations CreateAircraftAccelerations()
        {
            var random = new Random(21);
            return new AircraftAccelerations(random.NextAngle(),
                                             random.NextDouble(),
                                             random.NextDouble(),
                                             random.NextAngle());
        }
    }
}