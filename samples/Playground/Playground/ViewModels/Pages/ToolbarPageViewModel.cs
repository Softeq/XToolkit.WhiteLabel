// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using Playground.ViewModels.Pages.Temp;

namespace Playground.ViewModels.Pages
{
    public class ToolbarPageViewModel : ToolbarViewModelBase
    {
        public ToolbarPageViewModel(ITabNavigationService tabNavigationService) : base(tabNavigationService)
        {
            TabModels = new List<TabItem>
            {
                new TabItem("Chats", "Chat", typeof(RedViewModel)),
                new TabItem("Settings", "Settings", typeof(BlueViewModel))
            };
        }
    }
}
