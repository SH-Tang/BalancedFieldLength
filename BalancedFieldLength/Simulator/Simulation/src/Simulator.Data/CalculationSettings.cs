using System;
using Core.Common.Utils;

namespace Simulator.Data
{
    /// <summary>
    /// Class that holds all the numeric information to configure a calculation.
    /// </summary>
    public class CalculationSettings
    {
        /// <summary>
        /// Creates a new instance of <see cref="CalculationSettings"/>.
        /// </summary>
        /// <param name="failureSpeed">The speed at which a failure occurs. [m/s]</param>
        /// <param name="maximumNrOfTimeSteps">The maximum number of time steps before the
        /// calculator times out.</param>
        /// <param name="timeStep">The amount of seconds each time step represents. [s]</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="timeStep"/>
        /// is <see cref="double.NaN"/> or <see cref="double.PositiveInfinity"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureSpeed"/> &lt; 0</item>item>
        /// <item><paramref name="maximumNrOfTimeSteps"/> &lt;= 0</item>
        /// <item><paramref name="timeStep"/> &lt;= 0</item>
        /// </list></exception>
        public CalculationSettings(int failureSpeed, int maximumNrOfTimeSteps, double timeStep)
        {
            ValidateInput(failureSpeed, maximumNrOfTimeSteps, timeStep);

            FailureSpeed = failureSpeed;
            MaximumNrOfTimeSteps = maximumNrOfTimeSteps;
            TimeStep = timeStep;
        }

        /// <summary>
        /// Gets the failure speed. [m/s]
        /// </summary>
        public int FailureSpeed { get; }

        /// <summary>
        /// Gets the maximum number of time steps before the calculator times out.
        /// </summary>
        public int MaximumNrOfTimeSteps { get; }

        /// <summary>
        /// Gets the amount of seconds a time step represents. [s]
        /// </summary>
        public double TimeStep { get; }

        /// <summary>
        /// Validates the numeric input.
        /// </summary>
        /// <param name="failureSpeed">The speed at which a failure occurs. [m/s]</param>
        /// <param name="maximumNrOfTimeSteps">The maximum number of time steps before the
        /// calculator times out.</param>
        /// <param name="timeStep">The amount of seconds each time step represents. [s]</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="timeStep"/>
        /// is <see cref="double.NaN"/> or <see cref="double.PositiveInfinity"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when:
        /// <list type="bullet">
        /// <paramref name="failureSpeed"/> &lt; 0
        /// <paramref name="maximumNrOfTimeSteps"/> &lt;= 0
        /// <paramref name="timeStep"/> &lt;= 0
        /// </list></exception>
        private static void ValidateInput(int failureSpeed, int maximumNrOfTimeSteps, double timeStep)
        {
            NumberGuard.ValidateParameterLargerOrEqualToZero(failureSpeed, nameof(failureSpeed));
            NumberGuard.ValidateParameterLargerThanZero(maximumNrOfTimeSteps, nameof(maximumNrOfTimeSteps));

            NumberGuard.ValidateParameterLargerThanZero(timeStep, nameof(timeStep));
            NumberGuard.ValidateValueIsConcreteNumber(timeStep, nameof(timeStep));
        }
    }
}