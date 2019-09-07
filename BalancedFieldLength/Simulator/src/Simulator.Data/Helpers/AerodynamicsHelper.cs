// Copyright (C) 2018 Dennis Tang. All rights reserved.
//
// This file is part of Balanced Field Length.
//
// Balanced Field Length is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using Core.Common.Data;
using Core.Common.Utils;
using Simulator.Data.Properties;
using Simulator.Data.Exceptions;

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
        /// <exception cref="InvalidCalculationException">Thrown when the combination
        /// of input parameters leads to an invalid calculation result.</exception>
        public static double CalculateStallSpeed(AerodynamicsData aerodynamicsData, double takeOffWeight, double density)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            ValidateDensity(density);

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
        /// <exception cref="InvalidCalculationException">Thrown when the combination
        /// of input parameters leads to an invalid calculation result.</exception>
        public static double CalculateLiftCoefficient(AerodynamicsData aerodynamicsData, Angle angleOfAttack)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            if (angleOfAttack < aerodynamicsData.ZeroLiftAngleOfAttack)
            {
                throw new InvalidCalculationException(Resources.AerodynamicsHelper_Angle_of_attack_must_be_larger_than_ZeroLiftAngleOfAttack);
            }

            double liftCoefficient = aerodynamicsData.LiftCoefficientGradient *
                                     (angleOfAttack.Radians - aerodynamicsData.ZeroLiftAngleOfAttack.Radians);
            if (liftCoefficient > aerodynamicsData.MaximumLiftCoefficient)
            {
                throw new InvalidCalculationException(Resources.AerodynamicsHelper_Angle_of_attack_results_in_exceeding_CLMax);
            }

            return liftCoefficient;
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
        /// <exception cref="InvalidCalculationException">Thrown when the combination
        /// of input parameters leads to an invalid calculation result.</exception>
        public static double CalculateLift(AerodynamicsData aerodynamicsData,
                                           Angle angleOfAttack,
                                           double density,
                                           double velocity)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            ValidateDensity(density);
            ValidateVelocity(velocity);

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
        /// <exception cref="InvalidCalculationException">Thrown when the combination
        /// of input parameters leads to an invalid calculation result.</exception>
        public static double CalculateDragWithEngineFailure(AerodynamicsData aerodynamicsData,
                                                            double liftCoefficient,
                                                            double density,
                                                            double velocity)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            ValidateDensity(density);
            ValidateVelocity(velocity);
            ValidateLiftCoefficient(aerodynamicsData, liftCoefficient);

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
        /// <exception cref="InvalidCalculationException">Thrown when the combination
        /// of input parameters leads to an invalid calculation result.</exception>
        public static double CalculateDragWithoutEngineFailure(AerodynamicsData aerodynamicsData, double liftCoefficient, double density, double velocity)
        {
            if (aerodynamicsData == null)
            {
                throw new ArgumentNullException(nameof(aerodynamicsData));
            }

            ValidateDensity(density);
            ValidateVelocity(velocity);
            ValidateLiftCoefficient(aerodynamicsData, liftCoefficient);

            return CalculateDrag(aerodynamicsData, liftCoefficient, density, velocity, false);
        }

        private static double CalculateDynamicPressure(double velocity, double wingArea, double density)
        {
            ValidateVelocity(velocity);

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

        #region Validation Helpers

        /// <summary>
        /// Validates whether the velocity is valid.
        /// </summary>
        /// <param name="velocity">The velocity. [m/s]</param>
        /// <exception cref="InvalidCalculationException">Thrown when the velocity is invalid.</exception>
        private static void ValidateVelocity(double velocity)
        {
            try
            {
                velocity.ArgumentIsLargerOrEqualToZero("Velocity");
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new InvalidCalculationException(Resources.AerodynamicsHelper_Velocity_must_be_larger_or_equal_to_Zero, e);
            }
        }

        /// <summary>
        /// Validates whether the density is valid.
        /// </summary>
        /// <param name="density">The density. [kg/m^3]</param>
        /// <exception cref="InvalidCalculationException">Thrown when the density is invalid.</exception>
        private static void ValidateDensity(double density)
        {
            try
            {
                density.ArgumentIsLargerThanZero("Density");
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new InvalidCalculationException(Resources.AerodynamicsHelper_Density_must_be_larger_than_Zero, e);
            }
        }

        /// <summary>
        /// Validates whether the lift coefficient is valid.
        /// </summary>
        /// <param name="aerodynamicsData">The <see cref="AerodynamicsData"/> to validate against.</param>
        /// <param name="liftCoefficient">The lift coefficient to validate.</param>
        /// <exception cref="InvalidCalculationException">Thrown when the lift coefficient is invalid.</exception>
        private static void ValidateLiftCoefficient(AerodynamicsData aerodynamicsData, double liftCoefficient)
        {
            if (liftCoefficient < 0 || liftCoefficient > aerodynamicsData.MaximumLiftCoefficient)
            {
                throw new InvalidCalculationException(Resources.AerodynamicsHelper_LiftCoefficient_out_of_range);
            }
        }

        #endregion
    }
}