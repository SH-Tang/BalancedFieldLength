using System;
using Core.Common.Data;

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
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/> containing
        /// all the aerodynamic properties.</param>
        /// <param name="takeOffWeight">The take off weight of the aircraft. [N]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <returns>The stall speed. [m/s]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicsData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateStallSpeed(AerodynamicsData aerodynamicsData, double takeOffWeight, double density)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            return Math.Sqrt((2 * takeOffWeight) / (density * aerodynamicsData.WingArea * aerodynamicsData.MaximumLiftCoefficient));
        }

        /// <summary>
        /// Calculates the lift coefficient based on the input.
        /// </summary>
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="angleOfAttack">The angle of attack.</param>
        /// <returns>The lift coefficient.</returns>
        /// <exception cref="ArgumentNullException">Thrown when
        /// <paramref name="aerodynamicsData"/> is <c>null</c>.</exception>
        public static double CalculateLiftCoefficient(AerodynamicsData aerodynamicsData, Angle angleOfAttack)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            return aerodynamicsData.LiftCoefficientGradient *
                   (angleOfAttack.Radians - aerodynamicsData.ZeroLiftAngleOfAttack.Radians);
        }

        /// <summary>
        /// Calculates the lift based on the input.
        /// </summary>
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="angleOfAttack">The angle of attack. [deg]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicsData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateLift(AerodynamicsData aerodynamicsData,
                                           Angle angleOfAttack,
                                           double density,
                                           double velocity)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            double liftCoefficient = CalculateLiftCoefficient(aerodynamicsData, angleOfAttack);
            return liftCoefficient * CalculateDynamicPressure(velocity, aerodynamicsData.WingArea, density);
        }

        /// <summary>
        /// Calculates the drag based on the input (with engine failure).
        /// </summary>
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="liftCoefficient">The lift current coefficient. [-]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicsData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateDragWithEngineFailure(AerodynamicsData aerodynamicsData,
                                                            double liftCoefficient,
                                                            double density,
                                                            double velocity)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            return CalculateDrag(aerodynamicsData, liftCoefficient, density, velocity, true);
        }

        /// <summary>
        /// Calculates the drag based on the input (without engine failure).
        /// </summary>
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/>
        /// containing all the aerodynamic properties.</param>
        /// <param name="liftCoefficient">The lift current coefficient. [-]</param>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <param name="velocity">The true airspeed (Vtas). [m/s]</param>
        /// <returns>The generated lift. [N]</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aerodynamicsData"/>
        /// is <c>null</c>.</exception>
        public static double CalculateDragWithoutEngineFailure(AerodynamicsData aerodynamicsData, double liftCoefficient, double density, double velocity)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            return CalculateDrag(aerodynamicsData, liftCoefficient, density, velocity, false);
        }

        private static double CalculateDynamicPressure(double velocity, double wingArea, double density)
        {
            return 0.5 * density * Math.Pow(velocity, 2) * wingArea;
        }

        private static double CalculateDrag(AerodynamicsData aerodynamicsData, double liftCoefficient, double density, double velocity, bool hasEngineFailed)
        {
            double staticDragCoefficient = hasEngineFailed
                                               ? aerodynamicsData.RestDragCoefficientWithEngineFailure
                                               : aerodynamicsData.RestDragCoefficientWithoutEngineFailure;
            double inducedDragCoefficient = Math.Pow(liftCoefficient, 2) / (aerodynamicsData.AspectRatio * aerodynamicsData.OswaldFactor * Math.PI);
            double dragCoefficient = staticDragCoefficient + inducedDragCoefficient;
            return dragCoefficient * CalculateDynamicPressure(velocity, aerodynamicsData.WingArea, density);
        }
    }
}