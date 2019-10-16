// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardNavigation : IPlatformNavigationService
    {
        protected readonly IViewLocator ViewLocator;

        private WeakReferenceEx<UINavigationController> _navigationControllerRef;

        public StoryboardNavigation(IViewLocator viewLocator)
        {
            ViewLocator = viewLocator;
        }

        protected UINavigationController NavigationController
        {
            get => _navigationControllerRef?.Target;
            set => _navigationControllerRef = WeakReferenceEx.Create(value);
        }

        public bool CanGoBack => NavigationController.ViewControllers.Length > 1;

        public void Initialize(object navigation)
        {
            NavigationController = navigation as UINavigationController;
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() => { NavigationController.PopViewController(true); });
        }

        public void NavigateToViewModel(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters)
        {
            if (viewModelBase is IFrameViewModel frameViewModel)
            {
                frameViewModel.FrameNavigationService = this as IFrameNavigationService;
            }

            if (parameters != null)
            {
                viewModelBase.ApplyParameters(parameters);
            }

            Navigate(ViewLocator.GetView(viewModelBase), clearBackStack);
        }

        protected virtual void Navigate(UIViewController controller, bool clearBackStack)
        {
            Execute.BeginOnUIThread(() =>
            {
                if (clearBackStack)
                {
                    NavigationController.SetViewControllers(new[] { controller }, false);
                    return;
                }

                NavigationController.PushViewController(controller, true);
            });
        }
    }
}
