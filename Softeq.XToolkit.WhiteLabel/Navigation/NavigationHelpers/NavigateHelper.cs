// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers
{
    public class NavigateHelper<TViewModel> : NavigationHelper<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly PageNavigationService _navigationService;
        private readonly IFrameNavigationService _frameNavigationService;

        public NavigateHelper(PageNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public NavigateHelper(IFrameNavigationService navigationService)
        {
            _frameNavigationService = navigationService;
        }

        public new NavigateHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            base.WithParam(property, value);
            return this;
        }

        public NavigateHelper<TViewModel> WithParams(IEnumerable<NavigationParameterModel> navigationHelperParameters)
        {
            Parameters.AddRange(navigationHelperParameters);
            return this;
        }

        public void Navigate(bool clearBackStack = false)
        {
            if (_navigationService != null)
            {
                _navigationService.NavigateToViewModel<TViewModel>(clearBackStack, Parameters);
            }
            else
            {
                _frameNavigationService.NavigateToViewModel<TViewModel>(Parameters);
            }
        }
    }
}