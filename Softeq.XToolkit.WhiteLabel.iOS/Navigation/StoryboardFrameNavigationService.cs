// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardFrameNavigationService : StoryboardNavigation, IFrameNavigationService
    {
        public StoryboardFrameNavigationService(IViewLocator viewLocator) : base(viewLocator)
        {
        }

        bool IFrameNavigationService.IsInitialized => NavigationController != null;

        bool IFrameNavigationService.CanGoBack => CanGoBack;

        ViewModelBase IFrameNavigationService.CurrentViewModel => null;

        void IFrameNavigationService.GoBack()
        {
            GoBack();
        }

        void IFrameNavigationService.GoBack<T>()
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = NavigationController
                    .ChildViewControllers
                    .FirstOrDefault(x => x is ViewControllerBase<T>);
                if (controller != null)
                {
                    NavigationController.PopToViewController(controller, false);
                }
            });
        }

        void IFrameNavigationService.Initialize(object navigation)
        {
            Initialize(navigation);
        }

        void IFrameNavigationService.NavigateToViewModel<T, TParameter>(TParameter parameter)
        {
            NavigateToViewModel<T, TParameter>(parameter);
        }

        void IFrameNavigationService.NavigateToViewModel<T>(bool clearBackStack)
        {
            NavigateToViewModel<T>(clearBackStack);
        }

        void IFrameNavigationService.NavigateToViewModel<T>(T t)
        {
            NavigateToViewModel<T>(t);
        }

        void IFrameNavigationService.RestoreState()
        {
            //
        }

        private void NavigateToViewModel<T>(T t) where T : IViewModelBase
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView(t);
                Navigate(controller, false);
            });
        }
    }
}