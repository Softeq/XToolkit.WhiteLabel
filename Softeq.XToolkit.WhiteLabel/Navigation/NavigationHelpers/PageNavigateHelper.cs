// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers
{
    public class PageNavigateHelper<TViewModel> : NavigationHelperBase<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly PageNavigationService _navigationService;

        public PageNavigateHelper(PageNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public PageNavigateHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            ApplyParameter(property, value);
            return this;
        }

        public PageNavigateHelper<TViewModel> WithParams(IEnumerable<NavigationParameterModel> navigationHelperParameters)
        {
            Parameters.AddRange(navigationHelperParameters);
            return this;
        }

        public void Navigate(bool clearBackStack = false)
        {
            _navigationService.NavigateToViewModel<TViewModel>(clearBackStack, Parameters);
        }
    }
}