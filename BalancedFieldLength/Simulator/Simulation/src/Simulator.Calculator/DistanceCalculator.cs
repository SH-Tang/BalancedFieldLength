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
        private readonly INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator;
        private readonly IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator;
        private readonly IIntegrator integrator;
        private readonly int failureSpeed;
        private readonly int nrOfTimeSteps;
        private readonly double timeStep;

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
        /// <param name="failureSpeed">The velocity (true airspeed) at which the failure
        /// should occur.</param>
        /// <param name="nrOfTimeSteps">The number of time steps that is allowed for the integration
        /// before timing out.</param>
        /// <param name="timeStep">The time step to take during integration.</param>
        /// <exception cref="ArgumentNullException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="normalTakeOffDynamicsCalculator"/></item>
        /// <item><paramref name="failureTakeOffDynamicsCalculator"/></item>
        /// <item><paramref name="integrator"/></item>
        /// </list>
        /// is <c>null</c>.</exception>
        public DistanceCalculator(INormalTakeOffDynamicsCalculator normalTakeOffDynamicsCalculator,
            IFailureTakeOffDynamicsCalculator failureTakeOffDynamicsCalculator,
            IIntegrator integrator,
            int failureSpeed, int nrOfTimeSteps, double timeStep)
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

            this.normalTakeOffDynamicsCalculator = normalTakeOffDynamicsCalculator;
            this.failureTakeOffDynamicsCalculator = failureTakeOffDynamicsCalculator;
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
                    ? failureTakeOffDynamicsCalculator.Calculate(state)
                    : normalTakeOffDynamicsCalculator.Calculate(state);

                state = integrator.Integrate(state, accelerations, timeStep);

                if (state.TrueAirspeed > failureSpeed)
                {
                    hasFailureOccurred = true;
                }

                if (state.Height >= screenHeight || state.TrueAirspeed <= 0)
                {
                    return new DistanceCalculatorOutput(failureSpeed, state.Distance, !hasFailureOccurred);
                }
            }

            throw new NotImplementedException("Custom exception after failing to converge not implemented yet.");
        }
    }
}