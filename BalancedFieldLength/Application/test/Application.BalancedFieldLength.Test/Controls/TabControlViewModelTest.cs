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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Application.BalancedFieldLength.Controls;
using NUnit.Framework;

namespace Application.BalancedFieldLength.Test.Controls
{
    [TestFixture]
    public class TabControlViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var viewModel = new TabControlViewModel();

            // Assert
            Assert.That(viewModel, Is.InstanceOf<INotifyPropertyChanged>());
            Assert.That(viewModel.Tabs, Is.Empty);
        }

        [Test]
        public void GivenTabControlViewModelWithTabs_WhenTabRaisesPropertyChangedEvent_ThenTabControlViewRaisesPropertyChangedEvent()
        {
            // Given
            var viewModel = new TabControlViewModel();

            PropertyChangedEventArgs eventArgs = null;
            bool propertyChangedTriggered = false;
            viewModel.PropertyChanged += (o, e) =>
                                         {
                                             eventArgs = e;
                                             propertyChangedTriggered = true;
                                         };

            var tabViewModel = new TestTabViewModel();
            viewModel.Tabs.Add(tabViewModel);

            var random = new Random(21);

            // When 
            tabViewModel.TabProperty = random.Next();

            // Then
            Assert.That(propertyChangedTriggered, Is.True);
            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(TestTabViewModel.TabProperty)));
        }

        [Test]
        public void GivenTabControlViewModelWithTabs_WhenRemovingTabAndRemovedTabRaisesPropertyChangedEvent_ThenTabControlDoesNotRaiseEvent()
        {
            // Given
            var viewModel = new TabControlViewModel();

            PropertyChangedEventArgs eventArgs = null;
            bool propertyChangedTriggered = false;
            viewModel.PropertyChanged += (o, e) =>
                                         {
                                             eventArgs = e;
                                             propertyChangedTriggered = true;
                                         };

            var tabViewModel = new TestTabViewModel();
            viewModel.Tabs.Add(tabViewModel);

            var random = new Random(21);

            // When 
            bool removeSuccessful = viewModel.Tabs.Remove(tabViewModel);
            Assert.That(removeSuccessful, Is.True);

            tabViewModel.TabProperty = random.Next();

            // Then
            Assert.That(propertyChangedTriggered, Is.False);
            Assert.That(eventArgs, Is.Null);
        }


        private class TestTabViewModel : ITabViewModel
        {
            private int tabProperty;
            public event PropertyChangedEventHandler PropertyChanged;

            public int TabProperty
            {
                get
                {
                    return tabProperty;
                }
                set
                {
                    tabProperty = value;
                    OnPropertyChanged(nameof(TabProperty));
                }
            }

            public string TabName
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}