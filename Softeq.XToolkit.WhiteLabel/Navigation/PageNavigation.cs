using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private readonly IInternalNavigationService _pageNavigationService;
        private readonly IBackStackManager _backStackManager;
        private readonly IServiceLocator _serviceLocator;

        public PageNavigationService(IInternalNavigationService pageNavigationService,
            IBackStackManager backStackManager, IServiceLocator serviceLocator)
        {
            _pageNavigationService = pageNavigationService;
            _backStackManager = backStackManager;
            _serviceLocator = serviceLocator;
        }

        public bool CanGoBack => _pageNavigationService.CanGoBack;

        public void NavigateToViewModel<T>(bool clearBackStack = false)
            where T : IViewModelBase
        {
            NavigateToViewModel<T>(clearBackStack, null);
        }

        internal void NavigateToViewModel<T>(bool clearBackStack, IReadOnlyList<NavigationParameterModel> parameters)
            where T : IViewModelBase
        {
            if (clearBackStack)
            {
                _backStackManager.Clear();
            }

            var viewModel = _serviceLocator.Resolve<T>() as ViewModelBase;
            viewModel.ApplyParameters(parameters);

            _pageNavigationService.NavigateToViewModel(viewModel, clearBackStack, parameters);

            _backStackManager.PushViewModel(viewModel);
        }

        public void Initialize(object navigation)
        {
            _pageNavigationService.Initialize(navigation);
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                if (_backStackManager.Count != 0)
                {
                    _backStackManager.PopViewModel();
                }

                _pageNavigationService.GoBack();
            });
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>(this);
        }
    }
}