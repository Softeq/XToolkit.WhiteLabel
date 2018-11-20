// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPageNavigationService
    {
        int BackStackCount { get; }

        bool CanGoBack { get; }

        void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>;

        void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase;

        void Initialize(object navigation);

        void GoBack();

        void RestoreState();

        NavigateHelper<T> For<T>() where T : IViewModelBase;
    }
}