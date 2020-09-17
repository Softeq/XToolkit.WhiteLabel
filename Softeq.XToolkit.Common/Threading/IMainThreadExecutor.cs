// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Threading
{
    /// <summary>
    ///     Interface for platform specific operations that need enlightenment.
    /// </summary>
    public interface IMainThreadExecutor
    {
        /// <summary>
        ///     Gets a value indicating whether it is the current main UI thread.
        /// </summary>
        bool IsMainThread { get; }

        /// <summary>
        ///     Invokes asynchronously the specified code on the main UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void BeginOnUIThread(Action action);

        /// <summary>
        ///     Invokes asynchronously the specified code on the main UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        Task OnUIThreadAsync(Action action);

        /// <summary>
        ///     Invokes synchronously the specified code on the main UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void OnUIThread(Action action);
    }
}
