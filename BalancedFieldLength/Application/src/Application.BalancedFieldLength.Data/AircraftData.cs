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

using Core.Common.Data;

namespace Application.BalancedFieldLength.Data
{
    /// <summary>
    /// Class to hold aircraft data.
    /// </summary>
    public class AircraftData
    {
        /// <summary>
        /// Creates a new instance of <see cref="AircraftData"/>.
        /// </summary>
        public AircraftData()
        {
            TakeOffWeight = double.NaN;

            PitchGradient = Angle.FromDegrees(double.NaN);
            MaximumPitchAngle = Angle.FromDegrees(double.NaN);

            WingSurfaceArea = double.NaN;
            AspectRatio = double.NaN;
            OswaldFactor = double.NaN;
            MaximumLiftCoefficient = double.NaN;
            LiftCoefficientGradient = double.NaN;
            ZeroLiftAngleOfAttack = Angle.FromDegrees(double.NaN);

            RestDragCoefficient = double.NaN;
            RestDragCoefficientWithEngineFailure = double.NaN;
            RollResistanceCoefficient = double.NaN;
            RollResistanceWithBrakesCoefficient = double.NaN;
        }

        /// <summary>
        /// Gets or sets the take off weight. [kN]
        /// </summary>
        public double TakeOffWeight { get; set; }

        /// <summary>
        /// Gets or sets the pitch gradient. [-]
        /// </summary>
        /// <remarks>The linear gradient in which the pitch angle
        /// increases from 0 angle to the maximum pitch angle as a function
        /// of time.</remarks>
        public Angle PitchGradient { get; set; }

        /// <summary>
        /// Gets or sets the maximum pitch angle. [-]
        /// </summary>
        /// <remarks>Also denoted as ThetaMax</remarks>
        public Angle MaximumPitchAngle { get; set; }

        /// <summary>
        /// Gets or sets the wing surface area. [m2]
        /// </summary>
        /// <remarks>Also denoted as S.</remarks>
        public double WingSurfaceArea { get; set; }

        /// <summary>
        /// Gets or sets the aspect ratio. [-]
        /// </summary>
        /// <remarks>Also denoted as A.</remarks>
        public double AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the Oswald factor. [-]
        /// </summary>
        /// <remarks>Also denoted as e.</remarks>
        public double OswaldFactor { get; set; }

        /// <summary>
        /// Gets or sets the maximum lift coefficient. [-]
        /// </summary>
        /// <remarks>Also denoted as CLMax.</remarks>
        public double MaximumLiftCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the lift coefficient gradient. [-]
        /// </summary>
        /// <remarks>This represents the linear slope between the zero lift angle of attack
        /// and the maximum lift coefficient and stands for the increase of lift coefficient
        /// per angle of attack.</remarks>
        public double LiftCoefficientGradient { get; set; }

        /// <summary>
        /// Gets or sets the angle of attack where the lift is 0. [-]
        /// </summary>
        public Angle ZeroLiftAngleOfAttack { get; set; }

        /// <summary>
        /// Gets or sets the rest drag coefficient during normal take off conditions. [-]
        /// </summary>
        /// <remarks>Also denoted as CD0.</remarks>
        public double RestDragCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the rest drag coefficient when an engine failure is present. [-]
        /// </summary>
        /// <remarks>Also denoted as CD0.</remarks>
        public double RestDragCoefficientWithEngineFailure { get; set; }

        /// <summary>
        /// Gets or sets the roll resistance coefficient during normal take off conditions. [-]
        /// </summary>
        /// <remarks>Also denoted as MuRoll.</remarks>
        public double RollResistanceCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the roll resistance coefficient when the brakes are active. [-]
        /// </summary>
        /// <remarks>Also denoted as MuBrake.</remarks>
        public double RollResistanceWithBrakesCoefficient { get; set; }
    }
}