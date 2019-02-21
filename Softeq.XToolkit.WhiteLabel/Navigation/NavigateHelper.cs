// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class NavigateHelper<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly PageNavigationService _navigationService;
        private readonly List<NavigationParameterModel> _parameters = new List<NavigationParameterModel>();

        private string _screensGroupName;

        public NavigateHelper(PageNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public NavigateHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property, TValue value)
        {
            var parameter = new NavigationParameterModel { Value = value };

            var propertyInfo = (PropertyInfo)property.GetMemberInfo();
            parameter.PropertyInfo = PropertyInfoModel.FromProperty(propertyInfo);

            _parameters.Add(parameter);

            return this;
        }

        public NavigateHelper<TViewModel> InScreensGroup(string screensGroupName)
        {
            return WithParam(x => x.ScreensGroupName, screensGroupName);
        }

        public void Navigate(bool clearBackStack = false)
        {
            _navigationService.NavigateToViewModel<TViewModel>(clearBackStack, _parameters);
        }
    }

    public static class NavigationHelperExtensions
    {
        public static void ApplyParameters(this IViewModelBase viewmodel,
            IEnumerable<NavigationParameterModel> parameters)
        {
            parameters?.Apply(p => p.PropertyInfo.ToProperty().SetValue(viewmodel, p.Value));
        }
    }
}