// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    public static class NavigationHelperExtensions
    {
        public static void ApplyParameters(this IViewModelBase viewmodel,
            IEnumerable<NavigationParameterModel> parameters)
        {
            parameters?.Apply(p => p.PropertyInfo.ToProperty().SetValue(viewmodel, p.Value));
        }
    }
}