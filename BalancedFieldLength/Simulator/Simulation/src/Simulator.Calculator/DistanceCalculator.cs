using System;
using Simulator.Calculator.Integrators;
using Simulator.Calculator.TakeOffDynamics;
using Simulator.Data;
using Simulator.Data.Exceptions;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class for calculating the traversed distance until the simulation reached the stopping criteria.
    /// </summary>
    public class DistanceCalculator
    {
        private readonly INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator;
        private readonly IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator;
        private readonly IIntegrator integrator;
        private readonly DistanceCalculatorSettings calculatorSettings;

        private const double screenHeight = 10.7;

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculator"/>.
        /// </summary>
        /// <param name="normalTakeOffDynamicsCalculator">The <see cref="INormalTakeOffDynamicsCalculator"/>
        /// to calculate the aircraft dynamics without failure.</param>
        /// <param name="failureTakeOffDynamicsCalculator">The <see cref="IFailureTakeOffDynamicsCalculator"/>
        /// to calculate the aircraft dynamics after failure.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to integrate the first order
        /// dynamic system.</param>
        /// <param name="calculatorSettings">The <see cref="DistanceCalculatorSettings"/>
        /// to configure the calculator.</param>
        /// <exception cref="ArgumentNullException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="normalTakeOffDynamicsCalculator"/></item>
        /// <item><paramref name="failureTakeOffDynamicsCalculator"/></item>
        /// <item><paramref name="integrator"/></item>
        /// <item><paramref name="calculatorSettings"/></item>
        /// </list>
        /// is <c>null</c>.</exception>
        public DistanceCalculator(INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator,
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator,
            IIntegrator integrator, DistanceCalculatorSettings calculatorSettings)
        {
            if (normalTakeOffDynamicsCalculator == null)
            {
                throw new ArgumentNullException(nameof(normalTakeOffDynamicsCalculator));
            }

            if (failureTakeOffDynamicsCalculator == null)
            {
                throw new ArgumentNullException(nameof(failureTakeOffDynamicsCalculator));
            }

            if (integrator == null)
            {
                throw new ArgumentNullException(nameof(integrator));
            }

            if (calculatorSettings == null)
            {
                throw new ArgumentNullException(nameof(calculatorSettings));
            }

            this.normalTakeOffDynamicsCalculator = normalTakeOffDynamicsCalculator;
            this.failureTakeOffDynamicsCalculator = failureTakeOffDynamicsCalculator;
            this.integrator = integrator;
            this.calculatorSettings = calculatorSettings;
        }

        /// <summary>
        /// Calculates the distance which is needed before the aircraft reaches either the screen height
        /// or comes to a standstill.
        /// </summary>
        /// <returns>The <see cref="DistanceCalculatorOutput"/> with the calculated result.</returns>
        /// <exception cref="CalculatorException">Thrown when the calculator cannot calculate the output.</exception>
        public DistanceCalculatorOutput Calculate()
        {
            AircraftState state = new AircraftState();
            bool hasFailureOccurred = false;
            int failureSpeed = calculatorSettings.FailureSpeed;

            for (int i = 0; i < calculatorSettings.NrOfTimeSteps; i++)
            {
                AircraftAccelerations accelerations = hasFailureOccurred
                    ? failureTakeOffDynamicsCalculator.Calculate(state)
                    : normalTakeOffDynamicsCalculator.Calculate(state);

                state = integrator.Integrate(state, accelerations, calculatorSettings.TimeStep);

                if (state.TrueAirspeed > failureSpeed)
                {
                    hasFailureOccurred = true;
                }

                if (state.Height >= screenHeight || state.TrueAirspeed <= 0)
                {
                    return new DistanceCalculatorOutput(failureSpeed, state.Distance, !hasFailureOccurred, true);
                }
            }

            return new DistanceCalculatorOutput(failureSpeed, double.NaN, hasFailureOccurred, false);
        }
    }
}