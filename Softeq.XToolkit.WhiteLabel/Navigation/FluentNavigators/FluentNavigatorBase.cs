// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    public class FluentNavigatorBase<TViewModel> where TViewModel : IViewModelBase
    {
        public readonly List<NavigationParameterModel> Parameters;

        public FluentNavigatorBase()
        {
            Parameters = new List<NavigationParameterModel>();
        }

        protected void ApplyParameter<TValue>(Expression<Func<TViewModel, TValue>> property,
            TValue value)
        {
            var parameter = new NavigationParameterModel { Value = value };

            var propertyInfo = (PropertyInfo) property.GetMemberInfo();
            parameter.PropertyInfo = PropertyInfoModel.FromProperty(propertyInfo);

            Parameters.Add(parameter);
        }
    }
}