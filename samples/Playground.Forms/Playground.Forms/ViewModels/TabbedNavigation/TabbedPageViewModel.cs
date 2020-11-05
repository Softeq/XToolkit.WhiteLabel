// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class TabbedPageViewModel : ViewModelBase, ITabbedViewModel
    {
        public TabbedPageViewModel(Tab1PageViewModel tab1, RootFrameNavigationPageViewModel<Tab2PageViewModel> tab2)
        {
            TabsViewModel = new List<IViewModelBase> { tab1, tab2 };
        }

        public string? Title { get; set; } = "Tabbed Page";

        public IList<IViewModelBase> TabsViewModel { get; }
    }
}
