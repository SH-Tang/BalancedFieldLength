using System;
using Simulator.Calculator;
using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Integration.Factories
{
    /// <summary>
    /// Factory to create configured instance of <see cref="DistanceCalculator"/>.
    /// </summary>
    public class DistanceCalculatorFactory : IDistanceCalculatorFactory
    {
        private readonly ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory;

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculator"/>.
        /// </summary>
        /// <param name="takeOffDynamicsCalculatorFactory">The factory to create instances of take off dynamics calculators.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="takeOffDynamicsCalculatorFactory"/> is <c>null</c>.</exception>
        public DistanceCalculatorFactory(ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory)
        {
            if (takeOffDynamicsCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(takeOffDynamicsCalculatorFactory));
            }

            this.takeOffDynamicsCalculatorFactory = takeOffDynamicsCalculatorFactory;
        }

        public IDistanceCalculator CreateContinuedTakeOffDistanceCalculator(AircraftData data,
                                                                            IIntegrator integrator,
                                                                            int nrOfFailedEngines,
                                                                            double density,
                                                                            double gravitationalAcceleration,
                                                                            CalculationSettings calculationSettings)
        {
            if (takeOffDynamicsCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(takeOffDynamicsCalculatorFactory));
            }

            INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data, density, gravitationalAcceleration);
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator = 
                takeOffDynamicsCalculatorFactory.CreateContinuedTakeOffDynamicsCalculator(data, nrOfFailedEngines, density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, calculationSettings);
        }

        public IDistanceCalculator CreateAbortedTakeOffDistanceCalculator(AircraftData data,
                                                                          IIntegrator integrator,
                                                                          double density,
                                                                          double gravitationalAcceleration,
                                                                          CalculationSettings calculationSettings)
        {
            INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator = 
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data, density, gravitationalAcceleration);
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator = 
                takeOffDynamicsCalculatorFactory.CreateAbortedTakeOffDynamics(data, density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, calculationSettings);
        }
    }
}