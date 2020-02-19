// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public interface IViewControllerComponent
    {
        string Key { get; }

        void Attach(UIViewController controller);
        void Detach(UIViewController? controller = null);
    }
}