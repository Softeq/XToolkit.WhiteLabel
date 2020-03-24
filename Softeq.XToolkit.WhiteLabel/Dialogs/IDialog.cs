// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Base abstraction for apply extensions.
    /// </summary>
    public interface IDialog
    {
    }

    /// <summary>
    ///     The main contract of dialogue implementation.
    /// </summary>
    /// <typeparam name="T">Type of dialog result.</typeparam>
    public interface IDialog<T> : IDialog
    {
        /// <summary>
        ///    Shows dialog asynchronously with a return result of work.
        /// </summary>
        /// <returns>Dialog result.</returns>
        Task<T> ShowAsync();
    }
}
