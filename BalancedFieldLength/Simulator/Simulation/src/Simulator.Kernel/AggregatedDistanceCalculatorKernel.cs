using System;
using System.Collections.Generic;
using System.Linq;
using Simulator.Calculator.AggregatedDistanceCalculator;
using Simulator.Calculator.Integrators;
using Simulator.Data;
using Simulator.Integration.Factories;

namespace Simulator.Kernel
{
    /// <summary>
    /// Calculator kernel which is configured to calculate the aggregated distance output.
    /// </summary>
    public class AggregatedDistanceCalculatorKernel : IAggregatedDistanceCalculatorKernel
    {
        private readonly IAggregatedDistanceCalculator aggregatedDistanceCalculator;

        /// <summary>
        /// Creates a new instance of <see cref="AggregatedDistanceCalculatorKernel"/>.
        /// </summary>
        public AggregatedDistanceCalculatorKernel()
        {
            var distanceCalculatorFactory = new DistanceCalculatorFactory(new TakeOffDynamicsCalculatorFactory());
            aggregatedDistanceCalculator = new AggregatedDistanceCalculator(distanceCalculatorFactory);
        }

        public KernelValidationResult Validate(AircraftData aircraftData,
                                               double density,
                                               double gravitationalAcceleration,
                                               int nrOfFailedEngines)
        {
            if (aircraftData == null)
            {
                throw new ArgumentNullException(nameof(aircraftData));
            }

            var validationErrors = new List<KernelValidationError>();
            if (density <= 0)
            {
                validationErrors.Add(KernelValidationError.InvalidDensity);
            }

            if (gravitationalAcceleration <= 0)
            {
                validationErrors.Add(KernelValidationError.InvalidGravitationalAcceleration);
            }

            if (nrOfFailedEngines >= aircraftData.NrOfEngines)
            {
                validationErrors.Add(KernelValidationError.InvalidNrOfFailedEngines);
            }

            return new KernelValidationResult(!validationErrors.Any(), validationErrors);
        }

        public AggregatedDistanceOutput Calculate(AircraftData aircraftData,
                                                  IIntegrator integrator,
                                                  int nrOfFailedEngines,
                                                  double density,
                                                  double gravitationalAcceleration,
                                                  CalculationSettings calculationSettings)
        {
            return aggregatedDistanceCalculator.Calculate(aircraftData, integrator, nrOfFailedEngines, density, gravitationalAcceleration, calculationSettings);
        }
    }
}