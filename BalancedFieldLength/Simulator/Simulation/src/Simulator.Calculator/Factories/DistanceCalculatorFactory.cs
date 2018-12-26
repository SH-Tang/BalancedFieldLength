using System;
using Simulator.Calculator.Integrators;
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
        /// Gets an instance of <see cref="IDistanceCalculatorFactory"/>.
        /// </summary>
        public static IDistanceCalculatorFactory Instance
        {
            get { return instance ?? (instance = new DistanceCalculatorFactory()); }
        }

        public DistanceCalculator CreateContinuedTakeOffDistanceCalculator(
            ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory, AircraftData data,
            IIntegrator integrator,
            int nrOfFailedEngines, double density, double gravitationalAcceleration, int failureSpeed, int maximumSteps,
            double timeStep)
        {
            if (takeOffDynamicsCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(takeOffDynamicsCalculatorFactory));
            }

            var normalTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data, density, gravitationalAcceleration);
            var failureTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateContinuedTakeOffDynamicsCalculator(data, nrOfFailedEngines,
                    density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, new DistanceCalculatorSettings(failureSpeed, maximumSteps, timeStep));
        }

        public DistanceCalculator CreateAbortedTakeOffDistanceCalculator(
            ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory, AircraftData data,
            IIntegrator integrator,
            double density, double gravitationalAcceleration, int failureSpeed, int maximumSteps, double timeStep)
        {
            if (takeOffDynamicsCalculatorFactory == null)
            {
                throw new ArgumentNullException(nameof(takeOffDynamicsCalculatorFactory));
            }

            var normalTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateNormalTakeOffDynamics(data, density, gravitationalAcceleration);
            var failureTakeOffDynamicsCalculator =
                takeOffDynamicsCalculatorFactory.CreateAbortedTakeOffDynamics(data, density, gravitationalAcceleration);

            return new DistanceCalculator(normalTakeOffDynamicsCalculator, failureTakeOffDynamicsCalculator, integrator, new DistanceCalculatorSettings(failureSpeed, maximumSteps, timeStep));
        }
    }
}