// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Weak;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardNavigation : IPlatformNavigationService
    {
        protected readonly IViewLocator ViewLocator;

        private WeakReferenceEx<UINavigationController>? _navigationControllerRef;

        public StoryboardNavigation(IViewLocator viewLocator)
        {
            ViewLocator = viewLocator;
        }

        protected UINavigationController? NavigationController
        {
            get => _navigationControllerRef?.Target;
            set
            {
                if (value != null)
                {
                    _navigationControllerRef = WeakReferenceEx.Create(value);
                }
            }
        }

        public bool CanGoBack => NavigationController!.ViewControllers.Length > 1;

        public void Initialize(object navigation)
        {
            NavigationController = (UINavigationController) navigation;
        }

        public virtual void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                ViewLocator.GetTopViewController()?.View.EndEditing(true);

                NavigationController!.PopViewController(true);
            });
        }

        public virtual void NavigateToViewModel(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
        {
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
                var topViewController = ViewLocator.GetTopViewController();

                topViewController.View.EndEditing(true);

                if (clearBackStack)
                {
                    var animated = IsAnimatedWhenNavigateWithClear(NavigationController!, topViewController);
                    NavigationController!.SetViewControllers(new[] { controller }, animated);
                    return;
                }

                NavigationController!.PushViewController(controller, true);
            });
        }

        protected virtual bool IsAnimatedWhenNavigateWithClear(
            UINavigationController navigationController,
            UIViewController topViewController)
        {
            // YP: Workaround for iOS 13 issue, looks related to https://stackoverflow.com/a/23912009
            if (navigationController.ViewControllers.Length > 0)
            {
                return navigationController.ViewControllers[0] == topViewController;
            }
            return false;
        }

        public void GoBack<TViewModel>() where TViewModel : IViewModelBase
        {
            throw new System.NotImplementedException();
        }

        public void GoToRoot()
        {
            throw new System.NotImplementedException();
        }
    }
}