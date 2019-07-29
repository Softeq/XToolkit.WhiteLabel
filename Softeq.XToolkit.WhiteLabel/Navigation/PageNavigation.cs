// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private readonly IBackStackManager _backStackManager;
        private readonly IContainer _iocContainer;
        private readonly IPlatformNavigationService _pageNavigationService;

        public PageNavigationService(
            IPlatformNavigationService pageNavigationService,
            IBackStackManager backStackManager,
            IContainer iocContainer)
        {
            _pageNavigationService = pageNavigationService;
            _backStackManager = backStackManager;
            _iocContainer = iocContainer;
        }

        public void Initialize(object navigation)
        {
            _pageNavigationService.Initialize(navigation);
        }

        public bool CanGoBack => _pageNavigationService.CanGoBack;

        public void GoBack()
        {
            if (_backStackManager.Count != 0)
            {
                _backStackManager.PopViewModel();
            }

            _pageNavigationService.GoBack();
        }

        public PageFluentNavigator<T> For<T>() where T : IViewModelBase
        {
            return new PageFluentNavigator<T>(this);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false)
            where T : IViewModelBase
        {
            NavigateToViewModel<T>(clearBackStack, null);
        }

        internal void NavigateToViewModel<T>(
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters)
            where T : IViewModelBase
        {
            if (clearBackStack)
            {
                _backStackManager.Clear();
            }

            var viewModel = _iocContainer.Resolve<T>();

            viewModel.ApplyParameters(parameters);

            _pageNavigationService.NavigateToViewModel(viewModel, clearBackStack, parameters);

            _backStackManager.PushViewModel(viewModel);
        }
    }
}
