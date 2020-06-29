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

using System.Windows.Controls;
using WPF.Components;

namespace Application.BalancedFieldLength.Views.TabViews
{
    /// <summary>
    /// Interaction logic for AircraftDataTabView.xaml
    /// </summary>
    public partial class AircraftDataTabView : UserControl
    {
        public AircraftDataTabView()
        {
            InitializeComponent();

            TextBoxModificationBinding.Create(TakeOffWeightTextBox);
            TextBoxMoveFocusOnModification.Create(TakeOffWeightTextBox);

            TextBoxModificationBinding.Create(PitchGradientTextBox);
            TextBoxMoveFocusOnModification.Create(PitchGradientTextBox);

            TextBoxModificationBinding.Create(MaximumPitchAngleTextBox);
            TextBoxMoveFocusOnModification.Create(MaximumPitchAngleTextBox);

            TextBoxModificationBinding.Create(WingSurfaceAreaTextBox);
            TextBoxMoveFocusOnModification.Create(WingSurfaceAreaTextBox);

            TextBoxModificationBinding.Create(AspectRatioTextBox);
            TextBoxMoveFocusOnModification.Create(AspectRatioTextBox);

            TextBoxModificationBinding.Create(OswaldFactorTextBox);
            TextBoxMoveFocusOnModification.Create(OswaldFactorTextBox);

            TextBoxModificationBinding.Create(LiftCoefficientGradientTextBox);
            TextBoxMoveFocusOnModification.Create(LiftCoefficientGradientTextBox);

            TextBoxModificationBinding.Create(MaximumLiftCoefficientTextBox);
            TextBoxMoveFocusOnModification.Create(MaximumLiftCoefficientTextBox);

            TextBoxModificationBinding.Create(ZeroLiftAngleTextBox);
            TextBoxMoveFocusOnModification.Create(ZeroLiftAngleTextBox);

            TextBoxModificationBinding.Create(RestDragCoefficientTextBox);
            TextBoxMoveFocusOnModification.Create(RestDragCoefficientTextBox);

            TextBoxModificationBinding.Create(RestDragCoefficientWithEngineFailureTextBox);
            TextBoxMoveFocusOnModification.Create(RestDragCoefficientWithEngineFailureTextBox);

            TextBoxModificationBinding.Create(RollResistanceCoefficientTextBox);
            TextBoxMoveFocusOnModification.Create(RollResistanceCoefficientTextBox);

            TextBoxModificationBinding.Create(RollResistanceWithBrakesCoefficientTextBox);
            TextBoxMoveFocusOnModification.Create(RollResistanceWithBrakesCoefficientTextBox);
        }
    }
}