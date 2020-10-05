// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    /// <summary>
    ///     A factory service that resolves ViewModel dependencies
    ///     using the built-in IoC container.
    /// </summary>
    public interface IViewModelFactoryService
    {
        /// <summary>
        ///     Gets the instance of the specified ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">
        ///     Type of ViewModel to resolve.
        /// </typeparam>
        /// <returns>
        ///     Instance of the <typeparamref name="TViewModel"/>.
        /// </returns>
        TViewModel ResolveViewModel<TViewModel>()
            where TViewModel : ObservableObject;
    }
}
