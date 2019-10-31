// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public interface IViewModelBase
    {
        void OnInitialize();
        void OnAppearing();
        void OnDisappearing();
    }

    public interface IFrameViewModel
    {
        IFrameNavigationService FrameNavigationService { get; set; }
    }

    /// <summary>
    ///     A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IViewModelBase, IFrameViewModel
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public bool IsInitialized { get; private set; }

        // TODO YP: Rework to remove
        public IFrameNavigationService FrameNavigationService { get; set; } = default!;

        // TODO YP: Review using lifetime methods here
        public virtual void OnInitialize()
        {
            IsInitialized = true;
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }
}
