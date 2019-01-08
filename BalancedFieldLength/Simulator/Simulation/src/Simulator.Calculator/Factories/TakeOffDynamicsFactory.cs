using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;

namespace Simulator.Calculator.Factories
{
    /// <summary>
    /// Factory to create take off dynamics calculators.
    /// </summary>
    public class TakeOffDynamicsFactory : ITakeOffDynamicsCalculatorFactory
    {
        private static ITakeOffDynamicsCalculatorFactory instance;

        /// <summary>
        /// Gets an instance of <see cref="ITakeOffDynamicsCalculatorFactory"/>.
        /// </summary>
        public static ITakeOffDynamicsCalculatorFactory Instance
        {
            get
            {
                return instance ?? (instance = new TakeOffDynamicsFactory());
            }
        }

        public INormalTakeOffDynamicsCalculator CreateNormalTakeOffDynamics(AircraftData data,
                                                                            double density,
                                                                            double gravitationalAcceleration)
        {
            return new NormalTakeOffDynamicsCalculator(data, density, gravitationalAcceleration);
        }

        public IFailureTakeOffDynamicsCalculator CreateAbortedTakeOffDynamics(AircraftData data,
                                                                              double density,
                                                                              double gravitationalAcceleration)
        {
            return new AbortedTakeOffDynamicsCalculator(data, density, gravitationalAcceleration);
        }

        public IFailureTakeOffDynamicsCalculator CreateContinuedTakeOffDynamicsCalculator(AircraftData data,
                                                                                          int nrOfFailedEngines,
                                                                                          double density,
                                                                                          double gravitationalAcceleration)
        {
            return new ContinuedTakeOffDynamicsCalculator(data, nrOfFailedEngines, density, gravitationalAcceleration);
        }
    }
}