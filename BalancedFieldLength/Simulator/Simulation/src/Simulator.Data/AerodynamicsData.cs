using System;
using Core.Common.Data;

namespace Simulator.Data
{
    /// <summary>
    /// Class to hold all relevant aerodynamic data of the aircraft.
    /// </summary>
    public class AerodynamicsData
    {
        /// <summary>
        /// Creates a new instance of <see cref="AerodynamicsData"/>.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio of the aircraft. [-]</param>
        /// <param name="wingArea">The surface area of the lift generating elements of the aircraft. [m2]</param>
        /// <param name="zeroLiftAngleOfAttack">The angle of attack of which the lift coefficient is 0.</param>
        /// <param name="liftCoefficientGradient">The gradient of the lift coefficient as a function of the angle of attack. [1/rad]</param>
        /// <param name="maximumLiftCoefficient">The maximum lift coefficient. [-]</param>
        /// <param name="restDragCoefficientWithoutEngineFailure">The rest drag coefficient of the aircraft without engine failure. [-]</param>
        /// <param name="restDragCoefficientWithEngineFailure">The rest drag coefficient of the aircraft with engine failure. [-]</param>
        /// <param name="oswaldFactor">The Oswald factor. [-]</param>
        /// <exception cref="ArgumentException">Thrown when any parameter equals to <see cref="double.NaN"/>
        /// or <see cref="double.PositiveInfinity"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="aspectRatio"/> &lt;= 0</item>
        /// <item><paramref name="wingArea"/> &lt;= 0</item>
        /// <item><paramref name="liftCoefficientGradient"/> &lt;= 0</item>
        /// <item><paramref name="maximumLiftCoefficient"/> &lt;= 0</item>
        /// <item><paramref name="restDragCoefficientWithoutEngineFailure"/> &lt; 0</item>
        /// <item><paramref name="restDragCoefficientWithEngineFailure"/> &lt; 0</item>
        /// <item><paramref name="oswaldFactor"/> &lt;= 0</item>
        /// </list>
        /// </exception>
        public AerodynamicsData(double aspectRatio, double wingArea,
            Angle zeroLiftAngleOfAttack, double liftCoefficientGradient, double maximumLiftCoefficient,
            double restDragCoefficientWithoutEngineFailure, double restDragCoefficientWithEngineFailure,
            double oswaldFactor)
        {
            ValidateInput(aspectRatio, wingArea,
                zeroLiftAngleOfAttack, liftCoefficientGradient, maximumLiftCoefficient,
                restDragCoefficientWithoutEngineFailure, restDragCoefficientWithEngineFailure, oswaldFactor);

            AspectRatio = aspectRatio;
            WingArea = wingArea;
            ZeroLiftAngleOfAttack = zeroLiftAngleOfAttack;
            LiftCoefficientGradient = liftCoefficientGradient;
            MaximumLiftCoefficient = maximumLiftCoefficient;
            RestDragCoefficientWithoutEngineFailure = restDragCoefficientWithoutEngineFailure;
            RestDragCoefficientWithEngineFailure = restDragCoefficientWithEngineFailure;
            OswaldFactor = oswaldFactor;
        }

        /// <summary>
        /// Gets the aspect ratio of the aircraft. [-].
        /// </summary>
        /// <remarks>Also denoted as A.</remarks>
        public double AspectRatio { get; }

        /// <summary>
        /// Gets the wing area of the aircraft. [m2]
        /// </summary>
        /// <remarks>Also denoted as S.</remarks>
        public double WingArea { get; }

        /// <summary>
        /// Gets the angle of attack for which the lift is 0.
        /// </summary>
        /// <remarks>Also denoted as alpha_0.</remarks>
        public Angle ZeroLiftAngleOfAttack { get; }

        /// <summary>
        /// Gets the lift coefficient gradient as a function of the angle of attack. [1/rad]
        /// </summary>
        /// <remarks>Also denoted as Cl_alpha</remarks>
        public double LiftCoefficientGradient { get; }

        /// <summary>
        /// Gets the maximum lift coefficient. [-]
        /// </summary>
        /// <remarks>Also denoted as Cl_Max.</remarks>
        public double MaximumLiftCoefficient { get; }

        /// <summary>
        /// Gets the rest drag (static drag) coefficient of the aircraft without an engine failure. [-]
        /// </summary>
        /// <remarks>Also denoted as Cd_0.</remarks>
        public double RestDragCoefficientWithoutEngineFailure { get; }

        /// <summary>
        /// Gets the rest drag (static drag) coefficient of the aircraft with an engine failure. [-]
        /// </summary>
        /// <remarks>Also denoted as Cd_0.</remarks>
        public double RestDragCoefficientWithEngineFailure { get; }

        /// <summary>
        /// Gets the Oswald factor. [-]
        /// </summary>
        /// <remarks>Also denoted as e.</remarks>
        public double OswaldFactor { get; }

        /// <summary>
        /// Validates the input values.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio of the aircraft. [-]</param>
        /// <param name="wingArea">The surface area of the lift generating elements of the aircraft. [m2]</param>
        /// <param name="zeroLiftAngleOfAttack">The angle of attack of which the lift coefficient is 0.</param>
        /// <param name="liftCoefficientGradient">The gradient of the lift coefficient as a function of the angle of attack. [1/rad]</param>
        /// <param name="maximumLiftCoefficient">The maximum lift coefficient. [-]</param>
        /// <param name="restDragCoefficientWithoutEngineFailure">The rest drag coefficient of the aircraft without engine failure. [-]</param>
        /// <param name="restDragCoefficientWithEngineFailure">The rest drag coefficient of the aircraft with engine failure. [-]</param>
        /// <param name="oswaldFactor">The Oswald factor. [-]</param>
        /// <exception cref="ArgumentException">Thrown when any parameter equals to <see cref="double.NaN"/>
        /// or <see cref="double.PositiveInfinity"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="aspectRatio"/> &lt;= 0</item>
        /// <item><paramref name="wingArea"/> &lt;= 0</item>
        /// <item><paramref name="liftCoefficientGradient"/> &lt;= 0</item>
        /// <item><paramref name="maximumLiftCoefficient"/> &lt;= 0</item>
        /// <item><paramref name="restDragCoefficientWithoutEngineFailure"/> &lt; 0</item>
        /// <item><paramref name="restDragCoefficientWithEngineFailure"/> &lt; 0</item>
        /// <item><paramref name="oswaldFactor"/> &lt;= 0</item>
        /// </list>
        /// </exception>
        private static void ValidateInput(double aspectRatio, double wingArea, Angle zeroLiftAngleOfAttack,
            double liftCoefficientGradient, double maximumLiftCoefficient,
            double restDragCoefficientWithoutEngineFailure,
            double restDragCoefficientWithEngineFailure, double oswaldFactor)
        {
            ValidateParameterLargerThanZero(aspectRatio, nameof(aspectRatio));
            ValidateParameterIsConcreteValue(aspectRatio, nameof(aspectRatio));

            ValidateParameterLargerThanZero(wingArea, nameof(wingArea));
            ValidateParameterIsConcreteValue(wingArea, nameof(wingArea));

            ValidateParameterLargerThanZero(liftCoefficientGradient, nameof(liftCoefficientGradient));
            ValidateParameterIsConcreteValue(liftCoefficientGradient, nameof(liftCoefficientGradient));

            ValidateParameterIsConcreteValue(zeroLiftAngleOfAttack.Radians, nameof(zeroLiftAngleOfAttack));

            ValidateParameterLargerThanZero(maximumLiftCoefficient, nameof(maximumLiftCoefficient));
            ValidateParameterIsConcreteValue(maximumLiftCoefficient, nameof(maximumLiftCoefficient));

            ValidateParameterLargerOrEqualToZero(restDragCoefficientWithoutEngineFailure,
                nameof(restDragCoefficientWithoutEngineFailure));
            ValidateParameterIsConcreteValue(restDragCoefficientWithoutEngineFailure,
                nameof(restDragCoefficientWithoutEngineFailure));

            ValidateParameterLargerOrEqualToZero(restDragCoefficientWithEngineFailure,
                nameof(restDragCoefficientWithEngineFailure));
            ValidateParameterIsConcreteValue(restDragCoefficientWithEngineFailure,
                nameof(restDragCoefficientWithEngineFailure));

            ValidateParameterLargerThanZero(oswaldFactor, nameof(oswaldFactor));
            ValidateParameterIsConcreteValue(oswaldFactor, nameof(oswaldFactor));
        }

        /// <summary>
        /// Validates whether a value is larger than 0.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is less or equal to 0.</exception>
        private static void ValidateParameterLargerThanZero(double value, string propertyName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger than 0.");
            }
        }

        /// <summary>
        /// Validates whether a value is larger or equal to 0.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is less than 0.</exception>
        private static void ValidateParameterLargerOrEqualToZero(double value, string propertyName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} must be larger or equal to 0.");
            }
        }

        /// <summary>
        /// Validates whether a value is not <see cref="double.NaN"/> or Infinity.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/>
        /// is <see cref="double.NaN"/>, <see cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.</exception>
        private static void ValidateParameterIsConcreteValue(double value, string propertyName)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException($"{propertyName} must be a concrete number and cannot be NaN or Infinity.");
            }
        }
    }
}