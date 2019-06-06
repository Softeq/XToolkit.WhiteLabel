// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.ViewModels.Pages
{
    public class MainPageViewModel : ToolbarViewModelBase
    {
        public MainPageViewModel(ITabNavigationService tabNavigationService) : base(tabNavigationService)
        {
        }
    }
}
