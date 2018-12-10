﻿using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IInternalNavigationService
    {
        void NavigateToViewModel(ViewModelBase viewModelBase, bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters);
        
        bool CanGoBack { get; }
        
        void Initialize(object navigation);
        
        void GoBack();
    }
}