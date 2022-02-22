// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public static class FormsFluentNavigatorExtensions
    {
        [Obsolete("Use `_frameNavigation.From(this)` before using navigator instead of this.")]
        public static FrameFluentNavigator<TViewModel> From<TViewModel>(
            this FrameFluentNavigator<TViewModel> navigator,
            object source)
            where TViewModel : IViewModelBase
        {
            navigator.Initialize(source);
            return navigator;
        }

        // TODO YP: Frame navigation needs to be redesigned for the future (forms/maui).
        public static IFrameNavigationService From<TViewModel>(
            this IFrameNavigationService navigationService,
            TViewModel source)
            where TViewModel : IViewModelBase
        {
            navigationService.Initialize(source);
            return navigationService;
        }
    }
}
