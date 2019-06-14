using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers
{
    public class FrameNavigationHelper<TViewModel> : NavigationHelperBase<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public FrameNavigationHelper(IFrameNavigationService navigationService)
        {
            _frameNavigationService = navigationService;
        }

        public FrameNavigationHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            ApplyParameter(property, value);
            return this;
        }

        public FrameNavigationHelper<TViewModel> WithParams(IEnumerable<NavigationParameterModel> navigationHelperParameters)
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
