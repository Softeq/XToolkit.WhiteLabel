// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPageNavigationService
    {
        bool CanGoBack { get; }

        [ObsoleteAttribute(
            "Will be removed in future versions. Please use For<T>().WithParam(x=>x.ParameterName, parameterValue) method in NavigationService")]
        void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>;

        void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase;

        void Initialize(object navigation);

        void GoBack();

        NavigateHelper<T> For<T>() where T : IViewModelBase;
    }
}