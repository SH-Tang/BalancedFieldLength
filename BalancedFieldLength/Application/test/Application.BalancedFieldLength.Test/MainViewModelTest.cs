using System.Collections.ObjectModel;
using Application.BalancedFieldLength.Controls;
using NUnit.Framework;

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