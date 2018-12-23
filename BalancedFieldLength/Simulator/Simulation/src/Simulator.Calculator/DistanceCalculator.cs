using System;
using Simulator.Calculator.Dynamics;
using Simulator.Calculator.Integrators;
using Simulator.Data;

namespace Simulator.Calculator
{
    /// <summary>
    /// Class for calculating the traversed distance until the simulation reached the stopping criteria.
    /// </summary>
    public class DistanceCalculator
    {
        private readonly INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculatorCalculator;
        private readonly IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculatorCalculator;
        private readonly IIntegrator integrator;
        private readonly int failureSpeed;
        private readonly int nrOfTimeSteps;
        private readonly double timeStep;

        private const double screenHeight = 10.7;

        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculator"/>.
        /// </summary>
        /// <param name="normalTakeOffDynamicsCalculatorCalculator">The <see cref="INormalTakeOffDynamicsCalculator"/>
        /// to calculate the aircraft dynamics without failure.</param>
        /// <param name="failureTakeOffDynamicsCalculatorCalculator">The <see cref="IFailureTakeOffDynamicsCalculator"/>
        /// to calculate the aircraft dynamics after failure.</param>
        /// <param name="integrator">The <see cref="IIntegrator"/> to integrate the first order
        /// dynamic system.</param>
        /// <param name="failureSpeed">The velocity (true airspeed) at which the failure
        /// should occur.</param>
        /// <param name="nrOfTimeSteps">The number of time steps that is allowed for the integration
        /// before timing out.</param>
        /// <param name="timeStep">The time step to take during integration.</param>
        /// <exception cref="ArgumentNullException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="normalTakeOffDynamicsCalculatorCalculator"/></item>
        /// <item><paramref name="failureTakeOffDynamicsCalculatorCalculator"/></item>
        /// <item><paramref name="integrator"/></item>
        /// </list>
        /// is <c>null</c>.</exception>
        public DistanceCalculator(INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculatorCalculator,
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculatorCalculator,
            IIntegrator integrator,
            int failureSpeed, int nrOfTimeSteps, double timeStep)
        {
            this.normalTakeOffDynamicsCalculatorCalculator = normalTakeOffDynamicsCalculatorCalculator;
            this.failureTakeOffDynamicsCalculatorCalculator = failureTakeOffDynamicsCalculatorCalculator;
            this.integrator = integrator;
            this.failureSpeed = failureSpeed;
            this.nrOfTimeSteps = nrOfTimeSteps;
            this.timeStep = timeStep;
        }

        /// <summary>
        /// Calculates the distance which is needed before the aircraft reaches either the screen height
        /// or comes to a standstill.
        /// </summary>
        /// <returns>The <see cref="DistanceCalculatorOutput"/> with the calculated result.</returns>
        public DistanceCalculatorOutput Calculate()
        {
            AircraftState state = new AircraftState();
            bool hasFailureOccurred = false;
            for (int i = 0; i < nrOfTimeSteps; i++)
            {
                AircraftAccelerations accelerations = hasFailureOccurred
                    ? failureTakeOffDynamicsCalculatorCalculator.Calculate(state)
                    : normalTakeOffDynamicsCalculatorCalculator.Calculate(state);

                state = integrator.Integrate(state, accelerations, timeStep);

                if (state.TrueAirspeed > failureSpeed)
                {
                    hasFailureOccurred = true;
                }

                if (state.Height >= screenHeight || state.TrueAirspeed <= 0)
                {
                    return new DistanceCalculatorOutput(failureSpeed, 0, !hasFailureOccurred);
                }
            }

            throw new NotImplementedException("Custom exception after failing to converge not implemented yet.");
        }
    }
}