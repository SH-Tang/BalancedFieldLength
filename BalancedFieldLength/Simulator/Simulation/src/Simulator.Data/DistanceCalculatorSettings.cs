using System;
using Core.Common.Utils;

namespace Simulator.Data
{
    /// <summary>
    /// Class that holds all the numeric information to configure a
    /// calculator for the distance.
    /// </summary>
    public class DistanceCalculatorSettings
    {
        /// <summary>
        /// Creates a new instance of <see cref="DistanceCalculatorSettings"/>.
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
        public DistanceCalculatorSettings(int failureSpeed, int maximumNrOfTimeSteps, double timeStep)
        {
            ValidateInput(failureSpeed, maximumNrOfTimeSteps, timeStep);

            FailureSpeed = failureSpeed;
            MaximumNrOfTimeSteps = maximumNrOfTimeSteps;
            TimeStep = timeStep;
        }

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
            NumberValidator.ValidateParameterLargerOrEqualToZero(failureSpeed, nameof(failureSpeed));
            NumberValidator.ValidateParameterLargerThanZero(maximumNrOfTimeSteps, nameof(maximumNrOfTimeSteps));

            NumberValidator.ValidateParameterLargerThanZero(timeStep, nameof(timeStep));
            NumberValidator.ValidateValueIsConcreteNumber(timeStep, nameof(timeStep));
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
    }
}