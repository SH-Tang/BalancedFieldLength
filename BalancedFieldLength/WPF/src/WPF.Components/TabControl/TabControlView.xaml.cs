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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Components.TabControl
{
    /// <summary>
    /// Interaction logic for TabControlView.xaml
    /// </summary>
    public partial class TabControlView : UserControl
    {
        public TabControlView()
        {
            InitializeComponent();
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This commits control values when the tab control changes tab.
            // See also: https://stackoverflow.com/questions/10208861/wpf-data-bound-tabcontrol-doesnt-commit-changes-when-new-tab-is-selected
            Keyboard.FocusedElement?.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
        }
    }
}