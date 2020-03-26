// Developed by Softeq Development Corporation
// http://www.softeq.com

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

        public FrameFluentNavigator<TViewModel> Initialize(object initParameter)
        {
            _frameNavigationService.Initialize(initParameter);
            return this;
        }

        public void NavigateBack<TViewModelBack>() where TViewModelBack : IViewModelBase
        {
            _frameNavigationService.GoBack<TViewModelBack>();
        }

        public void NavigateBack()
        {
            _frameNavigationService.GoBack();
        }

        public void Navigate(bool clearBackStack = false)
        {
            _frameNavigationService.NavigateToViewModel<TViewModel>(clearBackStack, Parameters);
        }

        public void NavigateBackToRoot()
        {
            _frameNavigationService.NavigateToFirstPage();

        }
    }
}
