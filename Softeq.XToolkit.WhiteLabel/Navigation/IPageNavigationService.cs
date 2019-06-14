// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPageNavigationService
    {
        void Initialize(object navigation);

        bool CanGoBack { get; }

        void GoBack();

        PageNavigateHelper<T> For<T>() where T : IViewModelBase;

        void NavigateToViewModel<T>(bool clearBackStack = false)
            where T : IViewModelBase;
    }
}