// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.ViewModels.Pages
{
    public class MainPageViewModel : ToolbarViewModelBase
    {
        public MainPageViewModel(ITabNavigationService tabNavigationService) 
            : base(tabNavigationService, 2)
        {
        }

        protected override Type GetViewModel(int position)
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
    }
}
