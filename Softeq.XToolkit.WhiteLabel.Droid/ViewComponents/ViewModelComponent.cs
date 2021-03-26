// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public sealed class ViewModelComponent<TViewModel> where TViewModel : class
    {
        private TViewModel? _viewModel;

        public ViewModelComponent(string viewModelKey)
        {
            ViewModelKey = viewModelKey;
        }

        public TViewModel ViewModel => _viewModel ?? throw new InvalidOperationException("ViewModel instance is not assigned");

        public string ViewModelKey { get; }

        public void Initialize(TViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void SaveViewModel(FragmentManager fragmentManager)
        {
            var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
            viewModelStore.Add(ViewModelKey, ViewModel);
        }

        public void RestoreViewModel(FragmentManager fragmentManager)
        {
            var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
            _viewModel = viewModelStore.Get<TViewModel>(ViewModelKey);
        }

        public void ClearViewModel(FragmentManager fragmentManager)
        {
            var viewModelStore = Internal.ViewModelStore.Of(fragmentManager);
            viewModelStore.Remove(ViewModelKey);
        }
    }
}
