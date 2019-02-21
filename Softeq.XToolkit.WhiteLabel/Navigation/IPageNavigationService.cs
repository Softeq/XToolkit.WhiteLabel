// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IPageNavigationService
    {
        bool CanGoBack { get; }

        void Initialize(object navigation);

        void GoBack();

        void PopScreensGroup(string groupName);

        NavigateHelper<T> For<T>() where T : IViewModelBase;

        void NavigateToViewModel<T>(bool clearBackStack = false)
            where T : IViewModelBase;
    }
}