// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Components
{
    public partial class ConnectivityPageViewController : ViewControllerBase<ConnectivityPageViewModel>
    {
        public ConnectivityPageViewController(NativeHandle handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Bind(() => ViewModel.ConnectionStatus, () => ConnectionStatusLabel.Text);
            this.Bind(() => ViewModel.ConnectionTypes, () => ConnectionTypeLabel.Text);
        }
    }
}
