// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    public class PageFluentNavigator<TViewModel> : FluentNavigatorBase<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly IPageNavigationService _pageNavigationService;

        public PageFluentNavigator(IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
        }

        public PageFluentNavigator<TViewModel> WithParam<TValue>(
            Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            ApplyParameter(property, value);
            return this;
        }

        public PageFluentNavigator<TViewModel> WithParams(IEnumerable<NavigationParameterModel> navigationParameters)
        {
            Parameters.AddRange(navigationParameters);
            return this;
        }

        public void Navigate(bool clearBackStack = false)
        {
            _pageNavigationService.NavigateToViewModel<TViewModel>(clearBackStack, Parameters);
        }
    }
}
