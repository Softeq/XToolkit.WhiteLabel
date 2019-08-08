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

        /// <summary>
        ///     Set value to the target ViewModel property.
        /// </summary>
        /// <typeparam name="TValue">Type of parameter value.</typeparam>
        /// <param name="propertyExpression">Target property.</param>
        /// <param name="value">Value for set.</param>
        /// <returns></returns>
        public FluentNavigatorBase<TViewModel> ApplyParameter<TValue>(
            Expression<Func<TViewModel, TValue>> propertyExpression,
            TValue value)
        {
            var parameter = new NavigationParameterModel { Value = value };

            var propertyInfo = (PropertyInfo) propertyExpression.GetMemberInfo();
            parameter.PropertyInfo = PropertyInfoModel.FromProperty(propertyInfo);

            Parameters.Add(parameter);

            return this;
        }
    }
}