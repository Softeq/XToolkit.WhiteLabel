// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public interface IViewModelBase
    {
        void OnInitialize();
        void OnAppearing();
        void OnDisappearing();
    }

    /// <summary>
    ///     A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IViewModelBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public bool IsInitialized { get; private set; }

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
