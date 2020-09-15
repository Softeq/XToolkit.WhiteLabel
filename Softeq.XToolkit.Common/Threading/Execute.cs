// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Threading
{
    /// <summary>
    ///     Enables easy marshalling of code to the UI thread.
    /// </summary>
    public static class Execute
    {
        /// <summary>
        ///     Gets or sets the current <see cref="IMainThreadExecutor" />.
        /// </summary>
        public static IMainThreadExecutor? CurrentExecutor { get; set; } = default!;

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void BeginOnUIThread(this Action action)
        {
            CurrentExecutor.BeginOnUIThread(action);
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static Task OnUIThreadAsync(this Action action)
        {
            return CurrentExecutor.OnUIThreadAsync(action);
        }

        /// <summary>
        ///     Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void OnUIThread(this Action action)
        {
            CurrentExecutor.OnUIThread(action);
        }
    }
}
