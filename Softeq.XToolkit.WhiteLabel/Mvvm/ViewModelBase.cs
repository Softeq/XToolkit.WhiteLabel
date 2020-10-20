// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     Base implementation of the <see cref="IViewModelBase"/> interface.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IViewModelBase
    {
        private bool _isBusy;

        /// <summary>
        ///     Gets or sets a value indicating whether this ViewModel is doing some work right now.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        /// <inheritdoc/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc/>
        public virtual void OnInitialize()
        {
            IsInitialized = true;
        }

        /// <inheritdoc/>
        public virtual void OnAppearing()
        {
        }

        /// <inheritdoc/>
        public virtual void OnDisappearing()
        {
        }
    }
}
