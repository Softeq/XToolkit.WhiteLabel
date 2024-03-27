// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    /// <summary>
    ///     Extension methods regarding navigation.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        ///     Adds multiple navigation parameters to the specified ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel to add parameters to.</param>
        /// <param name="parameters">List of navigation parameters to add.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="viewModel"/>
        ///     and <paramref name="parameters"/> cannot be <see langword="null"/>.
        /// </exception>
        public static void ApplyParameters(
            this IViewModelBase viewModel,
            IEnumerable<NavigationParameterModel> parameters)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            parameters.Apply(p => p.PropertyInfo.ToPropertyInfo().SetValue(viewModel, p.Value));
        }
    }
}
