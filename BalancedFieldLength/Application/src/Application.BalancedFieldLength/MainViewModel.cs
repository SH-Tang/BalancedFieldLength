using Application.BalancedFieldLength.Controls;

namespace Application.BalancedFieldLength
{
    /// <summary>
    /// The view model of the application.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel()
        {
            var tabControlViewModel = new TabControlViewModel();
            var generalSettingsTab = new GeneralSimulationSettingsTabViewModel();
            tabControlViewModel.Tabs.Add(generalSettingsTab);
            tabControlViewModel.SelectedTabItem = generalSettingsTab;

            TabControlViewModel = tabControlViewModel;
        }

        public TabControlViewModel TabControlViewModel { get; }
    }
}