// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms
{
    /// <summary>
    ///     A <see cref="IMainThreadExecutor" /> implementation for Xamarin.Forms.
    /// </summary>
    public class FormsMainThreadExecutor : IMainThreadExecutor
    {
        /// <inheritdoc />
        public bool IsMainThread => MainThread.IsMainThread;

        /// <inheritdoc />
        public void BeginOnUIThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        /// <inheritdoc />
        public Task OnUIThreadAsync(Action action)
        {
            return Device.InvokeOnMainThreadAsync(action);
        }

        /// <inheritdoc />
        public void OnUIThread(Action action)
        {
            OnUIThreadAsync(action).GetAwaiter().GetResult();
        }
    }
}
