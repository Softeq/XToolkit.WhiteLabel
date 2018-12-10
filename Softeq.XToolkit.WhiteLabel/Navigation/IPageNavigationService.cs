// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPageNavigationService
    {
        bool CanGoBack { get; }

        void Initialize(object navigation);

        void GoBack();

        NavigateHelper<T> For<T>() where T : IViewModelBase;

        void NavigateToViewModel<T>(bool clearBackStack = false)
            where T : IViewModelBase;
    }
}