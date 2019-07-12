// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Playground.ViewModels.BottomTabs.First;
using Playground.ViewModels.BottomTabs.Second;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.ViewModels.BottomTabs
{
    public class BottomTabsPageViewModel : ToolbarViewModelBase
    {
        public BottomTabsPageViewModel(ITabNavigationService tabNavigationService) : base(tabNavigationService)
        {
            TabModels = new List<TabItem>
            {
                new TabItem("First", "Chat", typeof(RedViewModel)),
                new TabItem("Second", "Settings", typeof(BlueViewModel))
            };
        }
    }
}
