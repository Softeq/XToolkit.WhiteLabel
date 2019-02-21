// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private readonly IPlatformNavigationService _pageNavigationService;
        private readonly IBackStackManager _backStackManager;
        private readonly IIocContainer _iocContainer;

        public PageNavigationService(IPlatformNavigationService pageNavigationService,
            IBackStackManager backStackManager, IIocContainer iocContainer)
        {
            _pageNavigationService = pageNavigationService;
            _backStackManager = backStackManager;
            _iocContainer = iocContainer;
        }

        public bool CanGoBack => _pageNavigationService.CanGoBack;

        public void NavigateToViewModel<T>(bool clearBackStack = false, string screensGroupName = null)
            where T : IViewModelBase
        {
            NavigateToViewModel<T>(clearBackStack, null, screensGroupName);
        }

        public void Initialize(object navigation)
        {
            _pageNavigationService.Initialize(navigation);
        }

        public void GoBack()
        {
            if (_backStackManager.Count != 0)
            {
                _backStackManager.PopViewModel();
            }

            _pageNavigationService.GoBack();
        }

        public void PopScreensGroup(string groupName)
        {
            var targetViewModel = _pageNavigationService.PopScreensGroup(groupName);
            _backStackManager.PopTo(targetViewModel);
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>(this);
        }

        internal void NavigateToViewModel<T>(bool clearBackStack, IReadOnlyList<NavigationParameterModel> parameters, string screensGroupName)
            where T : IViewModelBase
        {
            if (clearBackStack)
            {
                _backStackManager.Clear();
            }

            var viewModel = _iocContainer.Resolve<T>() as ViewModelBase;
            viewModel.ApplyParameters(parameters);

            _pageNavigationService.NavigateToViewModel(viewModel, clearBackStack, parameters, screensGroupName);

            _backStackManager.PushViewModel(viewModel);
        }
    }
}