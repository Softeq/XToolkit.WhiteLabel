using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public interface IBackStackManager
    {
        void PushViewModel(IViewModelBase viewModel);
        void Clear();
        IViewModelBase PopViewModel();
        IViewModelBase GetExistingOrCreateViewModel(Type type);
        int Count { get; }
    }
}