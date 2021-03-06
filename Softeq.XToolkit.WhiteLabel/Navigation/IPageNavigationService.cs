﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPageNavigationService
    {
        void Initialize(object navigation);

        bool CanGoBack { get; }

        void GoBack();

        PageFluentNavigator<T> For<T>() where T : IViewModelBase;

        void NavigateToViewModel<T>(
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
            where T : IViewModelBase;
    }
}
