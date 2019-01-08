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
        private readonly List<CreateContinuedTakeOffDistanceCalculatorInput> createContinuedTakeOffDistanceCalculatorInputs;
        private readonly List<CreateAbortedTakeOffDistanceCalculatorInput> createAbortedTakeOffDistanceCalculatorInputs;

        /// <summary>
        /// Creates a new instance of <see cref="TestDistanceCalculatorFactory"/>.
        /// </summary>
        public TestDistanceCalculatorFactory()
        {
            createContinuedTakeOffDistanceCalculatorInputs = new List<CreateContinuedTakeOffDistanceCalculatorInput>();
            createAbortedTakeOffDistanceCalculatorInputs = new List<CreateAbortedTakeOffDistanceCalculatorInput>();
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
        public IEnumerable<CreateContinuedTakeOffDistanceCalculatorInput> CreateContinuedTakeOffDistanceCalculatorInputs
        {
            get
            {
                return createContinuedTakeOffDistanceCalculatorInputs;
            }
        }

        /// <summary>
        /// Gets the inputs that were used when calling <see cref="CreateAbortedTakeOffDistanceCalculatorInputs"/>.
        /// </summary>
        public IEnumerable<CreateAbortedTakeOffDistanceCalculatorInput> CreateAbortedTakeOffDistanceCalculatorInputs
        {
            get
            {
                return createAbortedTakeOffDistanceCalculatorInputs;
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
            createContinuedTakeOffDistanceCalculatorInputs.Add(new CreateContinuedTakeOffDistanceCalculatorInput(takeOffDynamicsCalculatorFactory,
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
            createAbortedTakeOffDistanceCalculatorInputs.Add(new CreateAbortedTakeOffDistanceCalculatorInput(takeOffDynamicsCalculatorFactory,
                                                                                                             data,
                                                                                                             integrator,
                                                                                                             density,
                                                                                                             gravitationalAcceleration,
                                                                                                             calculationSettings));
            return AbortedTakeOffDistanceCalculator;
        }
    }
}