// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.Threading;
using UIKit;

namespace Softeq.XToolkit.Common.iOS
{
    /// <summary>
    ///     A <see cref="IMainThreadExecutor" /> implementation for the Xamarin iOS platform.
    /// </summary>
    public class IosMainThreadExecutor : IMainThreadExecutor
    {
        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void BeginOnUIThread(Action action)
        {
            if (CheckAccess())
            {
                action.Invoke();
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(action);
            }
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public Task OnUIThreadAsync(Action action)
        {
            if (CheckAccess())
            {
                action();
                return Task.CompletedTask;
            }
            var completionSource = new TaskCompletionSource<bool>();

            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
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
            });

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

        private bool CheckAccess()
        {
            return NSThread.IsMain;
        }
    }
}
