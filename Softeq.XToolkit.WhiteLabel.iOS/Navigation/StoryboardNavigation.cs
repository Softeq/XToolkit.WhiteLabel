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

        public IViewModelBase PopScreensGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new System.ArgumentException($"{groupName} must not be empty");
            }
            var navigationStack = NavigationController.ChildViewControllers;
            if (!IsViewControllerFromGroup(navigationStack.Last(), groupName))
            {
                throw new System.Exception($"Top view controller does not belong to group {groupName}");
            }
            var targetViewController = navigationStack.LastOrDefault(x => !IsViewControllerFromGroup(x, groupName));
            if (targetViewController != null)
            {
                var targetModelToPop = GetViewModelFromViewController(targetViewController);
                NavigationController.PopToViewController(targetViewController, true);
                return targetModelToPop;
            }
            throw new System.Exception($"ViewController of group {groupName} was not found");
        }

        public void NavigateToViewModel(ViewModelBase viewModelBase, bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters, string screensGroupName)
        {
            viewModelBase.FrameNavigationService = this as IFrameNavigationService;
            var viewController = ViewLocator.GetView(viewModelBase);
            if (!string.IsNullOrEmpty(screensGroupName))
            {
                var screensGroupComponent = new ScreensGroupComponent(screensGroupName);
                (viewController as ViewControllerBase).ControllerComponents.Add(screensGroupComponent);
            }
            Navigate(viewController, clearBackStack);
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

        private static bool IsViewControllerFromGroup(UIViewController viewController, string groupName)
        {
            if (viewController is ViewControllerBase baseViewController)
            {
                var screensGroupComponent = baseViewController
                    .ControllerComponents.FirstOrDefault(x => x is ScreensGroupComponent) as ScreensGroupComponent;
                return screensGroupComponent?.ScreensGroupName == groupName;
            }
            return false;
        }

        private IViewModelBase GetViewModelFromViewController(UIViewController viewController)
        {
            if (viewController is ViewControllerBase)
            {
                return viewController.GetType().GetProperty("ViewModel").GetValue(viewController) as IViewModelBase;
            }
            return null;
        }
    }
}