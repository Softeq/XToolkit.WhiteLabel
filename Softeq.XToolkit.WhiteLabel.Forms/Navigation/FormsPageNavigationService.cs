// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsPageNavigationService : IPageNavigationService
    {
        private readonly IContainer _container;
        private readonly IPlatformNavigationService _platformNavigationService;

        public FormsPageNavigationService(
            IContainer container,
            IPlatformNavigationService platformNavigationService)
        {
            _container = container;
            _platformNavigationService = platformNavigationService;
        }

        public bool CanGoBack => _platformNavigationService.CanGoBack;

        public void Initialize(object navigation)
        {
            _platformNavigationService.Initialize(navigation);
        }

        public void GoBack()
        {
            _platformNavigationService.GoBack();
        }

        public void NavigateToViewModel<T>(
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
            where T : IViewModelBase
        {
            var viewModel = _container.Resolve<T>();
            _platformNavigationService.NavigateToViewModel(viewModel, clearBackStack, parameters);
        }
    }
}
