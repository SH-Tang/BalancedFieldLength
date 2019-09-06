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

using System.Collections.ObjectModel;
using Application.BalancedFieldLength.Controls;
using Application.BalancedFieldLength.Views.TabViews;
using NUnit.Framework;
using WPF.Components.TabControl;

namespace Application.BalancedFieldLength.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var mainViewModel = new MainViewModel();

            // Assert
            Assert.That(mainViewModel.OutputViewModel, Is.Not.Null);
            Assert.That(mainViewModel.MessageWindowViewModel, Is.Not.Null);
            Assert.That(mainViewModel.TabControlViewModel, Is.Not.Null);

            TabControlViewModel tabControlViewModel = mainViewModel.TabControlViewModel;
            ObservableCollection<ITabViewModel> tabs = tabControlViewModel.Tabs;
            Assert.That(tabs, Has.Count.EqualTo(3));
            Assert.That(tabs[0], Is.TypeOf<GeneralSimulationSettingsTabViewModel>());
            Assert.That(tabs[1], Is.TypeOf<EngineSettingsTabViewModel>());
            Assert.That(tabs[2], Is.TypeOf<AircraftDataTabViewModel>());
            Assert.That(tabControlViewModel.SelectedTabItem, Is.SameAs(tabs[0]));
        }
    }
}