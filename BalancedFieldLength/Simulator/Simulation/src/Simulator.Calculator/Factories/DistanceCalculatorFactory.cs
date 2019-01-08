using System;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Calculator.Factories
{
    /// <summary>
    /// Factory to create configured instance of <see cref="DistanceCalculator"/>.
    /// </summary>
    public class DistanceCalculatorFactory : IDistanceCalculatorFactory
    {
        private static IDistanceCalculatorFactory instance;

        /// <summary>
        /// Gets or sets an instance of <see cref="IDistanceCalculatorFactory"/>.
        /// </summary>
        public static IDistanceCalculatorFactory Instance
        {
            get
            {
                return instance ?? (instance = new DistanceCalculatorFactory());
            }

            internal set
            {
                instance = value;
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
            if (takeOffDynamicsCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(takeOffDynamicsCalculatorFactory));
            }

            INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data,
                                                                             density,
                                                                             gravitationalAcceleration);
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateContinuedTakeOffDynamicsCalculator(data,
                                                                                          nrOfFailedEngines,
                                                                                          density,
                                                                                          gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, calculationSettings);
        }

        public IDistanceCalculator CreateAbortedTakeOffDistanceCalculator(ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory,
                                                                          AircraftData data,
                                                                          IIntegrator integrator,
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
                takeOffDynamicsCalculatorFactory.CreateAbortedTakeOffDynamics(data, density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, calculationSettings);
        }
    }
}