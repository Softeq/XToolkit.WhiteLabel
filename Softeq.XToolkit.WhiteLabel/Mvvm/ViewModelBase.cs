// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     A base class for the ViewModel classes in the MVVM pattern.
    ///     Each ViewModel should correspond to the page or some other UI component.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IViewModelBase
    {
        private bool _isBusy;

        /// <summary>
        ///     Gets or sets a value indicating whether this ViewModel is doing some work.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        /// <summary>
        ///     Gets a value indicating whether this ViewModel is initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Called when the page or UI component corresponding to this ViewModel
        ///     is initializing.
        /// </summary>
        public virtual void OnInitialize()
        {
            IsInitialized = true;
        }

        /// <summary>
        ///     Called when the page or UI component corresponding to this ViewModel
        ///     is appearing on the screen.
        /// </summary>
        public virtual void OnAppearing()
        {
        }

        /// <summary>
        ///     Called when the page or UI component corresponding to this ViewModel
        ///     is disappearring from the screen.
        /// </summary>
        public virtual void OnDisappearing()
        {
        }
    }
}
