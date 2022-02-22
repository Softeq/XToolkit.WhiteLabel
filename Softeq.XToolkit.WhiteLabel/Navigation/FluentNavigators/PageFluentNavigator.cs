// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    /// <summary>
    ///     Navigator service that helps to setup and perform navigation to the specified ViewModel.
    /// </summary>
    /// <typeparam name="TViewModel">Type of ViewModel to perform navigation to.</typeparam>
    public class PageFluentNavigator<TViewModel> : FluentNavigatorBase<TViewModel> where TViewModel : IViewModelBase
    {
        private readonly IPageNavigationService _navigationService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PageFluentNavigator{TViewModel}"/> class.
        /// </summary>
        /// <param name="navigationService">
        ///     The <see cref="IPageNavigationService"/> implementation that will be used for navigation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="navigationService"/> cannot be <see langword="null"/>.
        /// </exception>
        public PageFluentNavigator(IPageNavigationService navigationService)
        {
            _navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        /// <summary>
        ///     Creates and adds navigation parameter for the specified property using reflection.
        /// </summary>
        /// <typeparam name="TValue">Type of parameter value.</typeparam>
        /// <param name="propertyExpression">Target property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Self.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="propertyExpression"/> cannot be <see langword="null"/>.
        /// </exception>
        public PageFluentNavigator<TViewModel> WithParam<TValue>(
            Expression<Func<TViewModel, TValue>> propertyExpression,
            TValue value)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            ApplyParameter(propertyExpression, value);
            return this;
        }

        /// <summary>
        ///     Adds multiple navigation parameters to the current instance.
        /// </summary>
        /// <param name="navigationParameters">List of navigation parameters to add.</param>
        /// <returns>Self.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="navigationParameters"/> cannot be <see langword="null"/>.
        /// </exception>
        public PageFluentNavigator<TViewModel> WithParams(IEnumerable<NavigationParameterModel> navigationParameters)
        {
            if (navigationParameters == null)
            {
                throw new ArgumentNullException(nameof(navigationParameters));
            }

            _parameters.AddRange(navigationParameters);
            return this;
        }

        /// <summary>
        ///     Performs navigation to the previously specified ViewModel with the list of parameters
        ///     specified with <see cref="WithParam{TValue}(Expression{Func{TViewModel, TValue}}, TValue)"/>
        ///     and <see cref="WithParams(IEnumerable{NavigationParameterModel})"/> methods.
        /// </summary>
        /// <param name="clearBackStack">
        ///     Boolean value indicating if this service should clear backstack after the navigation is performed.
        /// </param>
        public void Navigate(bool clearBackStack = false)
        {
            _navigationService.NavigateToViewModel<TViewModel>(clearBackStack, Parameters);
        }
    }
}
