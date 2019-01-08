using System;
using NSubstitute;
using NUnit.Framework;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test.Factories
{
    [TestFixture]
    public class DistanceCalculatorFactoryTest
    {
        [Test]
        public void Instance__Always_ReturnsAFactoryInstance()
        {
            // Call
            IDistanceCalculatorFactory factory = DistanceCalculatorFactory.Instance;

            // Assert
            Assert.IsInstanceOf<IDistanceCalculatorFactory>(factory);
        }

        [Test]
        public void CreateContinuedTakeOffDistanceCalculator_TakeOffDynamicsCalculatorFactoryNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var integrator = Substitute.For<IIntegrator>();

            IDistanceCalculatorFactory factory = DistanceCalculatorFactory.Instance;

            // Call
            TestDelegate call = () => factory.CreateContinuedTakeOffDistanceCalculator(null,
                                                                                       aircraftData,
                                                                                       integrator,
                                                                                       random.Next(),
                                                                                       random.NextDouble(),
                                                                                       random.NextDouble(),
                                                                                       CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("takeOffDynamicsCalculatorFactory", exception.ParamName);
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

            IDistanceCalculatorFactory factory = DistanceCalculatorFactory.Instance;

            // Call
            DistanceCalculator calculator = factory.CreateContinuedTakeOffDistanceCalculator(
                takeOffDynamicsCalculatorFactory,
                aircraftData,
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

            Assert.IsNotNull(calculator);
        }

        [Test]
        public void
            CreateAbortedTakeOffDistanceCalculator_TakeOffDynamicsCalculatorFactoryNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var integrator = Substitute.For<IIntegrator>();

            IDistanceCalculatorFactory factory = DistanceCalculatorFactory.Instance;

            // Call
            TestDelegate call = () => factory.CreateAbortedTakeOffDistanceCalculator(null,
                                                                                     aircraftData,
                                                                                     integrator,
                                                                                     random.NextDouble(),
                                                                                     random.NextDouble(),
                                                                                     CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("takeOffDynamicsCalculatorFactory", exception.ParamName);
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

            IDistanceCalculatorFactory factory = DistanceCalculatorFactory.Instance;

            // Call
            DistanceCalculator calculator = factory.CreateAbortedTakeOffDistanceCalculator(
                takeOffDynamicsCalculatorFactory,
                aircraftData,
                integrator,
                density,
                gravitationalAcceleration,
                CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            takeOffDynamicsCalculatorFactory.DidNotReceiveWithAnyArgs().CreateContinuedTakeOffDynamicsCalculator(
                Arg.Any<AircraftData>(),
                Arg.Any<int>(),
                Arg.Any<double>(),
                Arg.Any<double>());
            takeOffDynamicsCalculatorFactory.Received(1).CreateNormalTakeOffDynamics(Arg.Any<AircraftData>(),
                                                                                     Arg.Any<double>(),
                                                                                     Arg.Any<double>());
            takeOffDynamicsCalculatorFactory.Received(1).CreateAbortedTakeOffDynamics(Arg.Any<AircraftData>(),
                                                                                      Arg.Any<double>(),
                                                                                      Arg.Any<double>());

            Assert.IsNotNull(calculator);
        }
    }
}