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

        public void GoBack<T>() where T : IViewModelBase
        {
            var i = NavigationController.ChildViewControllers.Length - 1;
            while (i > 0 && !(NavigationController.ChildViewControllers[i] is ViewControllerBase<T>))
            {
                i--;
            }
            if (i > 0)
            {
                var targetViewController = NavigationController.ChildViewControllers[i - 1];
                NavigationController.PopToViewController(targetViewController, true);
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