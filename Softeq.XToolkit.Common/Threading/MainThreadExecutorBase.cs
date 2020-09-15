// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Threading
{
    /// <summary>
    ///     Base <see cref="IMainThreadExecutor" /> implementation.
    /// </summary>
    public abstract class MainThreadExecutorBase : IMainThreadExecutor
    {
        /// <inheritdoc />
        public abstract bool IsMainThread { get; }

        /// <inheritdoc />
        public virtual void BeginOnUIThread(Action action)
        {
            if (IsMainThread)
            {
                action();
            }
            else
            {
                PlatformBeginInvokeOnMainThread(action);
            }
        }

        /// <inheritdoc />
        public virtual Task OnUIThreadAsync(Action action)
        {
            if (IsMainThread)
            {
                action();
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<bool>();

            PlatformBeginInvokeOnMainThread(() =>
            {
                try
                {
                    action();

                    tcs.SetResult(true);
                }
                catch (TaskCanceledException)
                {
                    tcs.SetCanceled();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <inheritdoc />
        public virtual void OnUIThread(Action action)
        {
            if (IsMainThread)
            {
                action();
            }
            else
            {
                OnUIThreadAsync(action).GetAwaiter().GetResult();
            }
        }

        protected abstract void PlatformBeginInvokeOnMainThread(Action action);
    }
}
