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
        private static IMainThreadExecutor? _currentExecutor;

        /// <summary>
        ///     Gets the current <see cref="IMainThreadExecutor" />.
        /// </summary>
        public static IMainThreadExecutor CurrentExecutor =>
            _currentExecutor ?? throw new InvalidOperationException($"{nameof(CurrentExecutor)} isn't initialized.");

        /// <summary>
        ///     Initializes <see cref="Execute"/> class.
        /// </summary>
        /// <param name="executor">Instance to initialize.</param>
        public static void Initialize(IMainThreadExecutor executor)
        {
            _currentExecutor = executor ?? throw new ArgumentNullException();
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void BeginOnUIThread(this Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            CurrentExecutor.BeginOnUIThread(action);
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> representing the asynchronous operation.</returns>
        public static Task OnUIThreadAsync(this Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return CurrentExecutor.OnUIThreadAsync(action);
        }

        /// <summary>
        ///     Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void OnUIThread(this Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            CurrentExecutor.OnUIThread(action);
        }
    }
}
