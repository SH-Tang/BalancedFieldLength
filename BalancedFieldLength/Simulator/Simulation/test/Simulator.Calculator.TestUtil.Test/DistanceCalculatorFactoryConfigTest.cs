using System;
using NUnit.Framework;
using Simulator.Calculator.Factories;

namespace Simulator.Calculator.TestUtil.Test
{
    [TestFixture]
    public class DistanceCalculatorFactoryConfigTest
    {
        [Test]
        public static void NewInstance_Always_CanBeDisposed()
        {
            // Call
            var config = new DistanceCalculatorFactoryConfig();

            // Assert
            Assert.IsInstanceOf<IDisposable>(config);
            Assert.DoesNotThrow(() => config.Dispose());
        }

        [Test]
        public static void Constructor_Always_SetsInstanceToTestDistanceCalculatorFactory()
        {
            // Call 
            using (new DistanceCalculatorFactoryConfig())
            {
                // Assert
                Assert.IsInstanceOf<TestDistanceCalculatorFactory>(DistanceCalculatorFactory.Instance);
            }
        }

        [Test]
        public static void Dispose_Always_ResetsInstanceToOriginalInstance()
        {
            // Setup
            IDistanceCalculatorFactory originalInstance = DistanceCalculatorFactory.Instance;
            var config = new DistanceCalculatorFactoryConfig();

            // Precondition
            Assert.AreNotSame(originalInstance, DistanceCalculatorFactory.Instance);

            // Call 
            config.Dispose();

            // Assert
            Assert.AreSame(originalInstance, DistanceCalculatorFactory.Instance);
        }
    }
}