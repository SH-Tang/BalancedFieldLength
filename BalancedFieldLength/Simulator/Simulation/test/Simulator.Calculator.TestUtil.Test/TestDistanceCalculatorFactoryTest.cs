using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Data;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.TestUtil.Test
{
    [TestFixture]
    public class TestDistanceCalculatorFactoryTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var factory = new TestDistanceCalculatorFactory();

            // Assert
            Assert.IsInstanceOf<IDistanceCalculatorFactory>(factory);
            CollectionAssert.IsEmpty(factory.CreatedContinuedTakeOffDistanceCalculatorInputs);
            CollectionAssert.IsEmpty(factory.CreatedAbortedTakeOffDistanceCalculatorInputs);
        }

        [Test]
        public static void CreateContinuedTakeOffDistanceCalculator_Always_ReturnsSetCalculator()
        {
            // Setup
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            int nrOfFailedEngines = random.Next();

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var takeOffDynamicsCalculatorFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();
            var continuedTakeOffCalculator = Substitute.For<IDistanceCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            var factory = new TestDistanceCalculatorFactory
                          {
                              ContinuedTakeOffDistanceCalculator = continuedTakeOffCalculator
                          };

            // Call 
            IDistanceCalculator createdCalculator = factory.CreateContinuedTakeOffDistanceCalculator(takeOffDynamicsCalculatorFactory,
                                                                                                     aircraftData,
                                                                                                     integrator,
                                                                                                     nrOfFailedEngines,
                                                                                                     density,
                                                                                                     gravitationalAcceleration,
                                                                                                     CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            Assert.AreSame(continuedTakeOffCalculator, createdCalculator);
        }

        [Test]
        public static void GivenFactory_WhenCreateContinuedTakeOffDistanceCalculatorCalled_ThenReturnsInputArguments()
        {
            // Given
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            int nrOfFailedEngines = random.Next();

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var takeOffDynamicsCalculatorFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();
            var integrator = Substitute.For<IIntegrator>();

            var factory = new TestDistanceCalculatorFactory();

            // When 
            factory.CreateContinuedTakeOffDistanceCalculator(takeOffDynamicsCalculatorFactory,
                                                             aircraftData,
                                                             integrator,
                                                             nrOfFailedEngines,
                                                             density,
                                                             gravitationalAcceleration,
                                                             calculationSettings);

            // Then
            IEnumerable<CreatedContinuedTakeOffDistanceCalculatorInput> createContinuedTakeOffDistanceCalculatorInputs = factory.CreatedContinuedTakeOffDistanceCalculatorInputs;
            Assert.AreEqual(1, createContinuedTakeOffDistanceCalculatorInputs.Count());

            CreatedContinuedTakeOffDistanceCalculatorInput input = createContinuedTakeOffDistanceCalculatorInputs.Single();
            Assert.AreSame(takeOffDynamicsCalculatorFactory, input.TakeOffDynamicsCalculatorFactory);
            Assert.AreSame(aircraftData, input.AircraftData);
            Assert.AreSame(integrator, input.Integrator);
            Assert.AreEqual(nrOfFailedEngines, input.NrOfFailedEngines);
            Assert.AreEqual(density, input.Density);
            Assert.AreEqual(gravitationalAcceleration, input.GravitationalAcceleration);
            Assert.AreEqual(calculationSettings, input.CalculationSettings);
        }

        [Test]
        public static void CreateAbortedTakeOffDistanceCalculator_Always_ReturnsSetCalculator()
        {
            // Setup
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var takeOffDynamicsCalculatorFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();
            var abortedTakeOffCalculator = Substitute.For<IDistanceCalculator>();
            var integrator = Substitute.For<IIntegrator>();

            var factory = new TestDistanceCalculatorFactory
                          {
                              AbortedTakeOffDistanceCalculator = abortedTakeOffCalculator
                          };

            // Call 
            IDistanceCalculator createdCalculator = factory.CreateAbortedTakeOffDistanceCalculator(takeOffDynamicsCalculatorFactory,
                                                                                                   aircraftData,
                                                                                                   integrator,
                                                                                                   density,
                                                                                                   gravitationalAcceleration,
                                                                                                   CalculationSettingsTestFactory.CreateDistanceCalculatorSettings());

            // Assert
            Assert.AreSame(abortedTakeOffCalculator, createdCalculator);
        }

        [Test]
        public static void GivenFactory_WhenCreateAbortedTakeOffDistanceCalculatorCalled_ThenReturnsInputArguments()
        {
            // Given
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            CalculationSettings calculationSettings = CalculationSettingsTestFactory.CreateDistanceCalculatorSettings();

            var takeOffDynamicsCalculatorFactory = Substitute.For<ITakeOffDynamicsCalculatorFactory>();
            var integrator = Substitute.For<IIntegrator>();

            var factory = new TestDistanceCalculatorFactory();

            // When 
            factory.CreateAbortedTakeOffDistanceCalculator(takeOffDynamicsCalculatorFactory,
                                                           aircraftData,
                                                           integrator,
                                                           density,
                                                           gravitationalAcceleration,
                                                           calculationSettings);

            // Then
            IEnumerable<CreatedAbortedTakeOffDistanceCalculatorInput> createAbortedTakeOffDistanceCalculatorInputs = factory.CreatedAbortedTakeOffDistanceCalculatorInputs;
            Assert.AreEqual(1, createAbortedTakeOffDistanceCalculatorInputs.Count());

            CreatedAbortedTakeOffDistanceCalculatorInput input = createAbortedTakeOffDistanceCalculatorInputs.Single();
            Assert.AreSame(takeOffDynamicsCalculatorFactory, input.TakeOffDynamicsCalculatorFactory);
            Assert.AreSame(aircraftData, input.AircraftData);
            Assert.AreSame(integrator, input.Integrator);
            Assert.AreEqual(density, input.Density);
            Assert.AreEqual(gravitationalAcceleration, input.GravitationalAcceleration);
            Assert.AreEqual(calculationSettings, input.CalculationSettings);
        }
    }
}