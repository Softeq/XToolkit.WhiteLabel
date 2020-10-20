// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     The main contract of dialog implementation.
    /// </summary>
    /// <typeparam name="T">Type of dialog result.</typeparam>
    public interface IDialog<T>
    {
        /// <summary>
        ///    Shows dialog asynchronously.
        /// </summary>
        /// <returns>Dialog result.</returns>
        Task<T> ShowAsync();
    }
}
