// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Components
{
    public partial class PushNotificationsPageViewController : ViewControllerBase<PushNotificationsPageViewModel>
    {
        public PushNotificationsPageViewController(IntPtr handle) : base(handle)
        {
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            RegisterBtn.SetCommand(ViewModel.RegisterCommand);
            UnregisterBtn.SetCommand(ViewModel.UnregisterCommand);
        }
    }
}
