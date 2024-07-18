// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class DetailsPageViewController : ViewControllerBase<DetailsPageViewModel>
    {
        public DetailsPageViewController(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Navigation";
        }
    }
}
