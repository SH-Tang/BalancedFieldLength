using System;
using NUnit.Framework;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Components.Factories;
using Simulator.Components.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.TestUtil;

namespace Simulator.Components.Test.Factories
{
    [TestFixture]
    public class TakeOffDynamicsCalculatorFactoryTest
    {
        [Test]
        public void CreateNormalTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = new TakeOffDynamicsCalculatorFactory();

            // Call
            INormalTakeOffDynamicsCalculator calculator = factory.CreateNormalTakeOffDynamics(aircraftData,
                                                                                              random.NextDouble(),
                                                                                              random.NextDouble());

            // Assert
            Assert.IsInstanceOf<NormalTakeOffDynamicsCalculator>(calculator);
        }

        [Test]
        public void CreateAbortedTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = new TakeOffDynamicsCalculatorFactory();

            // Call
            IFailureTakeOffDynamicsCalculator calculator = factory.CreateAbortedTakeOffDynamics(aircraftData,
                                                                                                random.NextDouble(),
                                                                                                random.NextDouble());

            // Assert
            Assert.IsInstanceOf<AbortedTakeOffDynamicsCalculator>(calculator);
        }

        [Test]
        public void CreateContinuedTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = new TakeOffDynamicsCalculatorFactory();

            // Call
            IFailureTakeOffDynamicsCalculator calculator = factory.CreateContinuedTakeOffDynamicsCalculator(aircraftData,
                                                                                                            random.Next(),
                                                                                                            random.NextDouble(),
                                                                                                            random.NextDouble());

            // Assert
            Assert.IsInstanceOf<ContinuedTakeOffDynamicsCalculator>(calculator);
        }
    }
}