// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IBackStackManager
    {
        int Count { get; }

        void PushViewModel(IViewModelBase viewModel);

        IViewModelBase PopViewModel();

        void PopScreensGroup(string groupName);

        void Clear();

        TViewModel GetExistingOrCreateViewModel<TViewModel>() where TViewModel : IViewModelBase;
    }
}