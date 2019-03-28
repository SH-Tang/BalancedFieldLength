using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;
using Simulator.Integration.TakeOffDynamics;

namespace Simulator.Integration.Factories
{
    /// <summary>
    /// Factory to create take off dynamics calculators.
    /// </summary>
    public class TakeOffDynamicsFactory : ITakeOffDynamicsCalculatorFactory
    {
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