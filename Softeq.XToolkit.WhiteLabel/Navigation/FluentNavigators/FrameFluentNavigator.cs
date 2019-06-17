using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    public class FrameFluentNavigator<TViewModel> : FluentNavigatorBase<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public FrameFluentNavigator(IFrameNavigationService navigationService)
        {
            _frameNavigationService = navigationService;
        }

        public FrameFluentNavigator<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            ApplyParameter(property, value);
            return this;
        }

        public FrameFluentNavigator<TViewModel> WithParams(IEnumerable<NavigationParameterModel> navigationHelperParameters)
        {
            Parameters.AddRange(navigationHelperParameters);
            return this;
        }

        public void Navigate()
        {
            _frameNavigationService.NavigateToViewModel<TViewModel>(Parameters);
        }
    }
}
