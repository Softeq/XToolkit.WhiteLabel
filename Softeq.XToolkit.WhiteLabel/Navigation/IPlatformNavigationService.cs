﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPlatformNavigationService
    {
        void NavigateToViewModel(ViewModelBase viewModelBase, bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters);

        bool CanGoBack { get; }

        void Initialize(object navigation);

        void GoBack();
        void GoBack<T>() where T : IViewModelBase;
    }
}