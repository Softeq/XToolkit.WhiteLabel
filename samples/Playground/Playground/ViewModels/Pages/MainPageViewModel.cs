// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.ViewModels.Pages
{
    public class MainPageViewModel : ToolbarViewModelBase
    {
        public MainPageViewModel(ITabNavigationService tabNavigationService) : base(tabNavigationService)
        {
            TabModels = new List<TabItem>
            {
                new TabItem("Chats", "Chat", typeof(RedViewModel)),
                new TabItem("Settings", "Settings", typeof(BlueViewModel))
            };
        }
    }
}
