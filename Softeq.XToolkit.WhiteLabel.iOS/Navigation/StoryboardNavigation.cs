// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
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

        public void PopScreensGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new System.ArgumentException($"{groupName} must not be empty");
            }
            var i = NavigationController.ChildViewControllers.Length - 1;
            while (i > 0)
            {
                var viewController = NavigationController.ChildViewControllers[i] as ViewControllerBase;
                if (viewController == null || viewController.ScreensGroupName != groupName)
                {
                    break;
                }
                i--;
            }
            if (i >= 0)
            {
                var targetViewController = NavigationController.ChildViewControllers[i];
                NavigationController.PopToViewController(targetViewController, true);
            }
            else
            {
                throw new System.Exception($"ViewController of group {groupName} was not found");
            }
        }

        public void NavigateToViewModel(ViewModelBase viewModelBase, bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters)
        {
            viewModelBase.FrameNavigationService = this as IFrameNavigationService;
            Navigate(ViewLocator.GetView(viewModelBase), clearBackStack);
        }

        protected void Navigate(UIViewController controller, bool clearBackStack)
        {
            Execute.BeginOnUIThread(() =>
            {
                if (clearBackStack)
                {
                    NavigationController.SetViewControllers(new[] {controller}, false);
                    return;
                }

                NavigationController.PushViewController(controller, true);
            });
        }
    }
}