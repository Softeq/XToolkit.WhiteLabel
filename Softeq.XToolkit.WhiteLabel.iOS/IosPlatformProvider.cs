// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    /// <summary>
    ///     A <see cref="IPlatformProvider" /> implementation for the Xamarin iOS platfrom.
    /// </summary>
    public class IosPlatformProvider : IPlatformProvider
    {
        /// <summary>
        ///     Indicates whether or not the framework is in design-time mode.
        /// </summary>
        public bool InDesignMode => false;

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