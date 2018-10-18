// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public interface IViewLocator
    {
        ViewControllerBase<T> GetView<T, TParameter>(TParameter parameter,
            IFrameNavigationService frameNavigationService = null)
            where T : ViewModelBase, IViewModelParameter<TParameter>;

        UIViewController GetView<T>(IFrameNavigationService frameNavigationService = null)
            where T : ViewModelBase;

        UIViewController GetView(object model);
    }
}