// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.WhiteLabel.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="action">The action to execute.</param>
        public static void ContinueOnUIThread(this Task task, Action action)
        {
            task.ContinueWith(t => action.BeginOnUIThread()).ConfigureAwait(false);
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="action">The action to execute.</param>
        /// <typeparam name="T">Type of Task result.</typeparam>
        public static void ContinueOnUIThread<T>(this Task<T> task, Action<T> action)
        {
            task.ContinueWith(t => Execute.BeginOnUIThread(() =>
            {
                action(t.Result);
            })).ConfigureAwait(false);
        }
    }
}
