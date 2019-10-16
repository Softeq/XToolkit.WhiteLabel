// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using Playground.ViewModels.Components;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.Bindings.Extensions;

namespace Playground.iOS.ViewControllers.Components
{
    public partial class ConnectivityPageViewController : ViewControllerBase<ConnectivityPageViewModel>
    {
        public ConnectivityPageViewController(IntPtr handle) : base(handle)
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

