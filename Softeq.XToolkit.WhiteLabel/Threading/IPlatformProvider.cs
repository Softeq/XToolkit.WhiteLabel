// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Threading
{
    /// <summary>
    ///     Interface for platform specific operations that need enlightenment.
    /// </summary>
    public interface IPlatformProvider
    {
        /// <summary>
        ///     Indicates whether or not the framework is in design-time mode.
        /// </summary>
        bool InDesignMode { get; }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void BeginOnUIThread(Action action);

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        Task OnUIThreadAsync(Action action);

        /// <summary>
        ///     Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void OnUIThread(Action action);
    }

    public static class UiThreadExtensions
    {
        public static void ContinueOnUiThread(this Task task, Action action)
        {
            task.ContinueWith(t => action.BeginOnUIThread()).ConfigureAwait(false);
        }

        public static void ContinueOnUiThread<T>(this Task<T> task, Action<T> action)
        {
            task.ContinueWith(t => Execute.BeginOnUIThread(() => 
            {
                action(t.Result);
            })).ConfigureAwait(false);
        }
    }
}