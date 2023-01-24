// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.RIB.Platform.Navigation
{
    public class RibNavigationService : IRibNavigationService
    {
        private readonly IPlatformNavigationService _platformNavigationService;
        private readonly IBackStackManager _backStackManager;

        public RibNavigationService(
            IPlatformNavigationService platformNavigationService,
            IBackStackManager backStackManager)
        {
            _platformNavigationService = platformNavigationService;
            _backStackManager = backStackManager;
        }

        public void NavigateToViewModel(ViewModelBase viewModel, IReadOnlyList<NavigationParameterModel> parameters = null)
        {
            if (parameters != null)
            {
                viewModel.ApplyParameters(parameters);
            }

            _platformNavigationService.NavigateToViewModel(viewModel, false, parameters);

            _backStackManager.PushViewModel(viewModel);
        }

        public void GoBack()
        {
            if (_backStackManager.Count != 0)
            {
                _backStackManager.PopViewModel();
            }

            _platformNavigationService.GoBack();
        }
    }
}
