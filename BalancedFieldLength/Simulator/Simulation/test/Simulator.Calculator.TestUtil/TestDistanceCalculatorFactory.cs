using System.Collections.Generic;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator.TestUtil
{
    /// <summary>
    /// Distance calculator factory to facilitate testing calls of the <see cref="DistanceCalculatorFactory"/>.
    /// </summary>
    public class TestDistanceCalculatorFactory : IDistanceCalculatorFactory
    {
        private readonly List<CreatedContinuedTakeOffDistanceCalculatorInput> createdContinuedTakeOffDistanceCalculatorInputs;
        private readonly List<CreatedAbortedTakeOffDistanceCalculatorInput> createdAbortedTakeOffDistanceCalculatorInputs;

        /// <summary>
        /// Creates a new instance of <see cref="TestDistanceCalculatorFactory"/>.
        /// </summary>
        public TestDistanceCalculatorFactory()
        {
            createdContinuedTakeOffDistanceCalculatorInputs = new List<CreatedContinuedTakeOffDistanceCalculatorInput>();
            createdAbortedTakeOffDistanceCalculatorInputs = new List<CreatedAbortedTakeOffDistanceCalculatorInput>();
        }

        /// <summary>
        /// Sets the continued take off distance calculator.
        /// </summary>
        public IDistanceCalculator ContinuedTakeOffDistanceCalculator { private get; set; }

        /// <summary>
        /// Sets the aborted take off distance calculator.
        /// </summary>
        public IDistanceCalculator AbortedTakeOffDistanceCalculator { private get; set; }

        /// <summary>
        /// Gets the inputs that were used when calling <see cref="CreateContinuedTakeOffDistanceCalculator"/>.
        /// </summary>
        public IEnumerable<CreatedContinuedTakeOffDistanceCalculatorInput> CreatedContinuedTakeOffDistanceCalculatorInputs
        {
            get
            {
                return createdContinuedTakeOffDistanceCalculatorInputs;
            }
        }

        /// <summary>
        /// Gets the inputs that were used when calling <see cref="CreatedAbortedTakeOffDistanceCalculatorInputs"/>.
        /// </summary>
        public IEnumerable<CreatedAbortedTakeOffDistanceCalculatorInput> CreatedAbortedTakeOffDistanceCalculatorInputs
        {
            get
            {
                return createdAbortedTakeOffDistanceCalculatorInputs;
            }
        }

        public IDistanceCalculator CreateContinuedTakeOffDistanceCalculator(ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory,
                                                                            AircraftData data,
                                                                            IIntegrator integrator,
                                                                            int nrOfFailedEngines,
                                                                            double density,
                                                                            double gravitationalAcceleration,
                                                                            CalculationSettings calculationSettings)
        {
            createdContinuedTakeOffDistanceCalculatorInputs.Add(new CreatedContinuedTakeOffDistanceCalculatorInput(takeOffDynamicsCalculatorFactory,
                                                                                                                   data,
                                                                                                                   integrator,
                                                                                                                   nrOfFailedEngines,
                                                                                                                   density,
                                                                                                                   gravitationalAcceleration,
                                                                                                                   calculationSettings));
            return ContinuedTakeOffDistanceCalculator;
        }

        public IDistanceCalculator CreateAbortedTakeOffDistanceCalculator(ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory,
                                                                          AircraftData data,
                                                                          IIntegrator integrator,
                                                                          double density,
                                                                          double gravitationalAcceleration,
                                                                          CalculationSettings calculationSettings)
        {
            createdAbortedTakeOffDistanceCalculatorInputs.Add(new CreatedAbortedTakeOffDistanceCalculatorInput(takeOffDynamicsCalculatorFactory,
                                                                                                               data,
                                                                                                               integrator,
                                                                                                               density,
                                                                                                               gravitationalAcceleration,
                                                                                                               calculationSettings));
            return AbortedTakeOffDistanceCalculator;
        }
    }
}