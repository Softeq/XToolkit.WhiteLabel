// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public abstract class StoryboardNavigation
    {
        protected readonly IViewLocator ViewLocator;
        protected UINavigationController NavigationController;

        protected StoryboardNavigation(IViewLocator viewLocator)
        {
            ViewLocator = viewLocator;
        }

        public int BackStackCount => NavigationController.ChildViewControllers.Length;

        public bool CanGoBack => NavigationController.ViewControllers.Length > 1;

        public void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : ViewModelBase, IViewModelParameter<TParameter>
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView<T, TParameter>(parameter, this as IFrameNavigationService);
                NavigateToViewControllerImpl(controller, clearBackStack);
            });
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : ViewModelBase
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView<T>(this as IFrameNavigationService);
                NavigateToViewControllerImpl(controller, clearBackStack);
            });
        }

        public void NavigateToViewModel<T>(T t) where T : ViewModelBase
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView(t);
                NavigateToViewControllerImpl(controller, false);
            });
        }

        public void Initialize(object navigation)
        {
            NavigationController = navigation as UINavigationController;
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() => NavigationController.PopViewController(true));
        }

        private void NavigateToViewControllerImpl(UIViewController controller, bool clearBackStack)
        {
            if (clearBackStack)
            {
                NavigationController.SetViewControllers(new[] {controller}, false);
                return;
            }

            NavigationController.PushViewController(controller, true);
        }
    }
}