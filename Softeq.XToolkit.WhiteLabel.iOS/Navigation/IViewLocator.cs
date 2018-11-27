// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public interface IViewLocator
    {
        [Obsolete("Use GetView()")]
        UIViewController GetView<T, TParameter>(TParameter parameter,
            IFrameNavigationService frameNavigationService = null)
            where T : IViewModelBase, IViewModelParameter<TParameter>;

        [Obsolete("Use GetView()")]
        UIViewController GetView<T>(IFrameNavigationService frameNavigationService = null)
            where T : IViewModelBase;

        UIViewController GetView(object model);

        UIViewController GetTopViewController();
    }
}