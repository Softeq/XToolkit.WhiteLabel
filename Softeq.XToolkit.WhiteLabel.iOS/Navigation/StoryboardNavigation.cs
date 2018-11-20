// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;
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

        private WeakReferenceEx<UINavigationController> _navigationControllerRef;

        protected StoryboardNavigation(IViewLocator viewLocator)
        {
            ViewLocator = viewLocator;
        }

        protected UINavigationController NavigationController
        {
            get => _navigationControllerRef?.Target;
            set => _navigationControllerRef = WeakReferenceEx.Create(value);
        }

        public int BackStackCount => NavigationController.ChildViewControllers.Length;

        public bool CanGoBack => NavigationController.ViewControllers.Length > 1;

        public void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView<T, TParameter>(parameter, this as IFrameNavigationService);
                NavigateToViewControllerImpl(controller, clearBackStack);
            });
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView<T>(this as IFrameNavigationService);
                NavigateToViewControllerImpl(controller, clearBackStack);
            });
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            var controller = (ViewControllerBase<T>) ViewLocator.GetView<T>(this as IFrameNavigationService);

            return new NavigateHelper<T>(controller.ViewModel,
                shouldClearBackStack => { NavigateToViewControllerImpl(controller, shouldClearBackStack); });
        }

        public void NavigateToViewModel<T>(T t) where T : IViewModelBase
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