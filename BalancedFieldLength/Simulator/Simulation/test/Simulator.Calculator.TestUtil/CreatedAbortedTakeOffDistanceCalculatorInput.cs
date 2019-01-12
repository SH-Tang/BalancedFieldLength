using Simulator.Calculator.Factories;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator.TestUtil
{
    /// <summary>
    /// Class to hold all the input arguments when creating a continued take off distance calculator.
    /// </summary>
    public class CreatedAbortedTakeOffDistanceCalculatorInput
    {
        /// <summary>
        /// Creates a new instance of <see cref="CreatedContinuedTakeOffDistanceCalculatorInput"/>.
        /// </summary>
        internal CreatedAbortedTakeOffDistanceCalculatorInput(ITakeOffDynamicsCalculatorFactory takeOffDynamicsCalculatorFactory,
                                                             AircraftData aircraftData,
                                                             IIntegrator integrator,
                                                             double density,
                                                             double gravitationalAcceleration,
                                                             CalculationSettings calculationSettings)
        {
            TakeOffDynamicsCalculatorFactory = takeOffDynamicsCalculatorFactory;
            AircraftData = aircraftData;
            Integrator = integrator;
            Density = density;
            GravitationalAcceleration = gravitationalAcceleration;
            CalculationSettings = calculationSettings;
        }

        /// <summary>
        /// Gets the <see cref="ITakeOffDynamicsCalculatorFactory"/>.
        /// </summary>
        public ITakeOffDynamicsCalculatorFactory TakeOffDynamicsCalculatorFactory { get; }

        /// <summary>
        /// Gets the <see cref="AircraftData"/>.
        /// </summary>
        public AircraftData AircraftData { get; }

        /// <summary>
        /// Gets the <see cref="IIntegrator"/>.
        /// </summary>
        public IIntegrator Integrator { get; }

        /// <summary>
        /// Gets the air density. [kg/m^3]
        /// </summary>
        public double Density { get; }

        /// <summary>
        /// Gets the gravitational acceleration. [m/s^2]
        /// </summary>
        public double GravitationalAcceleration { get; }

        /// <summary>
        /// Gets the <see cref="Data.CalculationSettings"/>.
        /// </summary>
        public CalculationSettings CalculationSettings { get; }
    }
}