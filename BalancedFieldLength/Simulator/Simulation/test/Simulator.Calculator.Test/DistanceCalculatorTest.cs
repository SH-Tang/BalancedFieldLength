using System;
using Core.Common.Data;
using Core.Common.TestUtil;
using NSubstitute;
using NUnit.Framework;
using Simulator.Calculator.Dynamics;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator.Test
{
    [TestFixture]
    public class DistanceCalculatorTest
    {
        [Test]
        public void Constructor_WithArguments_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            var nrOfTimeSteps = random.Next();
            var timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var normalTakeOffDynamicsCalculator = Substitute.For<INormalTakeOffDynamicsCalculator>();
            var abortedTakeOffDynamicsCalculator = Substitute.For<IFailureTakeOffDynamicsCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            // Call
            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator, abortedTakeOffDynamicsCalculator,
                integrator, failureSpeed, nrOfTimeSteps, timeStep);

            // Assert
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithHeightAboveScreenHeightDuringNormalTakeOff_ThenCallsInExpectedOrderAndOutputReturned()
        {
            // Given
            var random = new Random(21);
            const double screenHeight = 10.7;

            var nrOfTimeSteps = random.Next(2, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var states = new[]
            {
                CreateAircraftStateWithHeight(screenHeight - 0.1),
                CreateAircraftStateWithHeight(screenHeight + 0.1),
                CreateAircraftStateWithHeight(screenHeight + 0.2)
            };

            var accelerations = new[]
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

            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator,
                failureTakeOffDynamicsCalculator, integrator, failureSpeed,
                nrOfTimeSteps, timeStep);

            // When
            DistanceCalculatorOutput output = calculator.Calculate();

            // Then
            failureTakeOffDynamicsCalculator.DidNotReceiveWithAnyArgs().Calculate(Arg.Any<AircraftState>());
            normalTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(2).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
            {
                normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state =>
                    IsZeroAircraftState(state)));
                integrator.Integrate(Arg.Is<AircraftState>(state =>
                    IsZeroAircraftState(state)), accelerations[0], timeStep);
                normalTakeOffDynamicsCalculator.Calculate(states[0]);
                integrator.Integrate(states[0], accelerations[1], timeStep);

                // Do not expect additional calls after the second state was returned
            });

            Assert.AreEqual(0, output.Distance);
            Assert.AreEqual(failureSpeed, output.FailureSpeed);
            Assert.IsTrue(output.ConvergenceBeforeFailure);
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithVelocityZeroAfterFailureDynamics_ThenCallsInExpectedOrderAndOutputReturned()
        {
            // Given
            var random = new Random(21);
            var nrOfTimeSteps = random.Next(3, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var states = new[]
            {
                CreateAircraftStateWithVelocity(failureSpeed + 0.1),
                CreateAircraftStateWithVelocity(failureSpeed - 0.2),
                CreateAircraftStateWithVelocity(0)
            };

            var accelerations = new[]
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

            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator,
                failureTakeOffDynamicsCalculator, integrator, failureSpeed,
                nrOfTimeSteps, timeStep);

            // Call
            DistanceCalculatorOutput output = calculator.Calculate();

            // Assert
            normalTakeOffDynamicsCalculator.Received(1).Calculate(Arg.Any<AircraftState>());
            failureTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(3).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
            {
                normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state =>
                    IsZeroAircraftState(state)));
                integrator.Integrate(Arg.Is<AircraftState>(state =>
                    IsZeroAircraftState(state)), accelerations[0], timeStep);

                failureTakeOffDynamicsCalculator.Calculate(states[0]);
                integrator.Integrate(states[0], accelerations[1], timeStep);

                failureTakeOffDynamicsCalculator.Calculate(states[1]);
                integrator.Integrate(states[1], accelerations[2], timeStep);
            });

            Assert.AreEqual(0, output.Distance);
            Assert.AreEqual(failureSpeed, output.FailureSpeed);
            Assert.IsFalse(output.ConvergenceBeforeFailure);
        }

        [Test]
        public void GivenCalculator_WhenCalculatingAndSolutionConvergesWithScreenHeightAfterFailureDynamics_ThenCallsInExpectedOrderAndOutputReturned()
        {
            // Given
            var random = new Random(21);
            const double screenHeight = 10.7;
            var nrOfTimeSteps = random.Next(3, int.MaxValue);
            double timeStep = random.NextDouble();
            int failureSpeed = random.Next();

            var failureState = new AircraftState(random.NextAngle(),
                random.NextAngle(),
                failureSpeed + 0.1,
                random.NextDouble(), 
                random.NextDouble());

            var states = new[]
            {
                failureState,
                CreateAircraftStateWithHeight(screenHeight - 0.2),
                CreateAircraftStateWithHeight(screenHeight)
            };

            var accelerations = new[]
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

            var calculator = new DistanceCalculator(normalTakeOffDynamicsCalculator,
                failureTakeOffDynamicsCalculator, integrator, failureSpeed,
                nrOfTimeSteps, timeStep);

            // Call
            DistanceCalculatorOutput output = calculator.Calculate();

            // Assert
            normalTakeOffDynamicsCalculator.Received(1).Calculate(Arg.Any<AircraftState>());
            failureTakeOffDynamicsCalculator.Received(2).Calculate(Arg.Any<AircraftState>());
            integrator.Received(3).Integrate(Arg.Any<AircraftState>(), Arg.Any<AircraftAccelerations>(), timeStep);
            Received.InOrder(() =>
            {
                normalTakeOffDynamicsCalculator.Calculate(Arg.Is<AircraftState>(state =>
                    IsZeroAircraftState(state)));
                integrator.Integrate(Arg.Is<AircraftState>(state =>
                    IsZeroAircraftState(state)), accelerations[0], timeStep);

                failureTakeOffDynamicsCalculator.Calculate(states[0]);
                integrator.Integrate(states[0], accelerations[1], timeStep);

                failureTakeOffDynamicsCalculator.Calculate(states[1]);
                integrator.Integrate(states[1], accelerations[2], timeStep);
            });

            Assert.AreEqual(0, output.Distance);
            Assert.AreEqual(failureSpeed, output.FailureSpeed);
            Assert.IsFalse(output.ConvergenceBeforeFailure);
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