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