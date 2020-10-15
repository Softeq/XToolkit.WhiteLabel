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
    /// <summary>
    ///    Base implementation of navigator service that helps to setup and perform navigation to the specified ViewModel.
    /// </summary>
    /// <typeparam name="TViewModel">Type of ViewModel to perform navigation to.</typeparam>
    public abstract class FluentNavigatorBase<TViewModel> where TViewModel : IViewModelBase
    {
        /// <summary>
        ///     Gets the list of navigation parameters for the current navigator instance.
        /// </summary>
        public List<NavigationParameterModel> Parameters { get; } = new List<NavigationParameterModel>();

        /// <summary>
        ///     Creates and adds navigation parameter for the specified property using reflection.
        /// </summary>
        /// <typeparam name="TValue">Type of parameter value.</typeparam>
        /// <param name="propertyExpression">Target property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Self.</returns>
        protected FluentNavigatorBase<TViewModel> ApplyParameter<TValue>(
            Expression<Func<TViewModel, TValue>> propertyExpression,
            TValue value)
        {
            var propertyInfo = (PropertyInfo) propertyExpression.GetMemberInfo();
            var parameter = new NavigationParameterModel(value, new PropertyInfoModel(propertyInfo));

            Parameters.Add(parameter);

            return this;
        }
    }
}