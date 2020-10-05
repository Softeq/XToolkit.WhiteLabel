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
        ///     Gets the instance of the specified ViewModel with the specified parameter.
        /// </summary>
        /// <typeparam name="TViewModel">
        ///     Type of ViewModel to resolve.
        /// </typeparam>
        /// <typeparam name="TParam">
        ///     Type of ViewModel parameter.
        /// </typeparam>
        /// <param name="param">ViewModel parameter.</param>
        /// <returns>
        ///     Instance of the <typeparamref name="TViewModel"/>
        ///     with the Parameter property set to <paramref name="param"/>.
        /// </returns>
        TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : ObservableObject, IViewModelParameter<TParam>;

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
