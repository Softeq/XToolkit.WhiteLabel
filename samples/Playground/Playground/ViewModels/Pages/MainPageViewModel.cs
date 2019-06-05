// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Halo.Core.ViewModels.Tab;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;

namespace Playground.ViewModels.Pages
{
    public class MainPageViewModel : ToolbarViewModelBase
    {
        public MainPageViewModel(ITabNavigationService tabNavigationService) : base(tabNavigationService, 2)
        {
        }

        public override Type GetViewModel(int position)
        {
            switch(position)
            {
                case 0:
                    return typeof(RedViewModel);
                case 1:
                    return typeof(BlueViewModel);
                default:
                    throw new NotImplementedException();
            }
        }

        public void NavigateToCollectionsSample()
        {
        }
    }
}
