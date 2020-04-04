// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public static class FluentNavigatorExtensions
    {
        public static FrameFluentNavigator<TViewModel> From<TViewModel>(
            this FrameFluentNavigator<TViewModel> navigator,
            object source)
            where TViewModel : IViewModelBase
        {
            navigator.Initialize(source);
            return navigator;
        }
    }
}
