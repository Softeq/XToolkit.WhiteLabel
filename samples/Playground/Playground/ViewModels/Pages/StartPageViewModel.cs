// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.ViewModels.Pages
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public StartPageViewModel(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            IList<TabItem> tabs = new List<TabItem>
            {
                new TabItem("Chats", "Chat", typeof(RedViewModel)),
                new TabItem("Settings", "Settings", typeof(BlueViewModel))
            };

            _pageNavigationService.For<MainPageViewModel>()
                .WithParam(x => x.TabModels, tabs)
                .Navigate(true);
        }
    }
}
