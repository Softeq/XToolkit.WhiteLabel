// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Softeq.XToolkit.Common.Threading;

namespace Softeq.XToolkit.Common.Droid
{
    /// <summary>
    ///     A <see cref="IMainThreadExecutor" /> implementation for the Xamarin Android platform.
    /// </summary>
    public class DroidMainThreadExecutor : IMainThreadExecutor
    {
        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void BeginOnUIThread(Action action)
        {
            Application.SynchronizationContext.Post(s => action(), null);
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public Task OnUIThreadAsync(Action action)
        {
            var completionSource = new TaskCompletionSource<bool>();

            Application.SynchronizationContext.Post(
                s =>
            {
                try
                {
                    action();

                    completionSource.SetResult(true);
                }
                catch (TaskCanceledException)
                {
                    completionSource.SetCanceled();
                }
                catch (Exception ex)
                {
                    completionSource.SetException(ex);
                }
            }, null);


            return completionSource.Task;
        }

        /// <summary>
        ///     Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void OnUIThread(Action action)
        {
            if (CheckAccess())
            {
                action();
            }
            else
            {
                OnUIThreadAsync(action).Wait();
            }
        }

        private static bool CheckAccess()
        {
            return SynchronizationContext.Current != null;
        }
    }
}
