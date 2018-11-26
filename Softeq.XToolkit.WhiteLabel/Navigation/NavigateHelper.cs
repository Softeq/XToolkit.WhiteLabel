using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public class NavigateHelper<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly Action<bool, IReadOnlyList<NavigationParameterModel>> _navigateAction;
        private readonly List<NavigationParameterModel> _parameters = new List<NavigationParameterModel>();

        public NavigateHelper(Action<bool, IReadOnlyList<NavigationParameterModel>> navigateAction)
        {
            _navigateAction = navigateAction;
        }

        public NavigateHelper<TViewModel> WithParam<TValue>(Expression<Func<TViewModel, TValue>> property, TValue value)
        {
            var parameter = new NavigationParameterModel {Value = value};

            var propertyInfo = (PropertyInfo) property.GetMemberInfo();
            parameter.PropertyInfo = new PropertyInfoModel(propertyInfo);

            _parameters.Add(parameter);

            return this;
        }

        public static void ApplyParametersToViewModel(IViewModelBase viewmodel,
            IReadOnlyList<NavigationParameterModel> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                parameter.PropertyInfo.ToProperty().SetValue(viewmodel, parameter.Value);
            }
        }

        public void Navigate(bool clearBackStack = false)
        {
            _navigateAction(clearBackStack, _parameters);
        }
    }
}