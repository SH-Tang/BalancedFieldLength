using System;
using NUnit.Framework;
using Simulator.Data;
using Simulator.Data.TestUtil;

namespace Simulator.Kernel.Test
{
    [TestFixture]
    public class AggregatedDistanceCalculatorKernelTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var kernel = new AggregatedDistanceCalculatorKernel();

            // Assert
            Assert.IsInstanceOf<IAggregatedDistanceCalculatorKernel>(kernel);
        }

        [Test]
        public void Validate_AircraftDataNull_ThrowsArgumentNullException()
        {
            // Setup
            var random = new Random(21);
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();
            int nrOfFailedEngines = random.Next();

            var kernel = new AggregatedDistanceCalculatorKernel();

            // Call
            TestDelegate call = () => kernel.Validate(null, density, gravitationalAcceleration, nrOfFailedEngines);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(call);
            Assert.AreEqual("aircraftData", exception.ParamName);
        }

        [Test]
        public void Validate_WithValidData_ReturnsExpectedValidationResult()
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            var kernel = new AggregatedDistanceCalculatorKernel();

            // Call
            KernelValidationResult result = kernel.Validate(aircraftData,
                                                            density,
                                                            gravitationalAcceleration,
                                                            aircraftData.NrOfEngines - 1);

            // Assert
            Assert.IsTrue(result.IsValid);
            CollectionAssert.IsEmpty(result.ValidationErrors);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_DensityInvalid_ReturnsExpectedValidationResult(double density)
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            double gravitationalAcceleration = random.NextDouble();

            var kernel = new AggregatedDistanceCalculatorKernel();

            // Call
            KernelValidationResult result = kernel.Validate(aircraftData,
                                                            density,
                                                            gravitationalAcceleration,
                                                            aircraftData.NrOfEngines - 1);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.AreEqual(new[]
                                      {
                                          KernelValidationError.InvalidDensity
                                      }, result.ValidationErrors);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_GravityInvalid_ReturnsExpectedValidationResult(double gravitationalAcceleration)
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            double density = random.NextDouble();

            var kernel = new AggregatedDistanceCalculatorKernel();

            // Call
            KernelValidationResult result = kernel.Validate(aircraftData,
                                                            density,
                                                            gravitationalAcceleration,
                                                            aircraftData.NrOfEngines - 1);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.AreEqual(new[]
                                      {
                                          KernelValidationError.InvalidGravitationalAcceleration
                                      }, result.ValidationErrors);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Validate_NrOfFailedEnginesInvalid_ReturnsExpectedValidationResult(int offset)
        {
            // Setup
            var random = new Random(21);
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();
            double density = random.NextDouble();
            double gravitationalAcceleration = random.NextDouble();

            var kernel = new AggregatedDistanceCalculatorKernel();

            // Call
            KernelValidationResult result = kernel.Validate(aircraftData,
                                                            density,
                                                            gravitationalAcceleration,
                                                            aircraftData.NrOfEngines + offset);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.AreEqual(new[]
                                      {
                                          KernelValidationError.InvalidNrOfFailedEngines
                                      }, result.ValidationErrors);
        }

        [Test]
        public void Validate_AllDataInvalid_ReturnsExpectedValidationResult()
        {
            // Setup
            AircraftData aircraftData = AircraftDataTestFactory.CreateRandomAircraftData();

            var kernel = new AggregatedDistanceCalculatorKernel();

            // Call
            KernelValidationResult result = kernel.Validate(aircraftData, 0, 0,
                                                            aircraftData.NrOfEngines + 1);

            // Assert
            Assert.IsFalse(result.IsValid);
            CollectionAssert.AreEquivalent(new[]
                                           {
                                               KernelValidationError.InvalidDensity,
                                               KernelValidationError.InvalidGravitationalAcceleration,
                                               KernelValidationError.InvalidNrOfFailedEngines
                                           }, result.ValidationErrors);
        }
    }
}