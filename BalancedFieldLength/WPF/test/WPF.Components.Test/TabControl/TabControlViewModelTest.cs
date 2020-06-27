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
using NUnit.Framework;
using WPF.Components.TabControl;

namespace WPF.Components.Test.TabControl
{
    [TestFixture]
    public class TabControlViewModelTest
    {
        [Test]
        public static void Constructor_ExpectedValues()
        {
            // Call
            var tabControlViewModel = new TabControlViewModel();

            // Assert
            Assert.That(tabControlViewModel, Is.InstanceOf<INotifyPropertyChanged>());
            Assert.That(tabControlViewModel, Is.InstanceOf<IDisposable>());
            Assert.That(tabControlViewModel.Tabs, Is.Empty);
        }

        [Test]
        public void GivenTabControlViewModelWithTabs_WhenTabRaisesPropertyChangedEvent_ThenTabControlViewRaisesPropertyChangedEvent()
        {
            // Given
            var tabControlViewModel = new TabControlViewModel();

            PropertyChangedEventArgs eventArgs = null;
            bool propertyChangedTriggered = false;
            tabControlViewModel.PropertyChanged += (o, e) =>
            {
                eventArgs = e;
                propertyChangedTriggered = true;
            };

            var tabViewModel = new TestTabViewModel();
            tabControlViewModel.Tabs.Add(tabViewModel);

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
            var tabControlViewModel = new TabControlViewModel();

            PropertyChangedEventArgs eventArgs = null;
            bool propertyChangedTriggered = false;
            tabControlViewModel.PropertyChanged += (o, e) =>
            {
                eventArgs = e;
                propertyChangedTriggered = true;
            };

            var tabViewModel = new TestTabViewModel();
            tabControlViewModel.Tabs.Add(tabViewModel);

            var random = new Random(21);

            // When 
            bool removeSuccessful = tabControlViewModel.Tabs.Remove(tabViewModel);
            Assert.That(removeSuccessful, Is.True);

            tabViewModel.TabProperty = random.Next();

            // Then
            Assert.That(propertyChangedTriggered, Is.False);
            Assert.That(eventArgs, Is.Null);
        }

        [Test]
        public void GivenTabControlViewModelWithTabs_WhenViewModelDisposedAndTabRaisesPropertyChangedEvent_ThenTabControlDoesNotRaiseEvent()
        {
            // Given
            var tabControlViewModel = new TabControlViewModel();

            PropertyChangedEventArgs eventArgs = null;
            bool propertyChangedTriggered = false;
            tabControlViewModel.PropertyChanged += (o, e) =>
            {
                eventArgs = e;
                propertyChangedTriggered = true;
            };

            var tabViewModel = new TestTabViewModel();
            tabControlViewModel.Tabs.Add(tabViewModel);

            var random = new Random(21);

            // Precondition
            CollectionAssert.AreEqual(new[]
            {
                tabViewModel
            }, tabControlViewModel.Tabs);

            // When 
            tabControlViewModel.Dispose();

            tabViewModel.TabProperty = random.Next();

            // Then
            Assert.That(propertyChangedTriggered, Is.False);
            Assert.That(eventArgs, Is.Null);
            Assert.That(tabControlViewModel.Tabs, Is.Empty);
        }


        [Test]
        public void SelectedTabItem_WithValidValue_RaisesPropertyChangedEvent()
        {
            // Setup
            var tabControlViewModel = new TabControlViewModel();

            bool propertyChangedTriggered = false;
            PropertyChangedEventArgs eventArgs = null;
            tabControlViewModel.PropertyChanged += (o, e) =>
            {
                propertyChangedTriggered = true;
                eventArgs = e;
            };

            var tabViewModel = new TestTabViewModel();
            tabControlViewModel.Tabs.Add(tabViewModel);

            // Call 
            tabControlViewModel.SelectedTabItem = tabViewModel;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);

            Assert.That(eventArgs, Is.Not.Null);
            Assert.That(eventArgs.PropertyName, Is.EqualTo(nameof(TabControlViewModel.SelectedTabItem)));
        }

        [Test]
        public void SelectedTabItem_WithItemNotInTabs_ThrowsArgumentException()
        {
            // Setup
            var tabControlViewModel = new TabControlViewModel();

            // Call 
            TestDelegate call = () => tabControlViewModel.SelectedTabItem = new TestTabViewModel();

            // Assert
            Assert.That(call, Throws.ArgumentException.And.Message.EqualTo("Item could not be found."));
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