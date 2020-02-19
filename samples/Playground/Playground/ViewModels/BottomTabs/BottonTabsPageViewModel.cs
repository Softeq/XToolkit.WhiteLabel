// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Playground.ViewModels.BottomTabs.First;
using Playground.ViewModels.BottomTabs.Second;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.ViewModels.BottomTabs
{
    public class BottomTabsPageViewModel : ToolbarViewModelBase<string>
    {
        public BottomTabsPageViewModel()
        {
            TabModels = new List<TabItem<string>>
            {
                new TabItem<RedViewModel, string>("First", "Chat"),
                new TabItem<BlueViewModel, string>("Second", "Settings")
            };
        }
    }
}