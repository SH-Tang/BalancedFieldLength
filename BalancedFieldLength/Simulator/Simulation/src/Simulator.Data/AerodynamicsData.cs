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
        public AerodynamicsData(double aspectRatio, double wingArea,
                                Angle zeroLiftAngleOfAttack, double liftCoefficientGradient, double maximumLiftCoefficient,
                                double restDragCoefficientWithoutEngineFailure, double restDragCoefficientWithEngineFailure, double oswaldFactor)
        {
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
    }
}