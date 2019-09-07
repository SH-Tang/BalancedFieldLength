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

using Application.BalancedFieldLength.Properties;
using WPF.Components.TabControl;
using WPF.Core;

namespace Application.BalancedFieldLength.Views.TabViews
{
    /// <summary>
    /// ViewModel to display aircraft data in a tab.
    /// </summary>
    public class AircraftDataTabViewModel : ViewModelBase, ITabViewModel
    {
        private double takeOffWeight;
        private double pitchGradient;
        private double maximumPitchAngle;
        private double wingSurfaceArea;
        private double aspectRatio;
        private double oswaldFactor;
        private double maximumLiftCoefficient;
        private double liftCoefficientGradient;
        private double zeroLiftAngleOfAttack;
        private double restDragCoefficient;
        private double restDragCoefficientWithEngineFailure;
        private double rollResistanceCoefficient;
        private double rollResistanceWithBrakesCoefficient;

        /// <summary>
        /// Creates a new instance of <see cref="AircraftDataTabViewModel"/>.
        /// </summary>
        public AircraftDataTabViewModel()
        {
            TakeOffWeight = double.NaN;
            PitchGradient = double.NaN;
            MaximumPitchAngle = double.NaN;

            WingSurfaceArea = double.NaN;
            AspectRatio = double.NaN;
            OswaldFactor = double.NaN;

            MaximumLiftCoefficient = double.NaN;
            LiftCoefficientGradient = double.NaN;
            ZeroLiftAngleOfAttack = double.NaN;

            RestDragCoefficient = double.NaN;
            RestDragCoefficientWithEngineFailure = double.NaN;

            RollResistanceCoefficient = double.NaN;
            RollResistanceWithBrakesCoefficient = double.NaN;
        }

        /// <summary>
        /// Gets or sets the take off weight. [kN]
        /// </summary>
        public double TakeOffWeight
        {
            get
            {
                return takeOffWeight;
            }
            set
            {
                takeOffWeight = value;
                OnPropertyChanged(nameof(TakeOffWeight));
            }
        }

        /// <summary>
        /// Gets or sets the pitch gradient. [-]
        /// </summary>
        /// <remarks>The linear gradient in which the pitch angle
        /// increases from 0 angle to the maximum pitch angle as a function
        /// of time.</remarks>
        public double PitchGradient
        {
            get
            {
                return pitchGradient;
            }
            set
            {
                pitchGradient = value;
                OnPropertyChanged(nameof(PitchGradient));
            }
        }

        /// <summary>
        /// Gets or sets the maximum pitch angle. [-]
        /// </summary>
        /// <remarks>Also denoted as ThetaMax</remarks>
        public double MaximumPitchAngle
        {
            get
            {
                return maximumPitchAngle;
            }
            set
            {
                maximumPitchAngle = value;
                OnPropertyChanged(nameof(MaximumPitchAngle));
            }
        }

        /// <summary>
        /// Gets or sets the wing surface area. [m2]
        /// </summary>
        /// <remarks>Also denoted as S.</remarks>
        public double WingSurfaceArea
        {
            get
            {
                return wingSurfaceArea;
            }
            set
            {
                wingSurfaceArea = value;
                OnPropertyChanged(nameof(WingSurfaceArea));
            }
        }

        /// <summary>
        /// Gets or sets the aspect ratio. [-]
        /// </summary>
        /// <remarks>Also denoted as A.</remarks>
        public double AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;
                OnPropertyChanged(nameof(AspectRatio));
            }
        }

        /// <summary>
        /// Gets or sets the Oswald factor. [-]
        /// </summary>
        /// <remarks>Also denoted as e.</remarks>
        public double OswaldFactor
        {
            get
            {
                return oswaldFactor;
            }
            set
            {
                oswaldFactor = value;
                OnPropertyChanged(nameof(OswaldFactor));
            }
        }

        /// <summary>
        /// Gets or sets the maximum lift coefficient. [-]
        /// </summary>
        /// <remarks>Also denoted as CLMax.</remarks>
        public double MaximumLiftCoefficient
        {
            get
            {
                return maximumLiftCoefficient;
            }
            set
            {
                maximumLiftCoefficient = value;
                OnPropertyChanged(nameof(MaximumLiftCoefficient));
            }
        }

        /// <summary>
        /// Gets or sets the lift coefficient gradient. [-]
        /// </summary>
        /// <remarks>This represents the linear slope between the zero lift angle of attack
        /// and the maximum lift coefficient and stands for the increase of lift coefficient
        /// per angle of attack.</remarks>
        public double LiftCoefficientGradient
        {
            get
            {
                return liftCoefficientGradient;
            }
            set
            {
                liftCoefficientGradient = value;
                OnPropertyChanged(nameof(LiftCoefficientGradient));
            }
        }

        /// <summary>
        /// Gets or sets the angle of attack where the lift is 0. [-]
        /// </summary>
        public double ZeroLiftAngleOfAttack
        {
            get
            {
                return zeroLiftAngleOfAttack;
            }
            set
            {
                zeroLiftAngleOfAttack = value;
                OnPropertyChanged(nameof(ZeroLiftAngleOfAttack));
            }
        }

        /// <summary>
        /// Gets or sets the rest drag coefficient during normal take off conditions. [-]
        /// </summary>
        /// <remarks>Also denoted as CD0.</remarks>
        public double RestDragCoefficient
        {
            get
            {
                return restDragCoefficient;
            }
            set
            {
                restDragCoefficient = value;
                OnPropertyChanged(nameof(RestDragCoefficient));
            }
        }

        /// <summary>
        /// Gets or sets the rest drag coefficient when an engine failure is present. [-]
        /// </summary>
        /// <remarks>Also denoted as CD0.</remarks>
        public double RestDragCoefficientWithEngineFailure
        {
            get
            {
                return restDragCoefficientWithEngineFailure;
            }
            set
            {
                restDragCoefficientWithEngineFailure = value;
                OnPropertyChanged(nameof(RestDragCoefficientWithEngineFailure));
            }
        }

        /// <summary>
        /// Gets or sets the roll resistance coefficient during normal take off conditions. [-]
        /// </summary>
        /// <remarks>Also denoted as MuRoll.</remarks>
        public double RollResistanceCoefficient
        {
            get
            {
                return rollResistanceCoefficient;
            }
            set
            {
                rollResistanceCoefficient = value;
                OnPropertyChanged(nameof(RollResistanceCoefficient));
            }
        }

        /// <summary>
        /// Gets or sets the roll resistance coefficient when the brakes are active. [-]
        /// </summary>
        /// <remarks>Also denoted as MuBrake.</remarks>
        public double RollResistanceWithBrakesCoefficient
        {
            get
            {
                return rollResistanceWithBrakesCoefficient;
            }
            set
            {
                rollResistanceWithBrakesCoefficient = value;
                OnPropertyChanged(nameof(RollResistanceWithBrakesCoefficient));
            }
        }

        public string TabName
        {
            get
            {
                return Resources.AircraftDataTabViewModel_TabName;
            }
        }
    }
}