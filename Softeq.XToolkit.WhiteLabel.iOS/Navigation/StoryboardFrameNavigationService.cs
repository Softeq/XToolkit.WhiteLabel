// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardFrameNavigationService : StoryboardNavigation, IFrameNavigationService
    {
        private readonly IIocContainer _iocContainer;

        public StoryboardFrameNavigationService(IViewLocator viewLocator, IIocContainer iocContainer) : base(
            viewLocator)
        {
            _iocContainer = iocContainer;
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>(this);
        }

        bool IFrameNavigationService.IsInitialized => NavigationController != null;

        bool IFrameNavigationService.CanGoBack => CanGoBack;

        ViewModelBase IFrameNavigationService.CurrentViewModel => null;

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<T>();
            NavigateToViewModel(viewModel as ViewModelBase, clearBackStack, null);
        }

        public void NavigateToViewModel<T, TParameter>(TParameter parameter)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            var viewModel = _iocContainer.Resolve<T>();
            viewModel.Parameter = parameter;
            NavigateToViewModel(viewModel as ViewModelBase, false, null);
        }

        public void NavigateToViewModel<TViewModel>(IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(parameters);
            NavigateToViewModel(viewModel as ViewModelBase, false, null);
        }

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

        void IFrameNavigationService.NavigateToViewModel<T>(T t)
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView(t);
                Navigate(controller, false);
            });
        }

        void IFrameNavigationService.RestoreState()
        {
        }
    }
}