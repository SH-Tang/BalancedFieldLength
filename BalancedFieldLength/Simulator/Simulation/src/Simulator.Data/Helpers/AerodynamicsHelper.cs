using System;

namespace Simulator.Data.Helpers
{
    /// <summary>
    /// Helper class which can be used to calculate the aerodynamic properties.
    /// </summary>
    public static class AerodynamicsHelper
    {
        /// <summary>
        /// Calculates the stall speed based on the input.
        /// </summary>
        /// <param name="aerodynamicData">The <see cref="AerodynamicData"/> containing
        /// all the aerodynamic properties.</param>
        /// <param name="takeOffWeight">The take off weight of the aircraft. [N]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <returns>The stall speed. [m/s]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateStallSpeed(AerodynamicData aerodynamicData, double takeOffWeight, double density)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            return Math.Sqrt((2 * takeOffWeight) / (density * aerodynamicData.WingArea * aerodynamicData.MaximumLiftCoefficient));
        }

        /// <summary>
        /// Calculates the lift coefficient based on the input.
        /// </summary>
        /// <param name="aerodynamicData">The <see cref="AerodynamicData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="angleOfAttack">The angle of attack. [deg]</param>
        /// <returns>The lift coefficient.</returns>
        /// <exception cref="ArgumentNullException">Thrown when
        /// <paramref name="aerodynamicData"/> is <c>null</c>.</exception>
        public static double CalculateLiftCoefficient(AerodynamicData aerodynamicData, double angleOfAttack)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            return aerodynamicData.LiftCoefficientGradient *
                   DegreesToRadians(angleOfAttack - aerodynamicData.ZeroLiftAngleOfAttack);
        }

        /// <summary>
        /// Calculates the lift based on the input.
        /// </summary>
        /// <param name="aerodynamicData">The <see cref="AerodynamicData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="angleOfAttack">The angle of attack. [deg]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateLift(AerodynamicData aerodynamicData,
                                           double angleOfAttack,
                                           double density,
                                           double velocity)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            double liftCoefficient = CalculateLiftCoefficient(aerodynamicData, angleOfAttack);
            return liftCoefficient * CalculateDynamicPressure(velocity, aerodynamicData.WingArea, density);
        }

        /// <summary>
        /// Calculates the drag based on the input (with engine failure).
        /// </summary>
        /// <param name="aerodynamicData">The <see cref="AerodynamicData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="liftCoefficient">The lift current coefficient. [-]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateDragWithEngineFailure(AerodynamicData aerodynamicData,
                                                            double liftCoefficient,
                                                            double density,
                                                            double velocity)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            return CalculateDrag(aerodynamicData, liftCoefficient, density, velocity, true);
        }

        /// <summary>
        /// Calculates the drag based on the input (without engine failure).
        /// </summary>
        /// <param name="aerodynamicData">The <see cref="AerodynamicData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="liftCoefficient">The lift current coefficient. [-]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateDragWithoutEngineFailure(AerodynamicData aerodynamicData, double liftCoefficient, double density, double velocity)
        {
            if (aerodynamicData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicData));
            }

            return CalculateDrag(aerodynamicData, liftCoefficient, density, velocity, false);
        }

        private static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private static double CalculateDynamicPressure(double velocity, double wingArea, double density)
        {
            return 0.5 * density * Math.Pow(velocity, 2) * wingArea;
        }

        private static double CalculateDrag(AerodynamicData aerodynamicData, double liftCoefficient, double density, double velocity, bool hasEngineFailed)
        {
            double staticDragCoefficient = hasEngineFailed
                                               ? aerodynamicData.RestDragCoefficientWithEngineFailure
                                               : aerodynamicData.RestDragCoefficientWithoutEngineFailure;
            double inducedDragCoefficient = Math.Pow(liftCoefficient, 2) / (aerodynamicData.AspectRatio * aerodynamicData.OswaldFactor * Math.PI);
            double dragCoefficient = staticDragCoefficient + inducedDragCoefficient;
            return dragCoefficient * CalculateDynamicPressure(velocity, aerodynamicData.WingArea, density);
        }
    }
}