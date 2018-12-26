using System;
using NUnit.Framework;
using Simulator.Calculator.Factories;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data.TestUtil;

namespace Simulator.Calculator.Test.Factories
{
    [TestFixture]
    public class TakeOffDynamicsFactoryTest
    {
        [Test]
        public void Instance__Always_ReturnsAFactoryInstance()
        {
            // Call
            var factory = TakeOffDynamicsFactory.Instance;

            // Assert
            Assert.IsInstanceOf<ITakeOffDynamicsCalculatorFactory>(factory);
        }

        [Test]
        public void CreateNormalTakeOffDynamics_Always_ReturnsExpectedCalculator()
        {
            // Setup
            var random = new Random(21);
            var aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = TakeOffDynamicsFactory.Instance;

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
            var aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = TakeOffDynamicsFactory.Instance;

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
            var aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var factory = TakeOffDynamicsFactory.Instance;

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