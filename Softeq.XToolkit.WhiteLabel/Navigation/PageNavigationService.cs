// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     This class supports Xamarin Native navigation only.
    /// </summary>
    public class PageNavigationService : IPageNavigationService
    {
        private readonly IBackStackManager _backStackManager;
        private readonly IContainer _container;
        private readonly IPlatformNavigationService _platformNavigationService;

        public PageNavigationService(
            IPlatformNavigationService platformNavigationService,
            IBackStackManager backStackManager,
            IContainer container)
        {
            _platformNavigationService = platformNavigationService;
            _backStackManager = backStackManager;
            _container = container;
        }

        public bool CanGoBack => _platformNavigationService.CanGoBack;

        public void Initialize(object navigation)
        {
            _platformNavigationService.Initialize(navigation);
        }

        public void GoBack()
        {
            if (_backStackManager.Count != 0)
            {
                _backStackManager.PopViewModel();
            }

            _platformNavigationService.GoBack();
        }

        public void NavigateToViewModel<T>(
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
            where T : IViewModelBase
        {
            if (clearBackStack)
            {
                _backStackManager.Clear();
            }

            var viewModel = _container.Resolve<T>();

            if (parameters != null)
            {
                viewModel.ApplyParameters(parameters);
            }

            _platformNavigationService.NavigateToViewModel(viewModel, clearBackStack, parameters);

            _backStackManager.PushViewModel(viewModel);
        }
    }
}
