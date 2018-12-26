using System;
using Core.Common.TestUtil;
using NUnit.Framework;

namespace Simulator.Data.Test
{
    [TestFixture]
    public class DistanceCalculatorSettingsTest
    {
        [Test]
        [TestCase(-1)]
        public void Constructor_InvalidFailureSpeed_ThrowsArgumentOutOfRangeException(int failureSpeed)
        {
            // Setup
            var random = new Random(21);
            var nrOfTimeSteps = random.Next();
            var timeStep = random.NextDouble();

            // Call
            TestDelegate call = () => new DistanceCalculatorSettings(failureSpeed, nrOfTimeSteps, timeStep);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, "failureSpeed must be larger or equal to 0.");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void Constructor_InvalidMaximumOfTimeSteps_ThrowsArgumentOutOfRangeException(int maximumNrOfTimeSteps)
        {
            // Setup
            var random = new Random(21);
            var failureSpeed = random.Next();
            var timeStep = random.NextDouble();

            // Call
            TestDelegate call = () => new DistanceCalculatorSettings(failureSpeed, maximumNrOfTimeSteps, timeStep);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, "maximumNrOfTimeSteps must be larger than 0.");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(double.NegativeInfinity)]
        public void Constructor_InvalidTimeStep_ThrowsArgumentOutOfRangeException(double timeStep)
        {
            // Setup
            var random = new Random(21);
            var failureSpeed = random.Next();
            var maximumNrOfTimeSteps = random.Next();

            // Call
            TestDelegate call = () => new DistanceCalculatorSettings(failureSpeed, maximumNrOfTimeSteps, timeStep);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentOutOfRangeException>(call, "timeStep must be larger than 0.");
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void Constructor_TimeStepNotConcrete_ThrowsArgumentOutOfRangeException(double timeStep)
        {
            // Setup
            var random = new Random(21);
            var failureSpeed = random.Next();
            var maximumNrOfTimeSteps = random.Next();

            // Call
            TestDelegate call = () => new DistanceCalculatorSettings(failureSpeed, maximumNrOfTimeSteps, timeStep);

            // Assert
            TestHelper.AssertThrowsArgumentException<ArgumentException>(call, "timeStep must be a concrete number and cannot be NaN or Infinity.");
        }

        [Test]
        public void Constructor_WithValidArguments_ExpectedValues()
        {
            // Setup
            var random = new Random(21);
            var failureSpeed = random.Next();
            var maximumNrOfTimeSteps = random.Next();
            var timeStep = random.NextDouble();

            // Call
            var settings = new DistanceCalculatorSettings(failureSpeed, maximumNrOfTimeSteps, timeStep);
            
            // Assert
            Assert.AreEqual(failureSpeed, settings.FailureSpeed);
            Assert.AreEqual(maximumNrOfTimeSteps, settings.MaximumNrOfTimeSteps);
            Assert.AreEqual(timeStep, settings.TimeStep);
        }
    }
}