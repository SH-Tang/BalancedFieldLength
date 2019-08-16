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
using System.Collections.Specialized;
using System.ComponentModel;

namespace Application.BalancedFieldLength.Controls
{
    /// <summary>
    /// The view model for a control that hosts tabs.
    /// </summary>
    public class TabControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of <see cref="TabControlViewModel"/>.
        /// </summary>
        public TabControlViewModel()
        {
            Tabs = new ObservableCollection<ITabViewModel>();
            Tabs.CollectionChanged += OnTabItemsChanged;
        }

        /// <summary>
        /// Gets the collection of tabs that are present within this control.
        /// </summary>
        public ObservableCollection<ITabViewModel> Tabs { get; }

        private void OnTabItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (ITabViewModel tab in e.NewItems)
                {
                    tab.PropertyChanged += TabPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (ITabViewModel tab in e.OldItems)
                {
                    tab.PropertyChanged -= TabPropertyChanged;
                }
            }
        }

        private void TabPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
    }
}