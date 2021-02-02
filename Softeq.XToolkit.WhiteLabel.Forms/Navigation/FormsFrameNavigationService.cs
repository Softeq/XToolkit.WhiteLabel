// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsFrameNavigationService : IFrameNavigationService
    {
        private readonly IPlatformNavigationService _platformNavigationService;
        private readonly IContainer _container;

        public FormsFrameNavigationService(
            IPlatformNavigationService platformNavigationService,
            IContainer container)
        {
            _platformNavigationService = platformNavigationService;
            _container = container;
        }

        public bool IsInitialized => true;

        public bool IsEmptyBackStack => !_platformNavigationService.CanGoBack;

        public bool CanGoBack => _platformNavigationService.CanGoBack;

        public void Initialize(object navigation)
        {
            _platformNavigationService.Initialize(navigation);
        }

        public async Task GoBackAsync()
        {
            await _platformNavigationService.GoBackAsync();
        }

        public Task GoBackAsync<T>() where T : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public async Task NavigateToViewModelAsync<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase
        {
            var viewModel = _container.Resolve<TViewModel>();
            await _platformNavigationService.NavigateToViewModelAsync(viewModel, clearBackStack, parameters);
        }

        public Task NavigateToFirstPageAsync()
        {
            throw new NotImplementedException();
        }

        public void RestoreNavigation()
        {
            throw new NotImplementedException();
        }

        public void RestoreUnfinishedNavigation()
        {
            throw new NotImplementedException();
        }
    }
}
