// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardPageNavigation : StoryboardNavigation, IPageNavigationService
    {
        public StoryboardPageNavigation(IViewLocator viewLocator, IServiceLocator serviceLocator) : base(viewLocator,
            serviceLocator)
        {
        }
    }
}