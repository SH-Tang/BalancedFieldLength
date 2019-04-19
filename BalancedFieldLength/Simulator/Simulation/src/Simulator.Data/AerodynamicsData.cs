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
            aspectRatio.ArgumentIsLargerThanZero(nameof(aspectRatio));
            aspectRatio.ArgumentIsConcreteNumber(nameof(aspectRatio));

            wingArea.ArgumentIsLargerThanZero(nameof(wingArea));
            wingArea.ArgumentIsConcreteNumber(nameof(wingArea));

            liftCoefficientGradient.ArgumentIsLargerThanZero(nameof(liftCoefficientGradient));
            liftCoefficientGradient.ArgumentIsConcreteNumber(nameof(liftCoefficientGradient));

            zeroLiftAngleOfAttack.ArgumentIsConcreteNumber(nameof(zeroLiftAngleOfAttack));

            maximumLiftCoefficient.ArgumentIsLargerThanZero(nameof(maximumLiftCoefficient));
            maximumLiftCoefficient.ArgumentIsConcreteNumber(nameof(maximumLiftCoefficient));

            restDragCoefficientWithoutEngineFailure.ArgumentIsLargerOrEqualToZero(nameof(restDragCoefficientWithoutEngineFailure));
            restDragCoefficientWithoutEngineFailure.ArgumentIsConcreteNumber(nameof(restDragCoefficientWithoutEngineFailure));

            restDragCoefficientWithEngineFailure.ArgumentIsLargerOrEqualToZero(nameof(restDragCoefficientWithEngineFailure));
            restDragCoefficientWithEngineFailure.ArgumentIsConcreteNumber(nameof(restDragCoefficientWithEngineFailure));

            oswaldFactor.ArgumentIsLargerThanZero(nameof(oswaldFactor));
            oswaldFactor.ArgumentIsConcreteNumber(nameof(oswaldFactor));
        }
    }
}