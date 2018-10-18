// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardPageNavigation : StoryboardNavigation, IPageNavigationService
    {
        public StoryboardPageNavigation(IViewLocator viewLocator) : base(viewLocator)
        {
        }

        public void RestoreState()
        {
            //not used in current platform
        }
    }
}