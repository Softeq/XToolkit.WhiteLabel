// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public interface IViewLocator
    {
        UIViewController GetView(object viewModel);

        UIViewController GetTopViewController();
    }
}
