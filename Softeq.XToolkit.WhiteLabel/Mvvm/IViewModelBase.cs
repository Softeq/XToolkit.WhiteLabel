// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    /// <summary>
    ///     A base interface for the ViewModel classes in the MVVM pattern.
    ///     Each <see cref="IViewModelBase"/> should correspond to the page or some other UI component.
    /// </summary>
    public interface IViewModelBase
    {
        /// <summary>
        ///     Gets a value indicating whether this ViewModel is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Called when the page or UI component corresponding to this ViewModel
        ///     is initializing.
        /// </summary>
        void OnInitialize();

        /// <summary>
        ///     Called when the page or UI component corresponding to this ViewModel
        ///     is appearing on the screen.
        /// </summary>
        void OnAppearing();

        /// <summary>
        ///     Called when the page or UI component corresponding to this ViewModel
        ///     is disappearring from the screen.
        /// </summary>
        void OnDisappearing();
    }
}
