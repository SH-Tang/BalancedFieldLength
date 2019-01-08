using System;
using Simulator.Calculator.Factories;

namespace Simulator.Calculator.TestUtil
{
    /// <summary>
    /// This class can be used to set a temporary <see cref="Simulator.Calculator.TestUtil.Test.TestDistanceCalculatorFactory"/> 
    /// for <see cref="DistanceCalculatorFactory.Instance"/> while testing. 
    /// Disposing an instance of this class will revert the <see cref="DistanceCalculatorFactory.Instance"/>.
    /// </summary>
    /// <example>
    /// The following is an example for how to use this class:
    /// <code>
    /// using(new DistanceCalculatorFactoryConfig())
    /// {
    ///     var testFactory = (TestDistanceCalculatorFactory) DistanceCalculatorFactory.Instance;
    /// 
    ///     // Perform tests with testFactory
    /// }
    /// </code>
    /// </example>
    public class DistanceCalculatorFactoryConfig : IDisposable
    {
        private readonly IDistanceCalculatorFactory originalInstance;

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculatorFactoryConfig"/>
        /// to set a <see cref="Simulator.Calculator.TestUtil.Test.TestDistanceCalculatorFactory"/> to the
        /// <see cref="DistanceCalculatorFactory.Instance"/>.
        /// </summary>
        public DistanceCalculatorFactoryConfig()
        {
            originalInstance = DistanceCalculatorFactory.Instance;
            DistanceCalculatorFactory.Instance = new TestDistanceCalculatorFactory();
        }

        /// <summary>
        /// Reverts the <see cref="DistanceCalculatorFactory.Instance"/> to the value
        /// it had at time of construction of the <see cref="DistanceCalculatorFactoryConfig"/>.
        /// </summary>
        public void Dispose()
        {
            DistanceCalculatorFactory.Instance = originalInstance;
        }
    }
}