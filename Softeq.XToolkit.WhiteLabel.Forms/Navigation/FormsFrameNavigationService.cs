// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
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

        public void GoBack()
        {
            _platformNavigationService.GoBack();
        }

        public void GoBack<T>() where T : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase
        {
            var viewModel = _container.Resolve<TViewModel>();
            _platformNavigationService.NavigateToViewModel(viewModel, clearBackStack, parameters);
        }

        public void NavigateToFirstPage()
        {
            throw new NotImplementedException();
        }

        public void RestoreNavigation()
        {
            throw new NotImplementedException();
        }
    }
}
